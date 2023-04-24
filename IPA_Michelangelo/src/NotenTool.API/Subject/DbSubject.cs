using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotenTool.API.Subject;

[Table("Subject")]
public record DbSubject(Guid Id, [Required] string Subject);