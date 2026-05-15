using System.ComponentModel.DataAnnotations;

namespace Core.Data.DTOs.Requests;

public record CreateSessionRequest([Required] string UserId);
