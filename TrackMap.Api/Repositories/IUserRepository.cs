using TrackMap.Api.Entities;
using TrackMap.Common.Dtos.User;
using TrackMap.Common.SeedWork;

namespace TrackMap.Api.Repositories;

public interface IUserRepository
{
    public ValueTask<IEnumerable<User>> GetAll();

    public ValueTask<User?> Get(Guid id);

    public ValueTask<PagedList<User>> Search(UserSearchDto dto);

    public ValueTask<User?> Create(User entity);

    public ValueTask<User?> Update(User entity);

    public ValueTask<User?> Delete(User entity);
}
