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
builder.Services.AddSingleton<IProfileService, ProfileService>();
builder.Services.AddSingleton<IProfileStore, PostgreSqlStore>();
builder.Services.AddSingleton<IFriendService, FriendService>();
builder.Services.AddSingleton<IFriendRequestStore, PostgreSqlStore>();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PostgreSQL")));

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