using AmazonAppBackend.DTO.Profiles;

namespace AmazonAppBackend.Services.ProfileService;
public interface IProfileService
{
    Task<Profile> GetProfile(string username);
    Task<Profile> GetProfileByEmail(string email);
    Task<UnverifiedProfile> GetUnverifiedProfile(string username);
    Task<UnverifiedProfile> CreateProfile(UnverifiedProfile profile);
    Task<Profile> UpdateProfile(Profile profile);
    Task<Profile> VerifyProfile(string username, string verificationCode);
    Task AddResetPasswordRequest(ResetPasswordRequest request);
    Task<ResetPasswordRequest> GetResetPasswordRequest(string username);
    Task ResetPassword(ChangedPasswordRequest request);
    Task DeleteProfile(string username);
}