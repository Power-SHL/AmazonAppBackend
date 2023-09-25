using AmazonAppBackend.DTO.Profiles;
using System.Security.Claims;

namespace AmazonAppBackend.Services.AuthorizationService;
public interface IAuthorizationService
{
    Task<AuthenticationToken> AuthorizeUser(SignInRequest request);
    void AuthorizeRequest(ClaimsPrincipal user, string username);
    void AuthorizeRequest(ClaimsPrincipal user, List<string> usernames);
}