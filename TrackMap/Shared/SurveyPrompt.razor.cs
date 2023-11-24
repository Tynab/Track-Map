using Microsoft.AspNetCore.Components;

namespace TrackMap.Shared;

public sealed partial class SurveyPrompt
{
    [Parameter]
    public string? Title { get; set; }
}
