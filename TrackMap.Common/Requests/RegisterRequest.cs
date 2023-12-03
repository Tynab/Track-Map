using System.ComponentModel.DataAnnotations;

namespace TrackMap.Common.Requests;

public sealed class RegisterRequest
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    public string? ConfirmPassword { get; set; }
}
