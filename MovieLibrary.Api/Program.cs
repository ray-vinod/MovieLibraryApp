using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieLibraryApp.MovieLibrary.Api.Data;
using MovieLibraryApp.MovieLibrary.Api.Model.Auth;

var builder = WebApplication.CreateBuilder(args);
{
    // Database connection string
    builder.Services.AddDbContext<MovieLibrayDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddOpenApi();

    // Configure Identity
    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<MovieLibrayDbContext>()
    .AddDefaultTokenProviders();

    // Configure Identity options
    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;

        // User settings
        options.User.RequireUniqueEmail = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.AllowedForNewUsers = true;
        options.SignIn.RequireConfirmedEmail = false; // set to on production
    });

    // jwt authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            ClockSkew = TimeSpan.Zero // Remove delay of token when expire
        };
    });

    builder.Services.AddAuthorization();
    builder.Services.AddEndpointsApiExplorer();

    // builder.Services.AddAuthorization(options =>
    // {
    //     options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
    //     options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
    // });



}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();
}

app.Run();