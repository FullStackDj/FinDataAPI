using Microsoft.EntityFrameworkCore; 
using FinDataAPI.Interfaces;
using FinDataAPI.Models;
using FinDataAPI.Data;

namespace FinDataAPI.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context;
    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stocks.ToListAsync();
    }
}