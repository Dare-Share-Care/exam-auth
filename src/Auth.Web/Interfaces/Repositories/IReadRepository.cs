using Ardalis.Specification;

namespace Auth.Web.Interfaces.Repositories;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
    
}