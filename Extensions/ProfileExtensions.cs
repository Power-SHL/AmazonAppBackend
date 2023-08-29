using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Storage.ProfileStore;
using System.Text;

namespace AmazonAppBackend.Extensions;

public static class ProfileExtensions
{
    public static async Task CheckUniqueEmail(this IProfileStore profileStore, string email)
    {
        try
        {
            await profileStore.GetProfileByEmail(email);
            throw new ProfileAlreadyExistsException($"Email {email} is taken.");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return;
            }
            throw;
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
}
