using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotenTool.API.Semester;
using NotenTool.API.Subject;

namespace NotenTool.API.Grade;

[Table("Grade")]
public class DbGrade
{
    public DbGrade()
    {
        
    }

    public DbGrade(Guid id, double grade, string comment, DateTime date, double weight, Guid subjectId, Guid semesterId)
    {
        Id = id;
        Grade = grade;
        Comment = comment;
        Date = date;
        Weight = weight;
        Subject_id = subjectId;
        Semester_id = semesterId;
    }

    [Key] public Guid Id { get; set; }

    [Required] public double Grade { get; set; }
    [Required] public string Comment { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public double Weight { get; set; }

    [Display(Name = "Subject")] [Required] public Guid Subject_id { get; set; }

    [ForeignKey("Subject_id")] public virtual DbSubject? Subjects { get; set; }

    [Display(Name = "Semester")]
    [Required]
    public Guid Semester_id { get; set; }

    [ForeignKey("Semester_id")] public virtual DbSemester? Semester { get; set; }
}