using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotenTool.API.School;
using NotenTool.API.Semester;
using NotenTool.API.Subject;
using NotenTool.API.UserInformation;

namespace NotenTool.API.SubjectUser;

[Table("Subject_has_User")]
public class DbSubjectUser
{
    public DbSubjectUser(Guid id, Guid subjectId, Guid userInfoId, Guid schoolId, Guid semesterId)
    {
        Id = id;
        Subject_id = subjectId;
        UserInfo_id = userInfoId;
        School_id = schoolId;
        Semester_id = semesterId;
    }

    public DbSubjectUser()
    {
    }

    [Key] public Guid Id { get; set; }

    [Display(Name = "Subject")] public Guid Subject_id { get; set; }
    [ForeignKey("Subject_id")] public virtual DbSubject? Subject { get; set; }

    [Display(Name = "UserInfo")] public Guid UserInfo_id { get; set; }
    [ForeignKey("UserInfo_id")] public virtual DbUserInformation? UserInformation { get; set; }

    [Display(Name = "School")] public Guid School_id { get; set; }
    [ForeignKey("School_id")] public virtual DbSchool? School { get; set; }

    [Display(Name = "Semester")] public Guid Semester_id { get; set; }
    [ForeignKey("Semester_id")] public virtual DbSemester? Semester { get; set; }
}