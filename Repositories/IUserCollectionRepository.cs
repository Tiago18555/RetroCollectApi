using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;

namespace RetroCollectApi.Repositories
{
    public interface IUserCollectionRepository
    {
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        UserCollection Add(UserCollection user);
        bool Delete(UserCollection user); //TRY VOID
        UserCollection GetById(Guid id);

        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<UserCollection, bool> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        UserCollection SingleOrDefault(Func<UserCollection, bool> predicate);
    }
}
