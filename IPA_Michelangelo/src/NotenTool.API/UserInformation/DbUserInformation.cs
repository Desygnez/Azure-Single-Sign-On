using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotenTool.API.UserInformation;

[Table("UserInformation")]
public class DbUserInformation
{
    public DbUserInformation(Guid id, string firstname, string lastname, string username, string email)
    {
        Id = id;
        Firstname = firstname;
        Lastname = lastname;
        Username = username;
        Email = email;

    }

    public DbUserInformation()
    {
        
    }
    
    [Key]
    public Guid Id { get; set; }

    [Required] public string Firstname { get; set; }
    [Required] public string Lastname { get; set; }
    [Required] public string Username { get; set; }
    [Required] public string Email { get; set; }
    
}