using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Storage.ProfileStore;
using System.Text;

namespace AmazonAppBackend.Extensions;
public static class ProfileExtensions
{
    public static async Task CheckUniqueEmail(this IProfileStore profileStore, string email)
    {
        if (await profileStore.CheckIfEmailTakenByVerified(email) || await profileStore.CheckIfEmailTakenByUnVerified(email))
        {
            throw new ProfileDuplicateException($"Email {email} already in use.");
        }
    }

    private static async Task<bool> CheckIfEmailTakenByVerified(this IProfileStore profileStore, string email)
    {
        try
        {
            await profileStore.GetProfileByEmail(email);
            return true;
        }
        catch (ProfileNotFoundException)
        {
            return false;
        }
    }

    private static async Task<bool> CheckIfEmailTakenByUnVerified(this IProfileStore profileStore, string email)
    {
        try
        {
            await profileStore.GetUnverifiedProfileByEmail(email);
            return true;
        }
        catch (ProfileNotFoundException)
        {
            return false;
        }
    }

    public static async Task CheckProfilesExist(this IProfileStore profileStore, List<string> usernames)
    {
        StringBuilder errorMessage = new();

        foreach (string username in usernames)
        {
            try
            {
                await profileStore.GetProfile(username);
            }
            catch (ProfileNotFoundException)
            {
                errorMessage.Append($"{username}, ");
            }
            
        }
        if (errorMessage.Length > 0)
        {
            throw new ProfileNotFoundException(errorMessage + $" users not found");
        }
    }
}