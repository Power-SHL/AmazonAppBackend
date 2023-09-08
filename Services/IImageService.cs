namespace AmazonAppBackend.Services;

public interface IImageService
{
    Task UploadImage(IFormFile image, string username);
    Task<byte[]> DownloadImage(string username);
    Task DeleteImage(string username);
}