using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotenTool.API.Grade;
using NotenTool.API.UserInformation;

namespace NotenTool.API.AllGrades;

[Table("AllGrades")]
public class DbAllGrades
{
    [Key]
    public Guid Id { get; set; }

    [Display(Name = "UserInfo")] public Guid UserInfo_id { get; set; }
    [ForeignKey("UserInfo_id")] public virtual DbUserInformation? UserInformations { get; set; }

    [Display(Name = "Grade")] public Guid Grade_id { get; set; }
    [ForeignKey("Grade_id")] public virtual DbGrade? Grades { get; set; }
}