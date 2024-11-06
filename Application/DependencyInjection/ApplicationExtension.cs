using Domain.Broker;
using Infrastructure.Kafka;
using Application.UseCases.IgdbIntegrationOperations.SearchGame;
using Application.UseCases.IgdbIntegrationOperations.SearchConsole;
using Application.UseCases.IgdbIntegrationOperations.SearchComputer;
using Application.UseCases.UserCollectionOperations.ManageConsoleCollection;
using Application.UseCases.UserCollectionOperations.ManageComputerCollection;
using Application.UseCases.UserOperations.CreateUser;
using Application.UseCases.UserOperations.ManageUser;
using Application.UseCases.UserOperations.Authenticate;
using Application.UseCases.UserOperations.VerifyAndRecoverUser;
using Application.UseCases.UserWishlistOperations;
using Application.UseCases.UserCollectionOperations.ManageGameCollection;

using Microsoft.Extensions.DependencyInjection;
using Application.UseCases.RatingOperations.AddRating;
using Application.UseCases.RatingOperations.ManageRating;

namespace Application.DependencyInjection;

public static class ApplicationExtension
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticateUsecase, AuthenticateUsecase>();
        services.AddScoped<ICreateUserUsecase, CreateUserUsecase>();
        services.AddScoped<IManageUserUsecase, ManageUserUsecase>();

        services.AddScoped<ISearchGameUsecase, SearchGameUsecase>();
        services.AddScoped<ISearchComputerUsecase, SearchComputerUsecase>();
        services.AddScoped<ISearchConsoleUsecase, SearchConsoleUsecase>();

        services.AddScoped<IVerifyAndRecoverUserUsecase, VerifyAndRecoverUserUsecase>();

        services.AddScoped<IManageGameCollectionUsecase, ManageGameCollectionUsecase>();
        services.AddScoped<IManageComputerCollectionUsecase, ManageComputerCollectionService>();
        services.AddScoped<IManageConsoleCollectionUsecase, ManageConsoleCollectionUsecase>();

        services.AddScoped<IAddRatingUsecase, AddRatingUsecase>();
        services.AddScoped<IManageRatingUsecase, ManageRatingUsecase>();

        services.AddScoped<IWishlistUsecase, WishlistUsecase>();

        return services;
    }

    public static IServiceCollection AddBrokerServices(this IServiceCollection services)
    {
        services.AddScoped<IProducerService, KafkaProducerService>();

        return services;
    }
}