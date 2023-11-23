using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackMap.Api.Entities;

public sealed class Device
{
    [Key]
    public Guid Id { get; set; }

    public string? DeviceType { get; set; }

    public string? DeviceOs { get; set; }

    public string? IpAddress { get; set; }

    [Column(TypeName = "decimal(19,15)")]
    public decimal Latitude { get; set; }

    [Column(TypeName = "decimal(19,15)")]
    public decimal Longtitude { get; set; }

    public DateTime LastLogin { get; set; }

    public Guid UserId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
