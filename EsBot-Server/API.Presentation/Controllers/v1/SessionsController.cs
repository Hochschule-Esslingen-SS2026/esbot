using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
[Produces("application/json")]
public class SessionsController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ISessionRepository _sessionRepository;
    private readonly IMapper _mapper;

    public SessionsController(
        IChatService chatService,
        ISessionRepository sessionRepository,
        IMapper mapper)
    {
        _chatService = chatService;
        _sessionRepository = sessionRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Starts a new learning session for a user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SessionResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequest request)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var session = await _chatService.CreateNewSession(request.UserId);

        var response = new SessionResponse(session.Id, session.ExternalUserId, session.CreatedAt);
        return CreatedAtAction(nameof(GetSessionHistory), new { sessionId = session.Id }, response);
    }

    /// <summary>
    /// Retrieves all active learning sessions for a specific user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SessionResponse>))]
    public async Task<IActionResult> GetUserSessions([FromQuery, Required] string userId)
    {
        var sessions = await _sessionRepository.GetSessionsByUserId(userId);

        var response = sessions.Select(s => new SessionResponse(s.Id, s.ExternalUserId, s.CreatedAt));
        return Ok(response);
    }

    /// <summary>
    /// Submits a user message and returns an AI-generated chat response.
    /// </summary>
    [HttpPost("{sessionId}/messages")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SubmitMessage(Guid sessionId, [FromBody] QuestionRequest request)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        string sanitizedContent = System.Net.WebUtility.HtmlEncode(request.Question.Trim());

        var questionRequest = new QuestionRequest
        {
            UserSessionId = sessionId,
            Question = sanitizedContent
        };

        var aiResponse = await _chatService.AskQuestion(questionRequest);

        if (aiResponse.Content.Contains("AI service is currently unavailable"))
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, aiResponse);
        }

        return Ok(aiResponse);
    }

    /// <summary>
    /// Retrieves the complete message history for a specified session.
    /// </summary>
    [HttpGet("{sessionId}/messages")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionHistory(Guid sessionId)
    {
        try
        {
            var messages = await _chatService.GetSession(sessionId);
            return Ok(messages);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Triggers the generation of practice quiz questions for a specific topic.
    /// </summary>
    [HttpPost("{sessionId}/quiz")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuizRequestResponse))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GenerateQuiz(Guid sessionId, [FromBody] CreateQuizRequest createQuizRequest)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        try
        {
            var quizResponse = await _chatService.RequestQuiz(createQuizRequest);
            return Ok(quizResponse);
        }
        catch (ServiceUnavailableException ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Submits a user's quiz answer and returns correctness feedback.
    /// </summary>
    [HttpPost("{sessionId}/quiz/{questionId}/answer")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EvaluationResultResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> EvaluateAnswer(Guid sessionId, Guid questionId, [FromBody] AnswerEvaluationRequest evaluationRequest)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var evaluationResult = await _chatService.EvaluateAnswer(evaluationRequest);
        return Ok(evaluationResult);
    }

    /// <summary>
    /// Permanently deletes a session and all its associated messages and quiz metadata.
    /// </summary>
    [HttpDelete("{sessionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSession(Guid sessionId)
    {
        var session = await _sessionRepository.GetSession(sessionId);
        if (session == null)
        {
            return NotFound(new { error = $"Session with ID {sessionId} not found." });
        }

        await _sessionRepository.DeleteSession(sessionId);

        return NoContent();
    }
}
