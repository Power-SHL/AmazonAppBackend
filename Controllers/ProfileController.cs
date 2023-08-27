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

    [HttpGet("{username}")]
    public async Task<ActionResult<Profile>> GetProfile(string username)
    {
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
            await _profileService.CreateProfile(profile);

            return CreatedAtAction(nameof(GetProfile), new { username = profile.Username }, profile);
        }
        catch (Exception e)
        {
            if (e is ProfileAlreadyExistsException)
            {
                return Conflict("Cannot create profile.\n" + e.Message);
            }
            else if (e is ProfileInvalidException)
            {
                return BadRequest(e.Message);
            }
            throw;
        }
    }

    [HttpPut("{username}")]
    public async Task<ActionResult> UpdateProfile(string username, PutProfile partProfile)
    {
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
