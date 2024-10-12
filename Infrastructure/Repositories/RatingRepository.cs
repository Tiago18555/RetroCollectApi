using Domain.Entities;
using Application.Data;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using System.Linq;
using Domain.Enums;
using System.Linq.Expressions;

namespace Application.Repositories
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

        public async Task<List<T>> GetRatingsByGame<T>(int gameId, Expression<Func<Rating, T>> predicate)
        {
            return await _context.Ratings
                .Include(g => g.User)
                .AsNoTracking()
                .Where(x => x.GameId == gameId)
                .Select(predicate)
                .ToListAsync();
        }

        public async Task<List<T>> GetRatingsByGame<T>(int gameId, Expression<Func<Rating, T>> predicate, int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;

            return await _context.Ratings
                .Include(g => g.User)
                .AsNoTracking()
                .Where(x => x.GameId == gameId)
                .Skip(offset)
                .Take(pageSize)
                .Select(predicate)
                .ToListAsync();
        }

        public async Task<List<T>> GetRatingsByUser<T>(Guid userId, Expression<Func<Rating, T>> predicate)
        {
            return await _context.Ratings
                .Include(g => g.Game)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(predicate)
                .ToListAsync();
        }

        public async Task<List<T>> GetRatingsByUser<T>(Guid userId, Expression<Func<Rating, T>> predicate, int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;

            return await _context.Ratings
                .Include(g => g.Game)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Skip(offset)
                .Take(pageSize)
                .Select(predicate)
                .ToListAsync();
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
}
