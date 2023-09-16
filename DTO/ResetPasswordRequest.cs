using System.ComponentModel.DataAnnotations;

namespace AmazonAppBackend.DTO;

public record ResetPasswordRequest
{
    public string Username { get; set; }
    public string Code { get; set; }

    public ResetPasswordRequest([Required] string username, string code)
    {
        Username = username.ToLower();
        Code = code;
    }

    public Profile User;
}