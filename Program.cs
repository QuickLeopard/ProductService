using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using ProductService.Models;
using ProductService.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку контроллеров и API
builder.Services.AddControllers();

// Настройка PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
});

var app = builder.Build();

// Включаем Swagger даже в Production (можно оставить только для Development)
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
});

app.UseHttpsRedirection();
app.MapControllers();

app.UseHttpsRedirection();

// Применяем миграции БД
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();

