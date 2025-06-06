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
}