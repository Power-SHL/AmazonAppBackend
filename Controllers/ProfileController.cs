using Microsoft.AspNetCore.Mvc;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.DTO;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Services;

namespace AmazonAppBackend.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    private async Task CheckUniqueEmail(string email)
    {
        try
        {
            await _profileService.GetProfileByEmail(email);
            throw new ProfileConflictException($"Email {email} is taken.");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return;
            }
            throw;
        }
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<Profile>> GetProfile(string username)
    {
        try
        {
            var profile = await _profileService.GetProfile(username);
            return Ok(profile);
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Profile with username {username} not found.");
            }
            throw;
        }
    }

    [HttpPost]
    public async Task<ActionResult<Profile>> AddProfile(Profile profile)
    {
        if (!profile.Email.IsValidEmail())
        {
            return BadRequest("Email format is invalid");
        }
        try
        {
            await CheckUniqueEmail(profile.Email);
            profile.Password = profile.Password.BCryptHash();
            await _profileService.CreateProfile(profile);

            return CreatedAtAction(nameof(GetProfile), new { username = profile.Username }, profile);
        }
        catch (Exception e)
        {
            if (e is ProfileConflictException)
            {
                return Conflict("Cannot create profile.\n" + e.Message);
            }
            throw;
        }
    }
}
