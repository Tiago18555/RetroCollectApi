using RetroCollect.Models;
using Microsoft.EntityFrameworkCore;

namespace RetroCollectApi.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        Rating Add(Rating rating);

        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><see langword="true" /> if the entity has deleted successfully</returns>
        bool Delete(Rating rating);

        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        Rating Update(Rating rating);

        /// <exception cref="ArgumentNullException"></exception>
        Task<List<T>> GetRatingsByUser<T>(Guid userId, Func<Rating, T> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        Task<List<T>> GetRatingsByGame<T>(int gameId, Func<Rating, T> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<Rating, bool> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        Rating SingleOrDefault(Func<Rating, bool> predicate);
    }
}
