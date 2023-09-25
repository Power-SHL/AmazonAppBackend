using System.Text;
using Amazon;
using Microsoft.EntityFrameworkCore;
using AmazonAppBackend.Data;
using AmazonAppBackend.Storage;
using AmazonAppBackend.Storage.FriendRequestStore;
using AmazonAppBackend.Storage.ImageStore;
using AmazonAppBackend.Storage.ProfileStore;
using Amazon.S3;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AmazonAppBackend.Services.AuthorizationService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using AmazonAppBackend.Services.FeedService;
using AmazonAppBackend.Services.PostService;
using AmazonAppBackend.Storage.FeedStore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

builder.Services.AddAuthorization();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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

builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped(sp =>
{
    var jwtSettings = sp.GetRequiredService<IOptions<JWTConfiguration>>().Value;
    return jwtSettings;
});
builder.Services.AddScoped<IAuthorizationService, JWTAuthorizationService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Spotify";
})
.AddCookie()
.AddOAuth("Spotify", options =>
{
    options.ClientId = builder.Configuration["Spotify:ClientID"];
    options.ClientSecret = builder.Configuration["Spotify:ClientSecret"];
    options.CallbackPath = new PathString("/signin-spotify");
    options.AuthorizationEndpoint = "https://accounts.spotify.com/authorize";
    options.TokenEndpoint = "https://accounts.spotify.com/api/token";

    options.ClaimActions.MapJsonKey("urn:spotify:external_id", "id");
    options.ClaimActions.MapJsonKey("urn:spotify:display_name", "display_name");

    options.SaveTokens = true;
});

builder.Services.Configure<SpotifySettings>(builder.Configuration.GetSection("Spotify"));
builder.Services.AddScoped(sp =>
{
    var spotifySettings = sp.GetRequiredService<IOptions<SpotifySettings>>().Value;
    return spotifySettings;
});

builder.Services.AddScoped<ISpotifyService, SpotifyApiWebService>();
builder.Services.AddScoped<IFeedService, FeedService>();
builder.Services.AddScoped<IFeedStore, PostgreSqlStore>();


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

app.MapControllers();

app.Run();

public partial class Program { }