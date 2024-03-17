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
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationExtension
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<ICreateUserService, CreateUserService>();
        services.AddScoped<IManageUserService, ManageUserService>();

        services.AddScoped<ISearchGameService, SearchGameService>();
        services.AddScoped<ISearchComputerService, SearchComputerService>();
        services.AddScoped<ISearchConsoleService, SearchConsoleService>();

        services.AddScoped<IVerifyAndRecoverUserService, VerifyAndRecoverUserService>();

        services.AddScoped<IManageGameCollectionService, ManageGameCollectionService>();
        services.AddScoped<IManageComputerCollectionService, ManageComputerCollectionService>();
        services.AddScoped<IManageConsoleCollectionService, ManageConsoleCollectionService>();

        services.AddScoped<IAddRatingService, AddRatingService>();
        services.AddScoped<IManageRatingService, ManageRatingService>();

        return services;
    }
}