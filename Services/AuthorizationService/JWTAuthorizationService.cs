using Amazon.S3.Model;
using AmazonAppBackend.Configuration.Settings;
using AmazonAppBackend.DTO.Profiles;
using AmazonAppBackend.Exceptions.AuthenticationExceptions;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Services.ProfileService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmazonAppBackend.Services.AuthorizationService;

public class JWTAuthorizationService : IAuthorizationService
{

    private readonly IProfileService _profileService;
    private readonly JWTConfiguration _config;

    public JWTAuthorizationService(IProfileService profileService, JWTConfiguration config)
    {
        _profileService = profileService;
        _config = config;
    }

    public void AuthorizeRequest(ClaimsPrincipal user, string username)
    {
        string? usernameClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (usernameClaim is null || usernameClaim != username)
        {
            throw new UnauthorizedAccessException($"{username} is not authorized to perform this action");
        }
    }

    public void AuthorizeRequest(ClaimsPrincipal user, List<string> usernames)
    {
        foreach(string username in usernames)
        {
            try
            {
                AuthorizeRequest(user, username);
                return;
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }
        }
        throw new UnauthorizedAccessException($"You are not authorized to perform this action");
    }

    public async Task<AuthenticationToken> AuthorizeUser(SignInRequest request)
    {
        Profile profile;
        if (request.LogInString.IsValidEmail())
        {
            profile = await _profileService.GetProfileByEmail(request.LogInString);
        }
        else if (request.LogInString.IsValidUsername())
        {
            profile = await _profileService.GetProfile(request.LogInString);
        }
        else
        {
            throw new ProfileInvalidException("Invalid username or email");
        }

        if (request.Password.BCryptVerify(profile.Password))
        {
            var claims = new[] {
                        new Claim(ClaimTypes.Name, profile.Username)
                };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: signinCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var expiration = jwtSecurityToken.ValidTo;

            return new AuthenticationToken(token, expiration);
        }
        throw new IncorrectPasswordException("Entered password is incorrect");
    }
}