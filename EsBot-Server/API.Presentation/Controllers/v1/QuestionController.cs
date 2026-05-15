using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IChatService _questionManagementService;

    public QuestionController(IChatService  questionManagementService)
    {
        _questionManagementService = questionManagementService;
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> AskQuestion([FromBody] QuestionRequest question)
    {
        MessageResponse result = null;
        try
        {
            result = await _questionManagementService.AskQuestion(question);
            return Ok(result);
        }
        catch (TimeoutException e)
        {
            return StatusCode(524, "LLM timeout");
        }

    }
}
