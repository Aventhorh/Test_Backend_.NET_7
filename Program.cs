global using Dotnet7Learning.models;
global using Microsoft.AspNetCore.Mvc;
global using Dotnet7Learning.Services.PostService;
global using Dotnet7Learning.Dtos.Post;
global using Dotnet7Learning.Dtos.User;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using Dotnet7Learning.Data;
global using System.Security.Claims;
global using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IPostService, PostService>();

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