using Domain.Entities;

namespace Domain.Repositories.Interfaces
{
    public interface IWishlistRepository
    {
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        Wishlist Add(Wishlist wishlist);

        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><see langword="true" /> if the entity has deleted successfully</returns>
        bool Delete(Wishlist wishlist);

        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        Task<List<T>> GetWishlistsByUser<T>(Guid userId, Func<Wishlist, T> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        Task<List<T>> GetWishlistsByGame<T>(int gameId, Func<Wishlist, T> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<Wishlist, bool> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        Wishlist SingleOrDefault(Func<Wishlist, bool> predicate);
    }
}
