using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("TARIFNI_PASMA")]
public class TariffZone
{
    [Key]
    [Column("ID_PASMO")]
    public int IdPasmo { get; set; }
    [Column("NAZEV")]
    public required string Nazev { get; set; }
}
