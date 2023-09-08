namespace AmazonAppBackend.Storage.ImageStore;

public interface IImageStore
{
    Task UploadImage(IFormFile image, string username);
    Task<Stream> DownloadImage(string username);
    Task DeleteImage(string username);
}
