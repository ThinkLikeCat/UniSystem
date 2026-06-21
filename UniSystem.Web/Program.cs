using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniSystem.Application.Common.Behaviors;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Application.Users.Commands.RegisterUser;
using UniSystem.Domain.Entities;
using UniSystem.Infrastructure;
using UniSystem.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UniSystemDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<UniSystemDbContext>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserCommand).Assembly);

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IApplicationDbContext, UniSystemDbContext>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 5001;
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await SeedRoles.InitializeAsync(scope.ServiceProvider);
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

await app.RunAsync();