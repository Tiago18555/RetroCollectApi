using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly DataContext _context;

    public WishlistRepository(DataContext context) =>
        _context = context;

    public bool Any(Func<Wishlist, bool> predicate)
    {
        return _context
            .Wishlists
            .AsNoTracking()
            .Any(predicate);
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
