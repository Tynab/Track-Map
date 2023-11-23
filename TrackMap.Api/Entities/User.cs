using Microsoft.AspNetCore.Identity;

namespace TrackMap.Api.Entities;

public sealed class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public ICollection<Device>? Devices { get; set; }
}
