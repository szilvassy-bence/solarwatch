using System.ComponentModel.DataAnnotations;

namespace solar_watch_backend.Models.Contracts;

public partial record UserDataChange
{
    [Required]
    [MinLength(3, ErrorMessage = "Email must be at least 3 characters long.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; init; }
    
    [Required]
    [MinLength(3, ErrorMessage = "UserName must be at least 3 characters long.")]
    public string UserName { get; init; }

    public UserDataChange(string email, string userName)
    {
        Email = email;
        UserName = userName;
    }
}