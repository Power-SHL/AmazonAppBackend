using AmazonAppBackend.Extensions;
using AmazonAppBackend.Storage.ImageStore;
using AmazonAppBackend.Storage.ProfileStore;

namespace AmazonAppBackend.Services;

public class ImageService : IImageService
{
    private readonly IImageStore _imageStore;
    private readonly IProfileStore _profileStore;

    public ImageService(IImageStore imageStore, IProfileStore profileStore)
    {
        _imageStore = imageStore;
        _profileStore = profileStore;
    }

    public async Task UploadImage(IFormFile image, string username)
    {
        await _profileStore.GetProfile(username);
        await _imageStore.UploadImage(image, username.HashSha256());
    }

    public async Task<byte[]> DownloadImage(string username)
    {
        var imageStream = await _imageStore.DownloadImage(username.HashSha256());
        using var ms = new MemoryStream();
        await imageStream.CopyToAsync(ms);
        return ms.ToArray(); ;
    }

    public async Task DeleteImage(string username)
    {
        await _imageStore.DeleteImage(username.HashSha256());
    }
}
