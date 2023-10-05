#pragma warning disable CS8618
#pragma warning disable CS8600
#pragma warning disable CS8602
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models;

public class Comment
{
    [Key]
    public int CommentId { get; set; }

    [Required]
    [MaxLength(45)]
    [DisplayName("Comment")]
    public string CommentText { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMMM d, yyyy}")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //! One-to-Many -> A Comment can only have one Commenter
    public int UserId { get; set; }
    public User? Commenter { get; set; }

    //! One-to-Many -> A Comment can only be on one Message
    public int MessageId { get; set; }
    public Message? CommentedMessage { get; set; }

}
