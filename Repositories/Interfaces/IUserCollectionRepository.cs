using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using System.Linq;

namespace RetroCollectApi.Repositories.Interfaces
{
    public interface IUserCollectionRepository
    {
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        UserCollection Add(UserCollection user);

        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><see langword="true" /> if the entity has deleted successfully</returns>
        bool Delete(UserCollection user);

        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>returns the Entity found or the default value for entity</returns>
        UserCollection GetById(Guid id);

        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<UserCollection, bool> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        UserCollection SingleOrDefault(Func<UserCollection, bool> predicate);

        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        UserCollection Update(UserCollection user);
    }
}
