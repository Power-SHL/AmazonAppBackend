﻿using Amazon.S3;
using Amazon.S3.Model;
using AmazonAppBackend.Exceptions.ImageExceptions;
using System.Net;
using AmazonAppBackend.Configuration.Clients;

namespace AmazonAppBackend.Storage.ImageStore;

public class ImageS3BucketStore : IImageStore
{
    private readonly string _bucketName;
    private readonly AmazonS3Client _s3Client;

    public ImageS3BucketStore(ImageS3BucketConfig config)
    {
        _s3Client = config.S3Client;
        _bucketName = config.BucketName;
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