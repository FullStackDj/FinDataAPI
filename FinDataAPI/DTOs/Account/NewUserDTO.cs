namespace FinDataAPI.DTOs.Account;

public class NewUserDTO
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}