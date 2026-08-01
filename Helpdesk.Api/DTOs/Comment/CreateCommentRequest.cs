using System.ComponentModel.DataAnnotations;

namespace Helpdesk.Dtos.Comment;

public class CreateCommentRequest
{
    [Required(ErrorMessage = "Content is required.")]
    [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
    public string Content { get; set; } = "";
}