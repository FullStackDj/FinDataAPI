using System.ComponentModel.DataAnnotations;

namespace FinDataAPI.DTOs.Comment;

public class UpdateCommentRequestDTO
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be over 4 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MinLength(5, ErrorMessage = "Content must be over 4 characters")]
    [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]
    public string Content { get; set; } = string.Empty;
}