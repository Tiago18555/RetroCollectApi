using RetroCollect.Models;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserOperations.ManageUser
{
    public static class ManageUserHelper
    {
        public static User MapAndFill(this User source, UpdateUserRequestModel target)
        {
            var user = source.MapObjectTo(new User());

            if (target.Username != null) { user.Username = target.Username; }
            if (target.Email != null) { user.Email = target.Email; }
            if (target.FirstName != null) { user.FirstName = target.FirstName; }
            if (target.LastName != null) { user.LastName = target.LastName; }

            user.UpdatedAt = DateTime.Now;

            return user;
        }
    }

}
