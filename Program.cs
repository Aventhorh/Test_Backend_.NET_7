global using Test_Backend_NET_7.models;
global using Microsoft.AspNetCore.Mvc;
global using Test_Backend_NET_7.Services.PostService;
global using Test_Backend_NET_7.Dtos.Post;
global using Test_Backend_NET_7.Dtos.User;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using Test_Backend_NET_7.Data;
global using System.Security.Claims;
global using Microsoft.IdentityModel.Tokens;
global using Test_Backend_NET_7.Services.UserService;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.Filters;
global using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("Jwt:Token").Value!
            )),
    };
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();