namespace TrackMap.Common.Dtos;

public sealed record DeviceDto
{
    public Guid Id { get; set; }

    public string? DeviceType { get; set; }

    public string? DeviceOs { get; set; }

    public string? IpAddress { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longtitude { get; set; }

    public DateTime LastLogin { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
