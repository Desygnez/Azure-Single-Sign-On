using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NotenTool.API.Semester;

public record DbSemester(Guid Id, [Required] int Semester);