using FinDataAPI.Models;

namespace FinDataAPI.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}