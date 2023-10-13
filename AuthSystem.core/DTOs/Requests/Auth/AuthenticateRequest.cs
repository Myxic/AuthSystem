using System.ComponentModel.DataAnnotations;


namespace AuthSystem.core.DTOs.Requests.Auth;


public class AuthenticateRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}