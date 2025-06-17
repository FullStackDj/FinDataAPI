namespace FinDataAPI.DTOs.Account;

public class NewUserDTO
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}