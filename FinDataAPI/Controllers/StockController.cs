using Microsoft.AspNetCore.Mvc;
using FinDataAPI.Data;
using FinDataAPI.Mappers;
using FinDataAPI.DTOs.Stock;

namespace FinDataAPI.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var stocks = _context.Stocks
            .Select(s => s.ToStockDTO())
            .ToList();
        
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var stock = _context.Stocks.Find(id);

        if (stock == null)
        {
            return NotFound();
        }
        
        return Ok(stock.ToStockDTO());
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequestDTO stockDTO)
    {
        var stockModel = stockDTO.ToStockFromCreateDTO();
        _context.Stocks.Add(stockModel);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id},  stockModel.ToStockDTO());
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
    {
        var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        stockModel.Symbol = updateDTO.Symbol;
        stockModel.CompanyName = updateDTO.CompanyName;
        stockModel.Purchase = updateDTO.Purchase;
        stockModel.LastDiv = updateDTO.LastDiv;
        stockModel.Industry = updateDTO.Industry;
        stockModel.MarketCap = updateDTO.MarketCap;

        _context.SaveChanges();

        return Ok(stockModel.ToStockDTO());
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }
        
        _context.Stocks.Remove(stockModel);

        _context.SaveChanges();

        return NoContent();
    }
}