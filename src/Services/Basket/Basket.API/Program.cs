using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.Behaviors;
using Carter;
using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();               // Quản lý Enpoint API

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

var app = builder.Build();

// Configure to HTTP request pipeline
app.MapCarter();                            // Đi chung với AddCarter() để ánh xạ Endpoint

app.Run();
