using FinDataAPI.Models;

namespace FinDataAPI.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
}