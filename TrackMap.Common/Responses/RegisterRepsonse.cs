namespace TrackMap.Common.Responses;

public sealed class RegisterRepsonse
{
    public bool Success { get; set; }

    public IEnumerable<string>? Errors { get; set; }
}
