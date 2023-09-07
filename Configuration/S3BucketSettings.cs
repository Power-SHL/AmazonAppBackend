﻿namespace AmazonAppBackend.Configuration;

public class S3BucketSettings
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string RegionEndpoint { get; set; }
    public string BucketName { get; set; }
}