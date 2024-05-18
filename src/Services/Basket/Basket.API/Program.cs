using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.CustomException;
using Carter;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();                               // Quản lý Enpoint API

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>                       // Đăng ký MediatR vào ứng dụng
{
    config.RegisterServicesFromAssembly(assembly);          // Cấu hình để đăng ký các dịch vụ từ assembly chứa chương trình chính
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));  // Add validate để check request rồi quăng throw ValidationException nếu có lỗi
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));     // Add write log trước quest và sau khi respone trả về
});

// Đăng ký thư viện Marten tạo DB bằng Postgre và lấy UserName làm key
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

// Đăng ký Service Scope cho Repository
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Đăng ký Service cho CachedBasketRepository (Có sử dụng Scruto)
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// Add Services sử dụng Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket";
});

// Grpc Service (Đăng ký)
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();  // Tùy chỉnh Exception Response (Nhớ add thêm app.UseExceptionHandler)

// Check sức khỏe cho Database & Redis
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

// Configure to HTTP request pipeline
app.MapCarter();                            // Đi chung với AddCarter() để ánh xạ Endpoint

app.UseExceptionHandler(options => { });  // Hanlde tùy chỉnh Exception Response (Đi 1 cặp chung với method AddExceptionHandler)

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
