using Amazon;
using Microsoft.EntityFrameworkCore;
using AmazonAppBackend.Data;
using AmazonAppBackend.Storage;
using AmazonAppBackend.Storage.FriendRequestStore;
using AmazonAppBackend.Storage.ImageStore;
using AmazonAppBackend.Storage.ProfileStore;
using Amazon.S3;
using AmazonAppBackend.Configuration;
using Microsoft.Extensions.Options;
using Amazon.Runtime;
using AmazonAppBackend.Configuration.Clients;
using AmazonAppBackend.Storage.EmailStore;
using AmazonAppBackend.Configuration.Settings;
using AmazonAppBackend.Services.FriendServices;
using AmazonAppBackend.Services.FriendService;
using AmazonAppBackend.Services.EmailService;
using AmazonAppBackend.Services.ProfileService;
using AmazonAppBackend.Services.ImageService;

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

builder.Services.Configure<AWSConfig>(builder.Configuration.GetSection("AWSConfig"));
builder.Services.Configure<S3ProfilePicConfig>(builder.Configuration.GetSection("S3ProfilePicConfig"));
builder.Services.AddScoped<IImageStore, ImageS3BucketStore>();
builder.Services.AddScoped(sp =>
{
    var awsConfig = sp.GetRequiredService<IOptions<AWSConfig>>().Value;
    var s3ProfilePicConfig = sp.GetRequiredService<IOptions<S3ProfilePicConfig>>().Value;
    var credentials = new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey);
    var s3Config = new AmazonS3Config()
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(s3ProfilePicConfig.RegionEndpoint)
    };
    return new ImageS3BucketConfig(new AmazonS3Client(credentials, s3Config), s3ProfilePicConfig.BucketName);
});
builder.Services.AddScoped<IImageStore, ImageS3BucketStore>();

builder.Services.Configure<S3EmailContentConfig>(builder.Configuration.GetSection("S3EmailContentConfig"));
builder.Services.AddScoped<IEmailService, MailKitService>();
builder.Services.AddScoped<IEmailStore, S3BucketEmailStore>();
builder.Services.AddScoped(sp =>
{
    var awsConfig = sp.GetRequiredService<IOptions<AWSConfig>>().Value;
    var s3EmailContentConfig = sp.GetRequiredService<IOptions<S3EmailContentConfig>>().Value;
    var credentials = new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey);
    var s3Config = new AmazonS3Config()
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(s3EmailContentConfig.RegionEndpoint)
    };
    return new EmailContentS3BucketConfig(new AmazonS3Client(credentials, s3Config), s3EmailContentConfig.BucketName);
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("GmailSettings"));
builder.Services.AddScoped(sp =>
{
    var gmailSettings = sp.GetRequiredService<IOptions<MailSettings>>().Value;
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