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
            CreatedOn = commentModel.CreatedOn,
            CreatedBy = commentModel.AppUser.UserName,
            StockId = commentModel.StockId,
        };
    }

    public static Comment ToCommentFromCreate(this CreateCommentDTO commentDTO, int stockId)
    {
        return new Comment
        {
            Title = commentDTO.Title,
            Content = commentDTO.Content,
            StockId = stockId,
        };
    }

    public static Comment ToCommentFromUpdate(this UpdateCommentRequestDTO commentDTO)
    {
        return new Comment
        {
            Title = commentDTO.Title,
            Content = commentDTO.Content,
        };
    }
}