using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Storage.ProfileStore;
using System.Text;
using AmazonAppBackend.Exceptions.FriendExceptions;
using AmazonAppBackend.Services;

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