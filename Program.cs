using Amazon;
using Microsoft.EntityFrameworkCore;
using AmazonAppBackend.Data;
using AmazonAppBackend.Services;
using AmazonAppBackend.Storage;
using AmazonAppBackend.Storage.FriendRequestStore;
using AmazonAppBackend.Storage.ImageStore;
using AmazonAppBackend.Storage.ProfileStore;
using Amazon.S3;
using AmazonAppBackend.Configuration;
using Microsoft.Extensions.Options;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add scoped elements
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileStore, PostgreSqlStore>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<IFriendRequestStore, PostgreSqlStore>();
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.Configure<S3BucketSettings>(builder.Configuration.GetSection("AmazonS3Config"));
builder.Services.AddScoped<IImageStore, S3BucketStore>();
builder.Services.AddScoped(sp =>
{
    var s3BucketOptions = sp.GetRequiredService<IOptions<S3BucketSettings>>().Value;
    var credentials = new BasicAWSCredentials(s3BucketOptions.AccessKey, s3BucketOptions.SecretKey);
    var s3Config = new AmazonS3Config()
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(s3BucketOptions.RegionEndpoint)
    };
    return new AmazonS3Client(credentials, s3Config);
});
builder.Services.AddScoped(sp =>
{
    var s3BucketOptions = sp.GetRequiredService<IOptions<S3BucketSettings>>().Value;
    return s3BucketOptions.BucketName;
});

builder.Services.Configure<GmailSettings>(builder.Configuration.GetSection("GmailSettings"));
builder.Services.AddScoped<IEmailService, GmailService>();
builder.Services.AddScoped(sp =>
{
    var gmailSettings = sp.GetRequiredService<IOptions<GmailSettings>>().Value;
    return gmailSettings;
});

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