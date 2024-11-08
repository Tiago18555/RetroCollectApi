using Application.UseCases.CollectionOperations.ManageComputerCollection;
using Application.UseCases.CollectionOperations.ManageConsoleCollection;
using Application.UseCases.CollectionOperations.ManageGameCollection;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.UseCases.CollectionOperations.Shared;

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
        if (typeof(TSource) == typeof(UpdateGameRequestModel) && typeof(TTarget) == typeof(GameCollectionItem))
        {
            var userCollection = target as GameCollectionItem;
            var request = source as UpdateGameRequestModel;

            if (request.PurchaseDate != DateTime.MinValue) { userCollection.PurchaseDate = request.PurchaseDate; }

            if (!string.IsNullOrEmpty(request.Notes)) { userCollection.Notes = request.Notes; }
            if (!string.IsNullOrEmpty(request.Condition)) { userCollection.Condition = Enum.Parse<Condition>(request.Condition); }
            if (!string.IsNullOrEmpty(request.OwnershipStatus)) { userCollection.OwnershipStatus = Enum.Parse<OwnershipStatus>(request.OwnershipStatus); }

            return userCollection as TTarget;

        }
        if (typeof(TSource) == typeof(UpdateComputerRequestModel) && typeof(TTarget) == typeof(ComputerCollectionItem))
        {
            var userCollection = target as GameCollectionItem;
            var computer = source as UpdateComputerRequestModel;

            if (computer.PurchaseDate != DateTime.MinValue) { userCollection.PurchaseDate = computer.PurchaseDate; }

            if (!string.IsNullOrEmpty(computer.Notes)) { userCollection.Notes = computer.Notes; }
            if (!string.IsNullOrEmpty(computer.Condition)) { userCollection.Condition = Enum.Parse<Condition>(computer.Condition); }
            if (!string.IsNullOrEmpty(computer.OwnershipStatus)) { userCollection.OwnershipStatus = Enum.Parse<OwnershipStatus>(computer.OwnershipStatus); }

            return userCollection as TTarget;
        }
        if (typeof(TSource) == typeof(UpdateConsoleRequestModel) && typeof(TTarget) == typeof(ConsoleCollectionItem))
        {
            var userCollection = target as GameCollectionItem;
            var console = source as UpdateConsoleRequestModel;

            if (console.PurchaseDate != DateTime.MinValue) { userCollection.PurchaseDate = console.PurchaseDate; }

            if (!string.IsNullOrEmpty(console.Notes)) { userCollection.Notes = console.Notes; }
            if (!string.IsNullOrEmpty(console.Condition)) { userCollection.Condition = Enum.Parse<Condition>(console.Condition); }
            if (!string.IsNullOrEmpty(console.OwnershipStatus)) { userCollection.OwnershipStatus = Enum.Parse<OwnershipStatus>(console.OwnershipStatus); }

            return userCollection as TTarget;
        }
        else
        {
            throw new InvalidClassTypeException($"Invalid Type: {nameof(source)}, or {nameof(target)}");
        }
    }
}
