using EjApi.AccessData;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EjApi.Services;
using System.Text;
using EjApi.AccessData.Interface;
using EjApi.Services.Interface;
using Service.AuthenticationService;
using EjApi.AccessData.Repository;
using EjApi.Services.UserService;
using EjApi.Security;
using Microsoft.Extensions.Configuration;
using Configuration;
using static Org.BouncyCastle.Math.EC.ECCurve;
using AccessData.Interface;
using AccessData.Repository;
using Service.BookService;
using Service.Interface;
using Service.RentBookService;
using Service.RoleService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container(dependencies)

builder.Services.AddScoped<IHashService, BcryptHasher>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IRentBookRepository, RentBookRepository>();
builder.Services.AddScoped<IRentBookService, RentBookService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.Configure<BDConfiguration>(builder.Configuration.GetSection("connectionDb"));
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtSettings"));

/*
 * ................
 */

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))

    };
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("*")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
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

app.Run();