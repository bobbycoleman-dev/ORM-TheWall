#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models;
public class LoginUser
{
    [Required]
    [DisplayName("Email")]
    public string LogEmail { get; set; }
    [Required]
    [DisplayName("Password")]
    [DataType(DataType.Password)]
    public string LogPassword { get; set; }
}