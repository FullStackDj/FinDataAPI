using FinDataAPI.Models;

namespace FinDataAPI.Interfaces;

public interface IFMPService
{
    Task<Stock> FindStockBySymbolAsync(string symbol);
}