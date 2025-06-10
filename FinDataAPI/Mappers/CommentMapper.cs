using FinDataAPI.DTOs.Comment;
using FinDataAPI.Models;

namespace FinDataAPI.Mappers;

public static class CommentMapper
{
    public static CommentDTO ToCommentDTO(this Comment commentModel)
    {
        return new CommentDTO
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn.Date,
            StockId = commentModel.StockId,
        };
    }
}