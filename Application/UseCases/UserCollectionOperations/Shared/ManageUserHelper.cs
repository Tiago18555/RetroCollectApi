using Application.UseCases.UserCollectionOperations.ManageComputerCollection;
using Application.UseCases.UserCollectionOperations.ManageConsoleCollection;
using Application.UseCases.UserCollectionOperations.ManageGameCollection;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.UseCases.UserCollectionOperations.Shared;

public static class ManageUserCollectionHelper
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
    public static TTarget MapAndFill<TTarget, TSource>(this object target, TSource source) where TTarget : class //target banco / source request
    {
        if (typeof(TSource) == typeof(UpdateGameRequestModel) && typeof(TTarget) == typeof(UserCollection))
        {
            var userCollection = target as UserCollection;
            var request = source as UpdateGameRequestModel;

            if (request.PurchaseDate != DateTime.MinValue) { userCollection.PurchaseDate = request.PurchaseDate; }

            if (request.User_id != default) { userCollection.UserId = request.User_id; }

            if (!string.IsNullOrEmpty(request.Notes)) { userCollection.Notes = request.Notes; }
            if (!string.IsNullOrEmpty(request.Condition)) { userCollection.Condition = Enum.Parse<Condition>(request.Condition); }
            if (!string.IsNullOrEmpty(request.OwnershipStatus)) { userCollection.OwnershipStatus = Enum.Parse<OwnershipStatus>(request.OwnershipStatus); }

            if (request.Game_id != 0) { userCollection.GameId = request.Game_id; }
            if (request.Platform_id != 0 && request.PlatformIsComputer) { userCollection.ComputerId = request.Platform_id; }
            if (request.Platform_id != 0 && !request.PlatformIsComputer) { userCollection.ConsoleId = request.Platform_id; }


            return userCollection as TTarget;

        }
        if (typeof(TSource) == typeof(UpdateComputerRequestModel) && typeof(TTarget) == typeof(UserComputer))
        {
            var userCollection = target as UserCollection;
            var computer = source as UpdateComputerRequestModel;

            if (computer.PurchaseDate != DateTime.MinValue) { userCollection.PurchaseDate = computer.PurchaseDate; }

            if (computer.User_id != default) { userCollection.UserId = computer.User_id; }

            if (!string.IsNullOrEmpty(computer.Notes)) { userCollection.Notes = computer.Notes; }
            if (!string.IsNullOrEmpty(computer.Condition)) { userCollection.Condition = Enum.Parse<Condition>(computer.Condition); }
            if (!string.IsNullOrEmpty(computer.OwnershipStatus)) { userCollection.OwnershipStatus = Enum.Parse<OwnershipStatus>(computer.OwnershipStatus); }

            if (computer.Item_id != 0) { userCollection.ComputerId = computer.Item_id; }

            return userCollection as TTarget;
        }
        if (typeof(TSource) == typeof(UpdateConsoleRequestModel) && typeof(TTarget) == typeof(UserConsole))
        {
            var userCollection = target as UserCollection;
            var console = source as UpdateConsoleRequestModel;

            if (console.PurchaseDate != DateTime.MinValue) { userCollection.PurchaseDate = console.PurchaseDate; }

            if (console.User_id != default) { userCollection.UserId = console.User_id; }

            if (!string.IsNullOrEmpty(console.Notes)) { userCollection.Notes = console.Notes; }
            if (!string.IsNullOrEmpty(console.Condition)) { userCollection.Condition = Enum.Parse<Condition>(console.Condition); }
            if (!string.IsNullOrEmpty(console.OwnershipStatus)) { userCollection.OwnershipStatus = Enum.Parse<OwnershipStatus>(console.OwnershipStatus); }

            if (console.Item_id != 0) { userCollection.ConsoleId = console.Item_id; }

            return userCollection as TTarget;
        }
        else
        {
            throw new InvalidClassTypeException($"Invalid Type: {nameof(source)}, or {nameof(target)}");
        }
    }
}
