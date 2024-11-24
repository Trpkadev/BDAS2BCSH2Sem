using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("GARAZE")]
public class Garaz
{
    [Key]
    [JsonRequired]
    [Column("ID_GARAZ")]
    public int IdGaraz { get; set; }

    [JsonRequired]
    [Column("NAZEV")]
    [DisplayName("Název")]
    public string Nazev { get; set; } = string.Empty;

    [JsonRequired]
    [Column("KAPACITA")]
    public int Kapacita { get; set; }

    public override string ToString() => Nazev;
}