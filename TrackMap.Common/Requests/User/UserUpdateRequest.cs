using System.ComponentModel.DataAnnotations;

namespace TrackMap.Common.Requests.User;

public sealed class UserUpdateRequest
{
    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? FullName { get; set; }

    [Required]
    public Guid UpdatedBy { get; set; }
}
