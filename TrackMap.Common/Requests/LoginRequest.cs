using System.ComponentModel.DataAnnotations;

namespace TrackMap.Common.Requests;

public sealed class LoginRequest
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }
}
