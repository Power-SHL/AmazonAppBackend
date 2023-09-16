using Amazon.S3;
using Amazon.S3.Model;
using AmazonAppBackend.Configuration.Clients;

namespace AmazonAppBackend.Storage.EmailStore;

public class S3BucketEmailStore : IEmailStore
{
    private readonly string _bucketName;
    private readonly AmazonS3Client _s3Client;

    public S3BucketEmailStore(EmailContentS3BucketConfig config)
    {
        _s3Client = config.S3Client;
        _bucketName = config.BucketName;
    }

    public async Task<string> GetEmailContent(string emailType)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = emailType
        };

        using var response = await _s3Client.GetObjectAsync(request);
        await using var responseStream = response.ResponseStream;
        using var reader = new StreamReader(responseStream);
        return await reader.ReadToEndAsync();
    }
}