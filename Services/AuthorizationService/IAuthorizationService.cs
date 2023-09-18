using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Services.AuthorizationService;
public interface IAuthorizationService
{
    Task<AuthenticationToken> AuthorizeUser(SignInRequest request);
}