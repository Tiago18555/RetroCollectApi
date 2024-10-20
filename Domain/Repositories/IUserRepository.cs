using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IUserRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    Task<List<T>> GetAll<T>(Expression<Func<User, T>> predicate);


    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<User, bool> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    User SingleOrDefault(Func<User, bool> predicate);
    
    #region User Collections 
    //Task<List<T>> GetAllComputersByUser<T>(Guid userId, Expression<Func<UserComputer, T>> predicate)

    /// <exception cref="ArgumentNullException"></exception>
    Task<List<T>> GetAllComputersByUser<T>(Guid userId, Expression<Func<UserComputer, T>> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    Task<List<T>> GetAllComputersByUser<T>(Guid userId, Expression<Func<UserComputer, T>> predicate, int pageNumber, int pageSize);

    /// <exception cref="ArgumentNullException"></exception>
    Task<List<T>> GetAllConsolesByUser<T>(Guid userId, Expression<Func<UserConsole, T>> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    Task<List<T>> GetAllConsolesByUser<T>(Guid userId, Expression<Func<UserConsole, T>> predicate, int pageNumber, int pageSize);

    /// <exception cref="ArgumentNullException"></exception>
    Task<List<T>> GetAllCollectionsByUser<T>(Guid userId, Expression<Func<UserCollection, T>> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    Task<List<T>> GetAllCollectionsByUser<T>(Guid userId, Expression<Func<UserCollection, T>> predicate, int pageNumber, int pageSize);

    #endregion

}
