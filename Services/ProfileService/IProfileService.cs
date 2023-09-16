using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Services.ProfileService;
public interface IProfileService
{
    Task<Profile> GetProfile(string username);
    Task<UnverifiedProfile> GetUnverifiedProfile(string username);
    Task<UnverifiedProfile> CreateProfile(UnverifiedProfile profile);
    Task<Profile> UpdateProfile(Profile profile);
    Task<Profile> VerifyProfile(string username, string verificationCode);
    Task AddResetPasswordRequest(ResetPasswordRequest request);
    Task ResetPassword(ChangedPasswordRequest request);
    Task DeleteProfile(string username);
}