var builder = WebApplication.CreateBuilder(args);

// Add services to the container

var app = builder.Build();

// Configure to HTTP request pipeline

app.Run();
