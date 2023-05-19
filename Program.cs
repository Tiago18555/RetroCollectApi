using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RetroCollect.Data;
using RetroCollectApi.Application.UseCases.UserOperations.Authenticate;
using RetroCollectApi.Application.UseCases.UserOperations.CreateUser;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("Local");


//JWT
var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:ValidIssuer"],
        ValidAudience = configuration["Jwt:ValidAudience"],
        IssuerSigningKey = chaveSimetrica
    };
});

// Add services to the container.
builder.Services.AddControllers();

//Dependency Injection
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<ICreateUserService, CreateUserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "RetroCollect Api", Version = "v1" });

});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});


builder.Services.AddCors(c => {
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

app.UseAuthorization();

app.MapControllers();

System.Console.WriteLine(" __  ___ ___ __   _      _   _          ___  _ ___ \r\n )_) )_   )  )_) / )    / ` / ) )   )   )_  / ` )  \r\n/ \\ (__  (  / \\ (_/    (_. (_/ (__ (__ (__ (_. (  \n\n");

app.Run();

