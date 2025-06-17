using FinDataAPI.Models;

namespace FinDataAPI.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}