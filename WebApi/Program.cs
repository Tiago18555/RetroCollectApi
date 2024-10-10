using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Text;
using Infrastructure.Data;
using Domain.Repositories.Interfaces;
using Infrastructure.Repositories;
using CrossCutting.Providers;
using Microsoft.Extensions.FileProviders;

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

builder.Services.AddScoped<IRecoverRepository, MongoRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();

builder.Services.AddUseCases();
builder.Services.AddBrokerServices();

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
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    builder.Configuration["BasePath"] = Path.Combine(
        Environment.CurrentDirectory,
        ".."
    );
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Configuration["BasePath"]),
    RequestPath = "/static"
});

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