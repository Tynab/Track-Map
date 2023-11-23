using TrackMap.Api.Entities;

namespace TrackMap.Api.Repositories;

public interface IUserRepository
{
    public ValueTask<IEnumerable<User>> GetAll();

    public ValueTask<User?> Get(Guid id);

    public ValueTask<User?> Create(User entity);

    public ValueTask<User?> Update(User entity);
}
