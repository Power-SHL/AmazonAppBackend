using AmazonAppBackend.Configuration.Settings;
using AmazonAppBackend.DTO.Social;
using AmazonAppBackend.Exceptions.FeedExceptions;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;

namespace AmazonAppBackend.Services.FeedService;

public class SpotifyApiWebService : ISpotifyService
{
    private readonly SpotifySettings _spotifySettings;
    public SpotifyApiWebService(SpotifySettings spotifySettings)
    {
        _spotifySettings = spotifySettings;
    }
    public async Task<string> GetToken()
    {
        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(_spotifySettings.ClientID, _spotifySettings.ClientSecret);
        var response = await new OAuthClient(config).RequestToken(request);
        return response.AccessToken;
    }
    public async Task<Song> GetSong(string songId, string token)
    {
        var client = new SpotifyClient(token);
        var track = await client.Tracks.Get(songId);

        var artists = track.Artists.Select(artist => artist.Name).ToList();
        return new Song(track.Id, track.Name, track.Album.Images[0].Url, artists);
    }

    public async Task<List<Song>> GetSongsFromAlbum(string albumId, string token)
    {
        var client = new SpotifyClient(token);
        var album = await client.Albums.Get(albumId);

        var songIds = album.Tracks.Items.Select(track => track.Id).ToList();
        return await GetSongs(songIds, token);
    }

    public async Task<List<Song>> GetSongsFromPlaylist(string playlistId, string token)
    {
        var client = new SpotifyClient(token);
        var playlist = await client.Playlists.Get(playlistId);

        var songIds = playlist.Tracks.Items
            .Where(item => item.Track is FullTrack)
            .Select(item => ((FullTrack)item.Track).Id)
            .ToList();
        return await GetSongs(songIds, token);
    }

    private async Task<List<Song>> GetSongs(List<string> songIds, string token)
    {
        List<Song> songs = new();
        foreach (var songId in songIds)
        {
            songs.Add(await GetSong(songId, token));
        }
        return songs;
    }
}
