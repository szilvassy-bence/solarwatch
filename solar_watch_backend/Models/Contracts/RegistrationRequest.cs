using System.ComponentModel.DataAnnotations;

namespace solar_watch_backend.Models.Contracts;

public record RegistrationRequest([Required] string Email, [Required] string UserName, [Required] string Password);
