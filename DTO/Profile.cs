using System.ComponentModel.DataAnnotations;
namespace AmazonAppBackend.DTO;

public record Profile
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string? Post;
    public List<string> Following;
    public List<string> Followers;

    public Profile([Required] string Username, [Required] string Email, [Required] string Password,
        [Required] string FirstName, [Required] string LastName, string? Post = null)
    {
        this.Username = Username;
        this.Email = Email;
        this.Password = Password;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Post = Post;
        this.Following = new List<string>();
        this.Followers = new List<string>();
    }

}