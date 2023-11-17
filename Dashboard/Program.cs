using System.Text;
using Dashboard.Models;
using Dashboard.Repositories;
using Dashboard.Repositories.Impl;
using Dashboard.Services;
using Dashboard.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injection dependencies

// Repositories
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IGamertagRepository, GamertagRepository>();
builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

// Services
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITftService, TftService>();
builder.Services.AddTransient<ILoLService, LoLService>();
builder.Services.AddTransient<ILoRService, LoRService>();
builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddTransient<IVerifyGamertagService, VerifyGamertagService>();

// DB Context
builder.Services.AddDbContext<TheTrackingFellowshipContext>(options =>
    options.UseMySQL(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                     throw new MissingFieldException("Missing Environment Variable for mySQL DB connection string")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policyBuilder => policyBuilder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Authentication Token Bearer
var authKey = Environment.GetEnvironmentVariable("JWT_SECURITY_KEY") ??
              throw new ArgumentNullException("No JWT_SECURITY_KEY");

builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
    item.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = 401;
            context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            await context.Response.WriteAsync(JsonConvert.SerializeObject("Token invalid"));
        }
    };
});

builder.Services.Configure<JwtSettings>(settings => { settings.Securitykey = authKey; });

// App Build
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();