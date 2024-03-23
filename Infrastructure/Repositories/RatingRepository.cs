using Domain.Entities;
using Infrastructure.Data;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly DataContext _context;

        public RatingRepository(DataContext context) =>        
            _context = context;
        

        public Rating Add(Rating rating)
        {
            _context.Ratings.Add(rating);
            _context.SaveChanges();
            _context.Entry(rating).Reference(x => x.Game).Load();
            _context.Entry(rating).Reference(x => x.User).Load();
            _context.Entry(rating).State = EntityState.Detached;

            return rating;
        }    

        public bool Any(Func<Rating, bool> predicate)
        {
            return _context
                .Ratings
                .AsNoTracking()
                .Any(predicate);
        }
    

        public bool Delete(Rating rating)
        {
            _context.Ratings.Remove(rating);
            _context.SaveChanges();

            return !_context.Ratings.Any(x => x.RatingId == rating.RatingId);
        }

        public async Task<List<T>> GetRatingsByGame<T>(int gameId, Func<Rating, T> predicate)
        {
            return await Task.FromResult(_context.Ratings
                .Include(g => g.User)
                .AsNoTracking()
                .Where(x => x.GameId == gameId)
                .Select(predicate)
                .ToList());
        }

        public async Task<List<T>> GetRatingsByUser<T>(Guid userId, Func<Rating, T> predicate)
        {
            return await Task.FromResult( _context.Ratings
                .Include(g => g.Game)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(predicate)
                .ToList());
        }

        public Rating SingleOrDefault(Func<Rating, bool> predicate)
        {
            return _context
                .Ratings
                .Where(predicate)
                .AsQueryable()
                .AsNoTracking()
                .SingleOrDefault();
        }

        public Rating Update(Rating rating)
        {
            _context.Ratings.Update(rating);
            _context.Entry(rating).Reference(x => x.Game).Load();
            _context.Entry(rating).Reference(x => x.User).Load();
            _context.SaveChanges();

            return rating;
        }
    }

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

        public async Task<List<T>> GetWishlistsByGame<T>(int gameId, Func<Wishlist, T> predicate)
        {
            return await Task.FromResult(_context.Wishlists
                .Include(g => g.User)
                .AsNoTracking()
                .Where(x => x.GameId == gameId)
                .Select(predicate)
                .ToList());
        }

        public async Task<List<T>> GetWishlistsByUser<T>(Guid userId, Func<Wishlist, T> predicate)
        {
            return await Task.FromResult(_context.Wishlists
                .Include(g => g.Game)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(predicate)
                .ToList());
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
