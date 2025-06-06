using FinDataAPI.DTOs.Stock;
using FinDataAPI.Models;

namespace FinDataAPI.Mappers;

public static class StockMappers
{
    public static StockDTO ToStockDTO(this Stock stockModel)
    {
        return new StockDTO
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap
        };
    }

    public static Stock ToStockFromCreateDTO(this CreateStockRequestDTO stockDTO)
    {
        return new Stock
        {
            Symbol = stockDTO.Symbol,
            CompanyName = stockDTO.CompanyName,
            Purchase = stockDTO.Purchase,
            LastDiv = stockDTO.LastDiv,
            Industry = stockDTO.Industry,
            MarketCap = stockDTO.MarketCap
        };
    }
}