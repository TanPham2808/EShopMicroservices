using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ---------------
// Infrastructure - EF Core
// Application - MediatoR
// API - Carter, HealthChecks,...

builder.Services
      .AddApplicationServices(builder.Configuration)
      .AddInfrastructureServices(builder.Configuration)
      .AddApiServices(builder.Configuration);

// ---------------

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();           // Đi chung 1 cặp với AddApiServices()

// Khởi tạo data nếu là môi trường develop
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();
