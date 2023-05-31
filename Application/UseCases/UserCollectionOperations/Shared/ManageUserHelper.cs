using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared
{
    public static class ManageUserHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static TSource MapAndFill<TSource, TTarget>(this TSource source, object target) where TSource : class
        {
            if(typeof(TSource) == typeof(UserCollection) && typeof(TTarget) == typeof(UpdateGameRequestModel))
            {
                var u = source.MapObjectTo(new UserCollection());
                var game = target as UpdateGameRequestModel;

                if (game.PurchaseDate == DateTime.MinValue) { u.PurchaseDate = game.PurchaseDate; }

                if (game.User_id == default) { u.UserId = game.User_id; }

                if (string.IsNullOrEmpty(game.Notes)) { u.Notes = game.Notes; }
                if (string.IsNullOrEmpty(game.Condition)) { u.Condition = Enum.Parse<Condition>(game.Condition); }
                if (string.IsNullOrEmpty(game.OwnershipStatus)) { u.OwnershipStatus = Enum.Parse<OwnershipStatus>(game.OwnershipStatus); }

                if (game.Game_id == 0) { u.GameId = game.Game_id; }
                if (game.Platform_id == 0 && game.PlatformIsComputer) { u.ComputerId = game.Platform_id; }
                if (game.Platform_id == 0 && !game.PlatformIsComputer) { u.ConsoleId = game.Platform_id; }

                return u as TSource;

            }
            if (typeof(TSource) == typeof(UserComputer))
            {
                var u = source.MapObjectTo(new UserComputer());
                var computer = target as UpdateComputerRequestModel;

                if (computer.PurchaseDate == DateTime.MinValue) { u.PurchaseDate = computer.PurchaseDate; }

                if (computer.User_id == default) { u.UserId = computer.User_id; }

                if (string.IsNullOrEmpty(computer.Notes)) { u.Notes = computer.Notes; }
                if (string.IsNullOrEmpty(computer.Condition)) { u.Condition = Enum.Parse<Condition>(computer.Condition); }
                if (string.IsNullOrEmpty(computer.OwnershipStatus)) { u.OwnershipStatus = Enum.Parse<OwnershipStatus>(computer.OwnershipStatus); }

                if (computer.Item_id == 0) { u.ComputerId = computer.Item_id; }

                return u as TSource;
            }
            if (typeof(TSource) == typeof(UserConsole))
            {
                var u = source.MapObjectTo(new UserConsole());
                var console = target as UpdateConsoleRequestModel;

                if (console.PurchaseDate == DateTime.MinValue) { u.PurchaseDate = console.PurchaseDate; }

                if (console.User_id == default) { u.UserId = console.User_id; }

                if (string.IsNullOrEmpty(console.Notes)) { u.Notes = console.Notes; }
                if (string.IsNullOrEmpty(console.Condition)) { u.Condition = Enum.Parse<Condition>(console.Condition); }
                if (string.IsNullOrEmpty(console.OwnershipStatus)) { u.OwnershipStatus = Enum.Parse<OwnershipStatus>(console.OwnershipStatus); }

                if (console.Item_id == 0) { u.ConsoleId = console.Item_id; }

                return u as TSource;
            }
            else
            {
                throw new ArgumentException($"Invalid Type: {nameof(source)}, ou {nameof(target)}");
            }
        }
    }
}
