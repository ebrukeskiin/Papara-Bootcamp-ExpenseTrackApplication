using ExpenseTrack.Api;
using ExpenseTrack.Api.Impl.Command;
using ExpenseTrack.Api.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using static ExpenseTrack.Api.Impl.CQRS.ExpenseCategory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var connectionStringMsSql = builder.Configuration.GetConnectionString("MsSqlConnection");

// 2. DbContext'i servis olarak ekle
builder.Services.AddDbContext<MsSqlDbContext>(options =>
{
    options.UseSqlServer(connectionStringMsSql);
});

builder.Services.AddAutoMapper(typeof(MapperConfig).Assembly);


builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(CreateExpenseCategoryCommand).GetTypeInfo().Assembly));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
