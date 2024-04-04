using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>                       // Đăng ký MediatR vào ứng dụng & cấu hình để đăng ký các dịch vụ từ assembly chứa chương trình chính
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));  // Mở thêm 1 Behavior validate từ class ValidationBehavior
});

builder.Services.AddValidatorsFromAssembly(assembly);       // Đăng ký Fluent Library  

builder.Services.AddCarter();                               // Quản lý Enpoint API

builder.Services.AddMarten(opts =>                          // Đăng ký Marten tạo DB bằng Postgre 
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();



var app = builder.Build();

// Configure to HTTP request pipeline
app.MapCarter();

app.Run();
