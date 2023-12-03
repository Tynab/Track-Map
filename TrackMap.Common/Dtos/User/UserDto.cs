namespace TrackMap.Common.Dtos.User;

public sealed record UserDto
{
    public Guid Id { get; set; }

    public string? FullName { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
