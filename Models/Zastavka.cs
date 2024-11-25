using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("ZASTAVKA")]
public class Zastavka
{
    [Key]
    [JsonRequired]
    [Column("ID_ZASTAVKA")]
    public int IdZastavka { get; set; }

    [JsonRequired]
    [Column("NAZEV")]
    [DisplayName("Název")]
    public string Nazev { get; set; } = string.Empty;

    [JsonRequired]
    [Column("SOURADNICE_X")]
    [DisplayName("Souřadnice X")]
    public double SouradniceX { get; set; }

    [JsonRequired]
    [Column("SOURADNICE_Y")]
    [DisplayName("Souřadnice Y")]
    public double SouradniceY { get; set; }

    [JsonRequired]
    [Column("ID_PASMO")]
    public int IdPasmo { get; set; }

    [DisplayName("Pásmo")]
    public string? PasmoNazev { get; set; }

    public override string ToString() => Nazev;
}