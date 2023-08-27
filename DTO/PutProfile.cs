using System.ComponentModel.DataAnnotations;
namespace AmazonAppBackend.DTO;

public record PutProfile([Required] string FirstName, [Required] string LastName);