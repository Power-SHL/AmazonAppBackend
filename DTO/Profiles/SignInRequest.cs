using System.ComponentModel.DataAnnotations;

namespace AmazonAppBackend.DTO.Profiles;

public record SignInRequest
{
    public SignInRequest([Required] string loginString, [Required] string password)
    {
        LogInString = loginString;
        Password = password;
    }

    public string LogInString { get; set; }
    public string Password { get; set; }
}