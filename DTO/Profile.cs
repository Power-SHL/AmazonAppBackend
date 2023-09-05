using System.ComponentModel.DataAnnotations;
namespace AmazonAppBackend.DTO;

public record Profile
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Profile([Required] string username, [Required] string email, [Required] string password,
        [Required] string firstName, [Required] string lastName)
    {
        Username = username.ToLower();
        Email = email.ToLower();
        Password = password;
        FirstName = firstName;
        LastName = lastName;
    }

    public void SetTo(PutProfile putProfile)
    {
        FirstName = putProfile.FirstName.ToLower();
        LastName = putProfile.LastName.ToLower();
    }
}