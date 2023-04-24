using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotenTool.API.Grade;
using NotenTool.API.UserInformation;

namespace NotenTool.API.School;

public record DbSchool(Guid Id, [Required] string School);
