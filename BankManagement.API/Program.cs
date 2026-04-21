// 🎯 Configure DI, Middleware, Services
// Dependencies: All layers via project references
using BankManagement.Application.Mapping;
using BankManagement.Application.Services;
using BankManagement.Core.Interfaces;
using BankManagement.Infrastructure.Data;
using BankManagement.Infrastructure.Repositories;
using BankManagement.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Add Services to DI Container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context
builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔗 Register Repository & Unit of Work (Scoped = per request)
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 🔗 Register Application Services
builder.Services.AddScoped<IAccountService, AccountService>();

// 🔗 Register AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// 🔗 Enable CORS for MVC project to call API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC",
        policy => policy.WithOrigins("https://localhost:7002") // MVC project URL
                       .AllowAnyMethod()
                       .AllowAnyHeader());
});

var app = builder.Build();

// 🔄 Configure HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMVC"); // Enable CORS
app.UseAuthorization();
app.MapControllers();

app.Run();