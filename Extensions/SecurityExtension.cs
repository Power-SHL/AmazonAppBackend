using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AmazonAppBackend.Extensions;
public static class SecurityExtension
{
    public static string HashSha256(this string input)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = sha256.ComputeHash(inputBytes);

        StringBuilder builder = new();
        foreach (byte hashByte in hashBytes)
        {
            builder.Append(hashByte.ToString("x2"));
        }

        return builder.ToString();
    }

    public static string BCryptHash(this string input)
    {
        return BCrypt.Net.BCrypt.HashPassword(input);
    }

    public static bool BCryptVerify(this string input, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(input, hash);
    }

    public static bool IsValidEmail(this string email)
    {
        try
        {
            var address = new System.Net.Mail.MailAddress(email);
            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }
    public static bool IsValidPassword(this string password)
    {

        try
        {
            return Regex.IsMatch(password,
                @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public static bool IsValidUsername(this string username)
    {
        try
        {
            return Regex.IsMatch(username,
                @"^(?![0-9]+$)[A-Za-z0-9_]{2,30}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public static bool IsValidName(this string name)
    {
        try
        {
            return Regex.IsMatch(name,
                @"^[A-Za-z][A-Za-z\s'-]{1,29}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}