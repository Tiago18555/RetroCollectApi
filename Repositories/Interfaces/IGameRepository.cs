﻿using RetroCollect.Models;

namespace RetroCollectApi.Repositories.Interfaces
{
    public interface IGameRepository
    {
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        Game Add(Game game);

        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<Game, bool> predicate);
    }
}
