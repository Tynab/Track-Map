namespace TrackMap.Features;

public sealed class PagingLink(int page, bool isEnable, string text)
{
    public string? Text { get; set; } = text;

    public int Page { get; set; } = page;

    public bool IsEnable { get; set; } = isEnable;

    public bool IsActive { get; set; }
}
