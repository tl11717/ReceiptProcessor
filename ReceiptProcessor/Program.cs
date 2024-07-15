using Microsoft.EntityFrameworkCore;
using ReceiptProcessor.Data.Context;
using ReceiptProcessor.Data.Interfaces;
using ReceiptProcessor.Data.Repository;
using ReceiptProcessor.Models.Mapper;
using ReceiptProcessor.Services;
using ReceiptProcessor.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with DI
builder.Services.AddDbContext<ReceiptProcessorDBContext>(options =>
    options.UseInMemoryDatabase("ReceiptProcessorDb"));
builder.Services.AddScoped<IReceiptProcessorDBContext>(provider => provider.GetService<ReceiptProcessorDBContext>());

// Register repositories
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IReceiptPointRepository, ReceiptPointRepository>();

// Register services
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IReceiptPointService, ReceiptPointService>();

// Other service configurations
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
