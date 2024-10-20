using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IRatingRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<List<T>> GetRatingsByUser<T>(Guid userId, Expression<Func<Rating, T>> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<List<T>> GetRatingsByUser<T>(Guid userId, Expression<Func<Rating, T>> predicate, int pageNumber, int pageSize);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<List<T>> GetRatingsByGame<T>(int gameId, Expression<Func<Rating, T>> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<List<T>> GetRatingsByGame<T>(int gameId, Expression<Func<Rating, T>> predicate, int pageNumber, int pageSize);

    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<Rating, bool> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Rating SingleOrDefault(Func<Rating, bool> predicate);
}
