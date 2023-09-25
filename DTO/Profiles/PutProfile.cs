using System.ComponentModel.DataAnnotations;

namespace AmazonAppBackend.DTO.Profiles;

public record PutProfile
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public PutProfile([Required] string firstname, [Required] string lastname)
    {
        FirstName = firstname.ToLower();
        LastName = lastname.ToLower();
    }
}