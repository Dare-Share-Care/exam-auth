using Ardalis.Specification;

namespace Auth.Web.Interfaces.Repositories;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    
}