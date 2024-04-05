using Domain.Entities;
using Infrastructure.Data;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly DataContext _context;

        public WishlistRepository(DataContext context) =>
            _context = context;

        public Wishlist Add(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            _context.SaveChanges();
            _context.Entry(wishlist).Reference(x => x.Game).Load();
            _context.Entry(wishlist).Reference(x => x.User).Load();
            _context.Entry(wishlist).State = EntityState.Detached;

            return wishlist;
        }

        public bool Any(Func<Wishlist, bool> predicate)
        {
            return _context
                .Wishlists
                .AsNoTracking()
                .Any(predicate);
        }

        public bool Delete(Wishlist rating)
        {
            _context.Wishlists.Remove(rating);
            _context.SaveChanges();

            return !_context.Wishlists.Any(x => x.WishlistId == rating.WishlistId);
        }

        public async Task<List<T>> GetWishlistsByGame<T>(int gameId, Expression<Func<Wishlist, T>> predicate)
        {
            return await _context.Wishlists
                .Include(g => g.User)
                .AsNoTracking()
                .Where(x => x.GameId == gameId)
                .Select(predicate)
                .ToListAsync();
        }

        public async Task<List<T>> GetWishlistsByGame<T>(int gameId, Expression<Func<Wishlist, T>> predicate, int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;

            return await _context.Wishlists
                .Include(g => g.User)
                .AsNoTracking()
                .Where(x => x.GameId == gameId)
                .Skip(offset)
                .Take(pageSize)
                .Select(predicate)
                .ToListAsync();
        }

        public async Task<List<T>> GetWishlistsByUser<T>(Guid userId, Expression<Func<Wishlist, T>> predicate)
        {
            return await _context.Wishlists
                .Include(g => g.Game)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(predicate)
                .ToListAsync();
        }

        public async Task<List<T>> GetWishlistsByUser<T>(Guid userId, Expression<Func<Wishlist, T>> predicate, int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;

            return await _context.Wishlists
                .Include(g => g.Game)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Skip(offset)
                .Take(pageSize)
                .Select(predicate)
                .ToListAsync();
        }

        public Wishlist SingleOrDefault(Func<Wishlist, bool> predicate)
        {
            return _context
                .Wishlists
                .Where(predicate)
                .AsQueryable()
                .AsNoTracking()
                .SingleOrDefault();
        }
    }
}
