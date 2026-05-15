using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;

namespace API.Presentation.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IChatService _quizManagementService;

    public QuizController(IChatService quizManagementService)
    {
        _quizManagementService = quizManagementService;
    }

    [HttpPost]
    public async Task<ActionResult<QuizRequestResponse>> RequestQuiz([FromBody] CreateQuizRequest request)
    {
        if (request.Topic.Equals("NSFW", StringComparison.OrdinalIgnoreCase))
            return StatusCode(402);

        var result = await _quizManagementService.RequestQuiz(request);
        return Ok(result);
    }
}
