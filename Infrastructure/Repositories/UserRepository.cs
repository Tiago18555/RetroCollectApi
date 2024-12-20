﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq.Expressions;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<T>> GetAll<T>(Expression<Func<User, T>> predicate)
    {
        return await _context.Users
            .AsNoTracking()
            .Select(predicate)
            .ToListAsync();
    }

    public bool Any(Func<User, bool> predicate)
    {
        return _context.Users.AsNoTracking().Any(predicate);
    }

    public User SingleOrDefault(Func<User, bool> predicate)
    {
        return _context
            .Users
            .Where(predicate)
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefault();
    }

    public async Task<List<T>> GetAllComputersByUser<T>(Guid userId, Expression<Func<ComputerCollectionItem, T>> predicate)
    {
        return await _context.ComputerCollectionItems
            .Include(x => x.Computer)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(predicate)
            .ToListAsync();
    }

    public async Task<List<T>> GetAllComputersByUser<T>(Guid userId, Expression<Func<ComputerCollectionItem, T>> predicate, int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;

        return await _context.ComputerCollectionItems
                    .Include(x => x.Computer)
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Skip(offset)
                    .Take(pageSize)
                    .Select(predicate)
                    .ToListAsync();
    }

    public async Task<List<T>> GetAllConsolesByUser<T>(Guid userId, Expression<Func<ConsoleCollectionItem, T>> predicate)
    {
        return await Task.FromResult(_context.ConsoleCollectionItems
            .Include(x => x.Console)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(predicate)
            .ToList());
    }

    public async Task<List<T>> GetAllConsolesByUser<T>(Guid userId, Expression<Func<ConsoleCollectionItem, T>> predicate, int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;

        return await Task.FromResult(_context.ConsoleCollectionItems
            .Include(x => x.Console)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Skip(offset)
            .Take(pageSize)
            .Select(predicate)
            .ToList());
    }

    public async Task<List<T>> GetAllCollectionsByUser<T>(Guid userId, Expression<Func<GameCollectionItem, T>> predicate)
    {
        return await Task.FromResult(_context.GameCollectionItems
             .Include(x => x.Game)
             .AsNoTracking()
             .Where(x => x.UserId == userId)
             .Select(predicate)
             .ToList());
    }

    public async Task<List<T>> GetAllCollectionsByUser<T>(Guid userId, Expression<Func<GameCollectionItem, T>> predicate, int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;

        return await Task.FromResult(_context.GameCollectionItems
             .Include(x => x.Game)
             .AsNoTracking()
             .Where(x => x.UserId == userId)
             .Skip(offset)
             .Take(pageSize)
             .Select(predicate)
             .ToList());
    }
}
