using Amazon.S3.Model;
using AmazonAppBackend.Configuration.Settings;
using AmazonAppBackend.DTO;
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

        if (!request.Password.BCryptVerify(profile.Password))
        {
            var claims = new[] {
                        new Claim(ClaimTypes.Email, profile.Email)
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