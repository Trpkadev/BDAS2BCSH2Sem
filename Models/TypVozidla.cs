using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("TYPY_VOZIDEL")]
public class TypVozidla
{
    [Key]
    [JsonRequired]
    [Column("ID_TYP_VOZIDLA")]
    public int IdTypVozidla { get; set; }

    [JsonRequired]
    [Column("NAZEV")]
    public string Nazev { get; set; } = string.Empty;

    public override string ToString() => Nazev;
}