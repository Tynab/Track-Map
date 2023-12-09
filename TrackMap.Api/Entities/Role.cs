using Microsoft.AspNetCore.Identity;

namespace TrackMap.Api.Entities;

public sealed class Role : IdentityRole<Guid>
{
    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
