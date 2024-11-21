using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("LINKY")]
public class Linka
{
    [Key]
    [JsonRequired]
    [Column("ID_LINKA")]
    public int IdLinka { get; set; }

    [JsonRequired]
    [Column("CISLO")]
    public int Cislo { get; set; }

    [JsonRequired]
    [Column("NAZEV")]
    public string Nazev { get; set; } = string.Empty;

    [JsonRequired]
    [Column("ID_TYP_VOZIDLA")]
    public int IdTypVozidla { get; set; }

    public string TypVozidlaNazev { get; set; }

    public override string ToString() => Nazev;
}