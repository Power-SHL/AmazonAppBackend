using Amazon.S3;
namespace AmazonAppBackend.Configuration;

public class ImageS3BucketConfig
{
    public AmazonS3Client S3Client { get; set; }
    public string BucketName { get; set; }

    public ImageS3BucketConfig(AmazonS3Client s3Client, string bucketName)
    {
        S3Client = s3Client;
        BucketName = bucketName;
    }
}