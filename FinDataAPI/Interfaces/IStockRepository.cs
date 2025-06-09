using FinDataAPI.Models;
using FinDataAPI.DTOs.Stock;

namespace FinDataAPI.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock> CreateAsync(Stock stockModel);
    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDTO);
    Task<Stock?> DeleteAsync(int id);
}