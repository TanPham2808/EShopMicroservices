
using Catalog.API.Data;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>                       // Đăng ký MediatR vào ứng dụng & cấu hình để đăng ký các dịch vụ từ assembly chứa chương trình chính
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));  // Add validate để check request rồi quăng throw ValidationException nếu có lỗi
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));     // Add write log trước quest và sau khi respone trả về
});

builder.Services.AddValidatorsFromAssembly(assembly);       // Đăng ký Fluent Library  

builder.Services.AddCarter();                               // Quản lý Enpoint API

builder.Services.AddMarten(opts =>                          // Đăng ký Marten tạo DB bằng Postgre 
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();       // Seeding Data cho CatalogDb
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();  // Tùy chỉnh Exception Response (Nhớ add thêm app.UseExceptionHandler)

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure to HTTP request pipeline
app.MapCarter();


app.UseExceptionHandler(options => { });  // Hanlde tùy chỉnh Exception Response (Đi 1 cặp chung với method AddExceptionHandler)

app.UseHealthChecks("/health",new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
