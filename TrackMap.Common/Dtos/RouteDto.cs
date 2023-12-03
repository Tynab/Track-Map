using System.ComponentModel.DataAnnotations;

namespace TrackMap.Common.Dtos;

public sealed class RouteDto
{
    [Required]
    public decimal Latitude { get; set; }

    [Required]
    public decimal Longitude { get; set; }

    [Required]
    public string? DestinationLocation { get; set; }
}
