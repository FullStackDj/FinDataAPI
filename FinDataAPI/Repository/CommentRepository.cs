using Microsoft.EntityFrameworkCore;
using FinDataAPI.Interfaces;
using FinDataAPI.Models;
using FinDataAPI.Data;

namespace FinDataAPI.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDBContext _context;
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }
}