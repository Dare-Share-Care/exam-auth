using Ardalis.Specification.EntityFrameworkCore;
using Auth.Web.Interfaces.Repositories;

namespace Auth.Web.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
{
    public readonly UserContext UserContext;

    public EfRepository(UserContext userContext) : base(userContext) =>
        this.UserContext = userContext;
}