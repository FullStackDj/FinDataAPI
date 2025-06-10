using FinDataAPI.Models;

namespace FinDataAPI.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
	Task<Comment?> GetByIdAsync(int id);
}