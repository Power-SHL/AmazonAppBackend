using System.ComponentModel.DataAnnotations;

namespace AmazonAppBackend.DTO.Profiles;

public record UnverifiedProfile
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string VerificationCode { get; set; }

    public UnverifiedProfile(string username, string email, string password, string firstName, string lastName, string verificationCode)
    {
        Username = username;
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        VerificationCode = verificationCode;
    }
    public UnverifiedProfile(Profile profile, string verificationCode)
    {
        Username = profile.Username;
        Email = profile.Email;
        Password = profile.Password;
        FirstName = profile.FirstName;
        LastName = profile.LastName;
        VerificationCode = verificationCode;
    }
}