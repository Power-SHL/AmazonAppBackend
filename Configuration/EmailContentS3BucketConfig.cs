using Amazon.S3;
namespace AmazonAppBackend.Configuration;

public class EmailContentS3BucketConfig
{
    public AmazonS3Client S3Client { get; set; }
    public string BucketName { get; set; }

    public EmailContentS3BucketConfig(AmazonS3Client s3Client, string bucketName)
    {
        S3Client = s3Client;
        BucketName = bucketName;
    }
}