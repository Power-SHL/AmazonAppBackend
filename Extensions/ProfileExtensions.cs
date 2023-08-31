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
        List<Task> tasks = new();
        StringBuilder errorMessage = new();

        foreach (string username in usernames)
        {
            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    await profileStore.GetProfile(username);
                }
                catch
                {
                    errorMessage.Append(username + ", ");
                }
            }));
        }
        await Task.WhenAll(tasks);
        if (errorMessage.Length > 0)
        {
            throw new ProfileNotFoundException($"Profile(s) {errorMessage} do not exist.");
        }
    }
}