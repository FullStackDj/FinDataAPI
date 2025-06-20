using FinDataAPI.Models;

namespace FinDataAPI.Interfaces;

public interface IPortfolioRepository
{
    Task<List<Stock>> GetUserPortfolio(AppUser user);
}