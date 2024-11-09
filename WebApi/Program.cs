using Infrastructure.Repositories;

using Npgsql;
using System.Text;
using MongoDB.Driver;
using Domain.Repositories;
using Infrastructure.Data;
using CrossCutting.Providers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Application.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CrossCutting;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var configuration = builder.Configuration;
var version = configuration.GetSection("Version").Value.ToString();
var connectionString = configuration.GetConnectionString("Local");

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

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);



#region DEPENDENCY INJECTION SETUP

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserConsoleRepository, UserConsoleRepository>();
builder.Services.AddScoped<IUserComputerRepository, UserComputerRepository>();
builder.Services.AddScoped<IUserCollectionRepository, UserCollectionRepository>();

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IConsoleRepository, ConsoleRepository>();
builder.Services.AddScoped<IComputerRepository, ComputerRepository>();

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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(o =>
{
    o.UseNpgsql(connectionString);
    o.EnableSensitiveDataLogging();
});

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc(
        $"RetroCollect_v{version}", 
        new Microsoft.OpenApi.Models.OpenApiInfo { 
            Title = $"Tiago.RetroCollect", 
            Version = $"v{version}", 
            Description = String.Format(@"RetroCollect is a web application 
                that allows users to manage their collection 
                of vintage computers, gaming consoles, and video games. 
                It provides a user-friendly interface for users to track and organize their equipment, 
                add games to their collection, 
                and rate their gaming experience."
            )}
    );
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

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
await RunSqlScriptAsync(context);

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI(x => 
    {
        x.SwaggerEndpoint($"/swagger/RetroCollect_v{version}/swagger.json", $"RetroCollect v{version}");
        x.InjectStylesheet("/swagger-ui/SwaggerDark.css");
    });
}

if (app.Environment.IsDevelopment())
{
    builder.Configuration["BasePath"] = Path.Combine(
        Environment.CurrentDirectory,
        ".."
    );
}

app.UseStaticFiles();
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


async Task RunSqlScriptAsync(DataContext context)
{
    StdOut.Warning(        
        Path.Combine(
            configuration["BasePath"],
            "Static",
            "create_views.sql"
        ));

    var script = await File.ReadAllTextAsync(
        Path.Combine(
            configuration["BasePath"],
            "Static",
            "create_views.sql"
        )
    );
    await using var connection = new NpgsqlConnection(context.Database.GetDbConnection().ConnectionString);    
    await connection.OpenAsync();
    await using var command = new NpgsqlCommand(script, connection);   

    int result = await command.ExecuteNonQueryAsync();

    await connection.CloseAsync();        
}