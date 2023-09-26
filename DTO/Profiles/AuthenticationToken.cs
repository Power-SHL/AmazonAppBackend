namespace AmazonAppBackend.DTO.Profiles;

public class AuthenticationToken
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string Username { get; set; }

    public AuthenticationToken(string token, DateTime expiration, string username)
    {
        Token = token;
        Expiration = expiration;
        Username = username;
    }
}