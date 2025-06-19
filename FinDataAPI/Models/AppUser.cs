using Microsoft.AspNetCore.Identity;

namespace FinDataAPI.Models;

public class AppUser : IdentityUser
{
    public List<Portfolio> Portfolios { get; set; } = new();
}