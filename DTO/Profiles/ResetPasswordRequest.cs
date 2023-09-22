namespace AmazonAppBackend.DTO.Profiles;

public record ResetPasswordRequest
{
    public string Username { get; set; }
    public string Code { get; set; }
    public ResetPasswordRequest(string username, string code)
    {
        Username = username.ToLower();
        Code = code;
    }

    public Profile User;
}