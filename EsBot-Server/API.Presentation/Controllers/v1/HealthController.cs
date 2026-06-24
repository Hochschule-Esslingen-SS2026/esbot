using API.Presentation.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers.v1;

/// <summary>
/// Provides health check endpoints for the API.
/// </summary>
[ApiController]
[Route("v1/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Checks the health of the application and returns its version information.
    /// </summary>
    /// <returns>A <see cref="VersionResponseDto"/> containing the health status and version details.</returns>
    [HttpGet]
    public ActionResult<VersionResponseDto> HealthCheck()
    {
        var (major, minor, patch, commitHash) = VersionHelper.GetAppVersion();

        var response = new VersionResponseDto
        {
            Status = "Healthy",
            Major = major,
            Minor = minor,
            Patch = patch,
            CommitHash = commitHash
        };

        return Ok(response);
    }
}

/// <summary>
/// Represents the version information of the application along with its health status.
/// </summary>
public record VersionResponseDto
{
    /// <summary>
    /// The health status of the application.
    /// </summary>
    public required string Status { get; init; }

    /// <summary>
    /// The major version number.
    /// </summary>
    public required string Major { get; init; }

    /// <summary>
    /// The minor version number.
    /// </summary>
    public required string Minor { get; init; }

    /// <summary>
    /// The patch version number.
    /// </summary>
    public required string Patch { get; init; }

    /// <summary>
    /// The commit hash of the current build.
    /// </summary>
    public required string CommitHash { get; init; }
}
