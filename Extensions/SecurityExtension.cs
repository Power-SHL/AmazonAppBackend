using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AmazonAppBackend.Exceptions.FriendExceptions;

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

    public static void ValidateProfile(this Profile profile)
    {
        StringBuilder errorMessage = new();

        if (!profile.Email.IsValidEmail())
        {
            errorMessage.Append("Email format is invalid.\n");
        }

        if (!profile.Password.IsValidPassword())
        {
            errorMessage.Append("Password format is invalid.\n");
        }

        if (!profile.Username.IsValidUsername())
        {
            errorMessage.Append("Username format is invalid.\n");
        }

        if (!profile.FirstName.IsValidName())
        {
            errorMessage.Append("First name format is invalid.\n");
        }

        if (!profile.LastName.IsValidName())
        {
            errorMessage.Append("Last name format is invalid.\n");
        }

        if (errorMessage.Length != 0)
        {
            throw new ProfileInvalidException(errorMessage.ToString());
        }
    }

    public static void ValidatePutProfile(this PutProfile putProfile)
    {
        StringBuilder errorMessage = new();

        if (!putProfile.FirstName.IsValidName())
        {
            errorMessage.Append("First name format is invalid.\n");
        }

        if (!putProfile.LastName.IsValidName())
        {
            errorMessage.Append("Last name format is invalid.\n");
        }

        if (errorMessage.Length != 0)
        {
            throw new ProfileInvalidException(errorMessage.ToString());
        }
    }

    public static void ValidateFriendRequest(this FriendRequest request)
    {
        StringBuilder errorMessage = new();
        if (request.Sender == request.Receiver)
        {
            errorMessage.Append("Sender and receiver cannot be the same.\n");
        }

        if (!request.Sender.IsValidUsername())
        {
            errorMessage.Append("First name format is invalid.\n");
        }

        if (!request.Receiver.IsValidUsername())
        {
            errorMessage.Append("Last name format is invalid.\n");
        }

        if (errorMessage.Length != 0)
        {
            throw new FriendRequestInvalidException(errorMessage.ToString());
        }
    }
}