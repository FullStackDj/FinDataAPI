﻿using Microsoft.EntityFrameworkCore;
using FinDataAPI.Data;
using FinDataAPI.Interfaces;
using FinDataAPI.Models;

namespace FinDataAPI.Repository;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly ApplicationDBContext _context;

    public PortfolioRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<Stock>> GetUserPortfolio(AppUser user)
    {
        return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock!.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap,
            }).ToListAsync();
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        await _context.Portfolios.AddAsync(portfolio);
        await _context.SaveChangesAsync();
        return portfolio;
    }

    public async Task<Portfolio> DeletePortfolio(AppUser appuser, string symbol)
    {
        var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x =>
            x.AppUserId == appuser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

        if (portfolioModel == null)
        {
            return null;
        }

        _context.Portfolios.Remove(portfolioModel);
        await _context.SaveChangesAsync();
        return portfolioModel;
    }
}