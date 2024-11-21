using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("TARIFNI_PASMA")]
public class TarifniPasmo
{
    [Key]
    [JsonRequired]
    [Column("ID_PASMO")]
    public int IdPasmo { get; set; }

    [JsonRequired]
    [Column("NAZEV")]
    public string Nazev { get; set; } = string.Empty;

    public override string ToString() => Nazev;
}