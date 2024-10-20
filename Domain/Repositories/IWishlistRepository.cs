using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IWishlistRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<List<T>> GetWishlistsByUser<T>(Guid userId, Expression<Func<Wishlist, T>> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<List<T>> GetWishlistsByUser<T>(Guid userId, Expression<Func<Wishlist, T>> predicate, int pageNumber, int pageSize);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<List<T>> GetWishlistsByGame<T>(int gameId, Expression<Func<Wishlist, T>> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task<List<T>> GetWishlistsByGame<T>(int gameId, Expression<Func<Wishlist, T>> predicate, int pageNumber, int pageSize);

    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<Wishlist, bool> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Wishlist SingleOrDefault(Func<Wishlist, bool> predicate);
}
