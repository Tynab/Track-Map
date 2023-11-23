namespace TrackMap.Common.Requests;

public sealed class LoginRequest
{
    public required string UserName { get; set; }

    public required string Password { get; set; }
}
