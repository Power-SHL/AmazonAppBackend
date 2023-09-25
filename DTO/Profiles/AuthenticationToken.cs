namespace AmazonAppBackend.DTO.Profiles;

public class AuthenticationToken
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }

    public AuthenticationToken(string token, DateTime expiration)
    {
        Token = token;
        Expiration = expiration;
    }
}