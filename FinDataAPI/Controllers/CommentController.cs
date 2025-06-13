using Microsoft.AspNetCore.Mvc;
using FinDataAPI.Interfaces;
using FinDataAPI.Mappers;
using FinDataAPI.DTOs.Comment;

namespace FinDataAPI.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;

    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepo.GetAllAsync();

        var commentDTO = comments.Select(s => s.ToCommentDTO());

        return Ok(commentDTO);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await _commentRepo.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentDTO());
    }

    [HttpPost("{stockId}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDTO commentDTO)
    {
        if (!await _stockRepo.StockExists(stockId))
        {
            return BadRequest("Stock don't exist");
        }

        var commentModel = commentDTO.ToCommentFromCreate(stockId);
        await _commentRepo.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDTO());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDTO updateDTO)
    {
        var comment = await _commentRepo.UpdateAsync(id, updateDTO.ToCommentFromUpdate());

        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        return Ok(comment.ToCommentDTO());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var commentModel = await _commentRepo.DeleteAsync(id);

        if (commentModel == null)
        {
            return NotFound("Comment don't exist");
        }
        
        return Ok(commentModel);
    }
}