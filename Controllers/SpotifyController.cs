using AmazonAppBackend.Configuration.Settings;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using AmazonAppBackend.DTO.Social;
using AmazonAppBackend.Services.FeedService;
using AmazonAppBackend.Exceptions.FeedExceptions;

namespace AmazonAppBackend.Controllers;


[ApiController]
[Route("api/spotify")]
public class SpotifyController : ControllerBase
{
    private readonly ISpotifyService _spotifyService;
    public SpotifyController(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    [HttpGet("token")]
    public async Task<ActionResult> GetSpotifyToken()
    {
        try
        {
            return Ok(await _spotifyService.GetToken());
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to get album tracks. Error: {ex.Message}");
        }
    }

    [HttpGet("song")]
    public async Task<ActionResult<Song>> GetTrack(string songId, string token)
    {
        try
        {
            return Ok(await _spotifyService.GetSong(songId, token));
        }
        catch (Exception e)
        {
            if (e is APIUnauthorizedException)
            {
                return Unauthorized($"Expired token");
            }
            else
            {
                return NotFound($"Failed to get song. Error: {e.Message}");
            }
        }
    }

    [HttpGet("album")]
    public async Task<ActionResult<List<Song>>> GetAlbum(string albumId, string token)
    {
        try
        {
            return Ok(await _spotifyService.GetSongsFromAlbum(albumId, token));
        }
        catch (Exception ex)
        {
            if (ex is APIUnauthorizedException)
            {
                return Unauthorized($"Expired token");
            }
            else
            {
                return NotFound($"Failed to get album tracks. Error: {ex.Message}");
            }
        }
    }

    [HttpGet("playlist")]
    public async Task<ActionResult<List<Song>>> GetPlaylist(string playlistId, string token)
    {
        try
        {
            return Ok(await _spotifyService.GetSongsFromPlaylist(playlistId, token));
        }
        catch (Exception e)
        {
            if (e is APIUnauthorizedException)
            {
                return Unauthorized($"Expired token");
            }
            else
            {
                return NotFound($"Failed to get playlist tracks. Error: {e.Message}");
            }
        }
    }
}
