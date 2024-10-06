using Application.UseCases.GameOperations.AddRating;
using Application.UseCases.GameOperations.ManageRating;
using Application.UseCases.IgdbIntegrationOperations.SearchComputer;
using Application.UseCases.IgdbIntegrationOperations.SearchConsole;
using Application.UseCases.IgdbIntegrationOperations.SearchGame;
using Application.UseCases.UserCollectionOperations.AddItems;
using Application.UseCases.UserCollectionOperations.ManageComputerCollection;
using Application.UseCases.UserCollectionOperations.ManageConsoleCollection;
using Application.UseCases.UserOperations.Authenticate;
using Application.UseCases.UserOperations.CreateUser;
using Application.UseCases.UserOperations.ManageUser;
using Application.UseCases.UserOperations.VerifyAndRecoverUser;
using Application.UseCases.UserWishlistOperations;
using Microsoft.Extensions.DependencyInjection;

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

    public static IServiceCollection AddKafkaServices(this IServiceCollection services)
    {
        services.AddScoped<IKafkaConsumerService, KafkaConsumerService>();
        services.AddScoped<IKafkaProducerService, KafkaProducerService>();

        return services;
    }
}