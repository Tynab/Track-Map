using TrackMap.Common.SeedWork;

namespace TrackMap.Common.Dtos.User;

public sealed class UserSearchDto : PagingParameters
{
    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? UserName { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }
}
