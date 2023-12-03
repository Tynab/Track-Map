namespace TrackMap.Common.Requests.User;

public sealed class UserCreateRequest
{
    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public required string FullName { get; set; }

    public required Guid CreatedBy { get; set; }
}
