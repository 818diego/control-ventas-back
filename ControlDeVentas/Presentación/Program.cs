using Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("Conexion");
builder.Services.AddDbContext<DBContextSistema>(options=>options.UseSqlServer(connectionString));
builder.Services.AddCors(option =>
{
    option.AddPolicy("Todos",builder=>builder.WithOrigins("*").WithHeaders("*").WithMethods("*"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Todos");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
