using Entities.Models;
using Hangfire;
using Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Services;
using Persistence.Context;
using UnitOfWorks.UnitOfWorks;
using Mappings;
using Infrastructure.Repositories;
using Application.Commands;
using Infrastructure.Configuration;
using MediatR;
using Application.Queries.GetCatsPaged;
using FluentValidation;
using Application.Queries.GetCatsByTag;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<TheCatApiOptions>(
    builder.Configuration.GetSection("TheCatApi"));

builder.Services.AddHttpClient();
builder.Services.AddDbContext<CatsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
// Add Hangfire services
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHangfireServer(); // This starts the Hangfire job server

// Optional: register job client
//builder.Services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(FetchCatsCommand).Assembly));
builder.Services.AddAutoMapper(typeof(Mapper).Assembly);
builder.Services.AddScoped<ICatFetcherService, CatApiService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICatRepository, CatRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<FetchCatsJob>();

builder.Services.AddValidatorsFromAssemblyContaining<GetCatsQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetCatsByTagQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<FetchCatsCommandValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<Cats.Middleware.ExceptionHandling>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
