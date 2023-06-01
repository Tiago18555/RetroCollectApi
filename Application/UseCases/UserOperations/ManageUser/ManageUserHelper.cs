using RetroCollect.Models;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserOperations.ManageUser
{
    public static class ManageUserHelper
    {
        public static User MapAndFill(this User target, UpdateUserRequestModel source)
        {
            if (source.Username != null) { target.Username = source.Username; }
            if (source.Email != null) { target.Email = source.Email; }
            if (source.FirstName != null) { target.FirstName = source.FirstName; }
            if (source.LastName != null) { target.LastName = source.LastName; }

            target.UpdatedAt = DateTime.Now;

            return target;
        }
    }

}
