using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchComputer;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchConsole;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
using RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems;
using RetroCollectApi.Application.UseCases.UserOperations.Authenticate;
using RetroCollectApi.Application.UseCases.UserOperations.CreateUser;
using RetroCollectApi.Application.UseCases.UserOperations.ManageUser;
using RetroCollectApi.Repositories.Interfaces;
using RetroCollectApi.CrossCutting.Providers;
using RetroCollectApi.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using RetroCollect.Data;
using MongoDB.Driver;
using System.Text;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("Local");


//JWT
var symmetricalKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:ValidIssuer"],
        ValidAudience = configuration["Jwt:ValidAudience"],
        IssuerSigningKey = symmetricalKey
    };
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);



#region DEPENDENCY INJECTION SETUP

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserCollectionRepository, UserCollectionRepository>();
builder.Services.AddScoped<IUserComputerRepository, UserComputerRepository>();
builder.Services.AddScoped<IUserConsoleRepository, UserConsoleRepository>();

builder.Services.AddScoped<IConsoleRepository, ConsoleRepository>();
builder.Services.AddScoped<IComputerRepository, ComputerRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

builder.Services.AddScoped<IRecoverRepository, RecoverRepository>();

//Services
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<ICreateUserService, CreateUserService>();
builder.Services.AddScoped<IManageUserService, ManageUserService>();

builder.Services.AddScoped<ISearchGameService, SearchGameService>();
builder.Services.AddScoped<ISearchComputerService, SearchComputerService>();
builder.Services.AddScoped<ISearchConsoleService, SearchConsoleService>();

builder.Services.AddScoped<IVerifyAndRecoverUserService, VerifyAndRecoverUserService>();

builder.Services.AddScoped<IManageGameCollectionService, ManageGameCollectionService>();
builder.Services.AddScoped<IManageComputerCollectionService, ManageComputerCollectionService>();
builder.Services.AddScoped<IManageConsoleCollectionService, ManageConsoleCollectionService>();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddSingleton<IMongoDatabase>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetSection("MongoDB")["ConnectionString"];
    var databaseName = configuration.GetSection("MongoDatabaseName")["DatabaseName"];
    var client = new MongoClient(connectionString);
    return client.GetDatabase(databaseName);
});
builder.Services.AddScoped<MongoDBContext>();

#endregion


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(o =>
{
    o.UseNpgsql(connectionString);
    o.EnableSensitiveDataLogging();
});


builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "RetroCollect Api", Version = "v1" });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});


builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

System.Console.ForegroundColor = ConsoleColor.Green;
System.Console.WriteLine("\t __  ___ ___ __   _      _   _          ___  _ ___ \r\n\t )_) )_   )  )_) / )    / ` / ) )   )   )_  / ` )  \r\n\t/ \\ (__  (  / \\ (_/    (_. (_/ (__ (__ (__ (_. (  \n\n");
System.Console.ForegroundColor = ConsoleColor.White;


app.Run();