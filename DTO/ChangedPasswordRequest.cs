using System.ComponentModel.DataAnnotations;
namespace AmazonAppBackend.DTO;

public record ChangedPasswordRequest
{
    public string Username { get; set; }
    public string Code { get; set; }
    public string Password { get; set; }
    public ChangedPasswordRequest([Required] string username, string code, string password)
    {
        Username = username.ToLower();
        Code = code;
        Password = password;
    }
}