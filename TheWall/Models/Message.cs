#pragma warning disable CS8618
#pragma warning disable CS8600
#pragma warning disable CS8602
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models;

public class Message
{
    [Key]
    public int MessageId { get; set; }

    [Required]
    [MaxLength(45)]
    [DisplayName("Message")]
    public string MessageText { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMMM d, yyyy}")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //! One-to-Many -> A Message can only have one Creator
    public int UserId { get; set; }
    public User? Creator { get; set; }

    //! Many-to-Many -> A Message can have many Comments
    public List<Comment> MessageComments { get; set; } = new();
}
