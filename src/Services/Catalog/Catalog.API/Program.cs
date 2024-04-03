var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();          // Quản lý Enpoint API
builder.Services.AddMediatR(config =>  // Đăng ký MediatR vào ứng dụng & cấu hình để đăng ký các dịch vụ từ assembly chứa chương trình chính
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();

// Configure to HTTP request pipeline
app.MapCarter();

app.Run();
