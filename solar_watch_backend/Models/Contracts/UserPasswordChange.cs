namespace solar_watch_backend.Models.Contracts;

public record UserPasswordChange(string ExistingPassword, string NewPassword);