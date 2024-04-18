using Carter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();                               // Quản lý Enpoint API


var app = builder.Build();

// Configure to HTTP request pipeline

app.Run();
