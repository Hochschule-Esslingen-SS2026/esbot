using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IQuizManagementService _quizManagementService;

    public QuizController(IQuizManagementService  quizManagementService)
    {
        _quizManagementService = quizManagementService;
    }
}