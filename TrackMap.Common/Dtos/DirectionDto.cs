using System.ComponentModel.DataAnnotations;

namespace TrackMap.Common.Dtos;

public sealed class DirectionDto
{
    [Required]
    public string? SourceLocation { get; set; }

    [Required]
    public string? DestinationLocation { get; set; }
}
