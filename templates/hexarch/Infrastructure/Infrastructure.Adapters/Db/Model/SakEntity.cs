using System.ComponentModel.DataAnnotations.Schema;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;

[Table("Saker")]
internal class SakEntity : BaseEntity
{
    public required Guid Id { get; set; }

    [Column(TypeName = "varchar(9)")]
    public required string Organisajonsnummer { get; set; }

    [Column(TypeName = "varchar(24)")]
    public required SakStatus Status { get; set; }
}
