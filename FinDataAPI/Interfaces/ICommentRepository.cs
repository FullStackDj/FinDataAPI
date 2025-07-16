using FinDataAPI.Helpers;
using FinDataAPI.Models;

namespace FinDataAPI.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject);
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> CreateAsync(Comment commentModel);
    Task<Comment?> UpdateAsync(int id, Comment commentModel);
    Task<Comment?> DeleteAsync(int id);
}