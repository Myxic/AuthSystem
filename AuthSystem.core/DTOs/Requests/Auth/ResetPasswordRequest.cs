using System.ComponentModel.DataAnnotations;

namespace AuthSystem.core.DTOs.Requests.Auth;


public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}