using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotenTool.API.School;
using NotenTool.API.Semester;
using NotenTool.API.UserInformation;

namespace NotenTool.API.UserSemester;

public class DbUserSemester
{
    public DbUserSemester(Guid id, Guid userId, Guid semesterId)
    {
        Id = id;
        UserInfo_id = userId;
        Semester_id = semesterId;
    }

    public DbUserSemester()
    {
        
    }

    [Key] public Guid Id { get; set; }
    [Display(Name = "UserInformation")] public Guid UserInfo_id { get; set; }
    [ForeignKey("UserInfo_id")] public virtual DbUserInformation? UserInformation { get; set; }
    [Display(Name = "Semester")] public Guid Semester_id { get; set; }
    [Display(Name = "Semester_id")] public virtual DbSemester? Semester { get; set; }
}