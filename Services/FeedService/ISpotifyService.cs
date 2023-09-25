using AmazonAppBackend.DTO.Social;
namespace AmazonAppBackend.Services.FeedService;

public interface ISpotifyService
{
    Task<string> GetToken();
    Task<Song> GetSong(string songId, string token);
    Task<List<Song>> GetSongsFromAlbum(string albumId, string token);
    Task<List<Song>> GetSongsFromPlaylist(string playlistId, string token);
}