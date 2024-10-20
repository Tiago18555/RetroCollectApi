using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly DataContext _context;

    public RatingRepository(DataContext context) =>        
        _context = context;  

    public bool Any(Func<Rating, bool> predicate)
    {
        return _context
            .Ratings
            .AsNoTracking()
            .Any(predicate);
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
}
