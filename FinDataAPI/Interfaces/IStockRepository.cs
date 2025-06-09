using FinDataAPI.Models;

namespace FinDataAPI.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();
}