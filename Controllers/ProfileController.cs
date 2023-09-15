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
    private readonly IEmailService _emailService;

    public ProfileController(IProfileService profileService, IEmailService emailService)
    {
        _profileService = profileService;
        _emailService = emailService;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<Profile>> GetProfile(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
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
    public async Task<ActionResult<Profile>> CreateProfile(Profile profile)
    {
        try
        {
            profile.ValidateProfile();
            profile.Password = profile.Password.BCryptHash();
            UnverifiedProfile unverifiedProfile = new (profile, Guid.NewGuid().ToString("N"));
            await _profileService.CreateProfile(unverifiedProfile);
            await _emailService.SendEmail(unverifiedProfile);

            return CreatedAtAction(nameof(GetProfile), new { username = profile.Username }, profile);
        }
        catch (Exception e)
        {
            if (e is ProfileDuplicateException)
            {
                return Conflict("Cannot create profile.\n" + e.Message);
            }
            if (e is ProfileInvalidException)
            {
                return BadRequest(e.Message);
            }
            throw;
        }
    }

    [HttpPost("verify")]
    public async Task<ActionResult<Profile>> VerifyProfile(string username, string verificationCode)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }

        try
        {
            var profile = await _profileService.VerifyProfile(username, verificationCode);

            return CreatedAtAction(nameof(GetProfile), new { username = profile.Username }, profile);
        }
        catch (Exception e)
        {
            if (e is ProfileDuplicateException)
            {
                return Conflict("Cannot create profile.\n" + e.Message);
            }

            if (e is ProfileInvalidException)
            {
                return BadRequest(e.Message);
            }

            if (e is ProfileVerificationException)
            {
                return BadRequest("Incorrect verification code.");
            }

            throw;
        }
    }

    [HttpPut("{username}")]
    public async Task<ActionResult> UpdateProfile(string username, PutProfile partProfile)
    {
        username = username.ToLower();
        try
        {
            partProfile.ValidatePutProfile();
            var profile = await _profileService.GetProfile(username);
            profile.SetTo(partProfile);

            await _profileService.UpdateProfile(profile);
            return Ok($"User with username {username} updated");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"User with username {username} not found");
            }
            if (e is ProfileInvalidException)
            {
                return BadRequest(e.Message);
            }
            throw;
        }
    }

    [HttpDelete("{username}")]
    public async Task<ActionResult> DeleteProfile(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
        try
        {
            await _profileService.DeleteProfile(username);
            return Ok($"User with username {username} deleted");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"User with username {username} not found");
            }
            throw;
        }
    }
}