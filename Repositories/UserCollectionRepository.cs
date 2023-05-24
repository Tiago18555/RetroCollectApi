using RetroCollect.Data;
using RetroCollect.Models;
using RetroCollectApi.Repositories.Interfaces;

namespace RetroCollectApi.Repositories
{
    public class UserCollectionRepository : IUserCollectionRepository
    {
        private readonly DataContext _context;

        public UserCollectionRepository(DataContext context)
        {
            _context = context;
        }

        public UserCollection Add(UserCollection user)
        {
            _context.UserCollections.Add(user);
            _context.SaveChanges();

            return _context.UserCollections
                .Where(x => x.UserCollectionId == user.UserCollectionId)
                .FirstOrDefault();
        }

        public bool Any(Func<UserCollection, bool> predicate)
        {
            return _context.UserCollections.Any(predicate);
        }

        public bool Delete(UserCollection user)
        {
            _context.UserCollections.Remove(user);

            return !_context.UserCollections.Any(x => x.UserCollectionId == user.UserCollectionId); //NONE MATCH
        }

        public UserCollection GetById(Guid id)
        {
            return _context.UserCollections.Where(x => x.UserCollectionId == id).FirstOrDefault();
        }

        public UserCollection SingleOrDefault(Func<UserCollection, bool> predicate)
        {
            return _context.UserCollections.Where(predicate).SingleOrDefault();
        }
    }
}
