using Amazon.S3;
using Amazon.S3.Model;
using AmazonAppBackend.Exceptions.ImageExceptions;
using System.Net;

namespace AmazonAppBackend.Storage.ImageStore;

public class S3BucketStore : IImageStore
{
    private readonly string _bucketName;
    private readonly AmazonS3Client _s3Client;

    public S3BucketStore(AmazonS3Client s3Client, string bucketName)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }

    public async Task UploadImage(IFormFile image, string username)
    {
        var request = new PutObjectRequest()
        {
            BucketName = _bucketName,
            Key = username,
            InputStream = image.OpenReadStream(),
            ContentType = image.ContentType
        };
        await _s3Client.PutObjectAsync(request);
    }

    public async Task<Stream> DownloadImage(string username)
    {
        var request = new GetObjectRequest()
        {
            BucketName = _bucketName,
            Key = username
        };
        try
        {
            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception)
        {
            throw new ImageNotFoundException(username);
        }
    }

    public async Task DeleteImage(string username)
    {
        try
        {
            var checkRequest = new GetObjectMetadataRequest
            {
                BucketName = _bucketName,
                Key = username
            };
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = username
            };

            await Task.WhenAll(_s3Client.GetObjectMetadataAsync(checkRequest),
                            _s3Client.DeleteObjectAsync(deleteRequest));
        }
        catch (AmazonS3Exception e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ImageNotFoundException($"No image found with key {username}");
        }
    }
}