using System.Diagnostics;
using AmazonAppBackend.Exceptions.ImageExceptions;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Services.AuthorizationService;
using AmazonAppBackend.Services.ImageService;
using Microsoft.AspNetCore.Mvc;

namespace AmazonAppBackend.Controllers;

[ApiController]
[Route("api/images/{username}")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly IAuthorizationService _authorizationService;

    public ImageController(IImageService imageService, IAuthorizationService authorizationService)
    {
        _imageService = imageService;
        _authorizationService = authorizationService;
    }

    [HttpPost]
    public async Task<ActionResult> UploadImage(IFormFile image, string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
        var type = image.ContentType;
        if (type != "image/png" && type != "image/jpeg")
        {
            return BadRequest($"{type} file not supported. Upload a PNG or JPEG image instead.");
        }
        if (image.Length > 5e6)
        {
            return BadRequest("Image size is too large. Upload an image that is less than 5MB.");
        }

        try
        {
            _authorizationService.AuthorizeRequest(User, username);
            await _imageService.UploadImage(image, username);
            return Created($"api/images/{username}", $"Profile picture for {username} successfully posted.");
        }
        catch (Exception ex)
        {
            if (ex is ProfileNotFoundException)
            {
                return NotFound($"User with username {username} not found.");
            }
            if (ex is UnauthorizedAccessException)
            {
                return Unauthorized(ex.Message);
            }
            throw;
        }
    }

    [HttpGet]
    public async Task<ActionResult> DownloadImage(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
        try
        {
            var imageBytes = await _imageService.DownloadImage(username);
            return new FileContentResult(imageBytes, "image/jpeg");
        }
        catch (Exception e)
        {
            if (e is ImageNotFoundException)
            {
                return NotFound($"Profile picture for {username} not found.");
            }
            throw;
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteImage(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
        try
        {
            _authorizationService.AuthorizeRequest(User, username);
            await _imageService.DeleteImage(username);
            return Ok($"Profile picture for {username} successfully deleted.");
        }
        catch (Exception e)
        {
            if (e is ImageNotFoundException)
            {
                return NotFound($"Profile picture for {username} not found.");
            }
            if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }
            throw;
        }
    }
}