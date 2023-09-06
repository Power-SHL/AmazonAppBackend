using Microsoft.EntityFrameworkCore;
using AmazonAppBackend.Data;
using AmazonAppBackend.Services;
using AmazonAppBackend.Storage;
using AmazonAppBackend.Storage.FriendRequestStore;
using AmazonAppBackend.Storage.ProfileStore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add singletons
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileStore, PostgreSqlStore>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<IFriendRequestStore, PostgreSqlStore>();
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }