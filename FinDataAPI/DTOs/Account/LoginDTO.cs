using System.ComponentModel.DataAnnotations;

namespace FinDataAPI.DTOs.Account;

public class LoginDTO
{
    [Required] public required string Username { get; set; }
    [Required] public required string Password { get; set; }
}