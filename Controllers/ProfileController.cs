using Microsoft.AspNetCore.Mvc;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Services.EmailService;
using AmazonAppBackend.Services.ProfileService;
using AmazonAppBackend.Exceptions.ResetPasswordExceptions;
using AmazonAppBackend.Services.AuthorizationService;
using AmazonAppBackend.Exceptions.AuthenticationExceptions;
using AmazonAppBackend.DTO.Profiles;

namespace AmazonAppBackend.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IEmailService _emailService;
    private readonly IAuthorizationService _authorizationService;

    public ProfileController(IProfileService profileService, IEmailService emailService, IAuthorizationService authorizationService)
    {
        _profileService = profileService;
        _emailService = emailService;
        _authorizationService = authorizationService;
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
            UnverifiedProfile unverifiedProfile = new (profile, SecurityExtension.GenerateRandomCode());
            await _profileService.CreateProfile(unverifiedProfile);
            await _emailService.VerifyEmail(unverifiedProfile);

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

    [HttpPost("login")]
    public async Task<ActionResult<Profile>> Login(SignInRequest request)
    {
        if (!request.Password.IsValidPassword())
        {
            return BadRequest("Password format is invalid");
        }
        try
        {
            var token = await _authorizationService.AuthorizeUser(request);
            return Ok(token);
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException || e is IncorrectPasswordException)
            {
                return Unauthorized($"Email or password is incorrect. Try again or sign up.");
            }
            if (e is ProfileInvalidException)
            {
                return BadRequest("Invalid username or email");
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

    [HttpGet("verify/{username}")]
    public async Task<ActionResult<Profile>> ResendVerificationEmail(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }

        try
        {
            var profile = await _profileService.GetUnverifiedProfile(username);
            await _emailService.VerifyEmail(profile);

            return Ok($"Email successfully sent to {username}.");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"User {username} not found.");
            }
            throw;
        }
    }

    [HttpPut("{username}")]
    public async Task<ActionResult> UpdateProfile(string username, PutProfile partProfile)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
        try
        {
            _authorizationService.AuthorizeRequest(User, username);
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
            if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
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
            _authorizationService.AuthorizeRequest(User, username);
            await _profileService.DeleteProfile(username);
            return Ok($"User with username {username} deleted");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"User with username {username} not found");
            }
            if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }
            throw;
        }
    }

    [HttpPost("reset/{username}")]
    public async Task<ActionResult> ResetPasswordRequest(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
        try
        {
            var resetRequest = new ResetPasswordRequest(username, SecurityExtension.GenerateRandomCode());
            await _profileService.AddResetPasswordRequest(resetRequest);
            await _emailService.ResetPasswordEmail(resetRequest);
            return Ok($"Password reset request sent to {resetRequest.Username}");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"User with username {username} not found");
            }

            if (e is ResetPasswordRequestDuplicateException)
            {
                return Conflict($"Password reset request already sent to {username}");
            }
            throw;
        }
    }

    [HttpGet("reset/{username}")]
    public async Task<ActionResult<Profile>> ResendResetEmail(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }

        try
        {
            var request = await _profileService.GetResetPasswordRequest(username);
            await _emailService.ResetPasswordEmail(request);
            return Ok($"Email successfully sent to {username}.");
        }
        catch (Exception e)
        {
            if (e is ResetPasswordRequestNotFoundException)
            {
                return NotFound($"Password reset request for {username} not found.");
            }
            throw;
        }
    }

    [HttpPut("reset")]
    public async Task<ActionResult> ResetPassword(ChangedPasswordRequest resetRequest)
    {
        try
        {
            resetRequest.ValidateChangedPasswordRequest();
            resetRequest.Password = resetRequest.Password.BCryptHash();
            await _profileService.ResetPassword(resetRequest);
            return Ok($"Password reset for {resetRequest.Username}");
        }
        catch (Exception e)
        {

            if (e is ResetPasswordRequestNotFoundException)
            {
                return NotFound($"Password reset request for {resetRequest.Username} not found");
            }

            if (e is ResetPasswordRequestInvalidException)
            {
                return BadRequest(e.Message);
            }
            throw;
        }
    }   
}