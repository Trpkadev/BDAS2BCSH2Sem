using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("MODELY")]
public class Model
{
    [Key]
    [JsonRequired]
    [Column("ID_MODEL")]
    public int IdModel { get; set; }

    [JsonRequired]
    [Column("NAZEV")]
    [DisplayName("Název")]
    public string Nazev { get; set; } = string.Empty;

    [JsonRequired]
    [Column("JE_NIZKOPODLAZNI")]
    [DisplayName("Je nízkopodlažní")]
    public bool JeNizkopodlazni { get; set; }

    [JsonRequired]
    [Column("ID_ZNACKA")]
    public int IdZnacka { get; set; }

    [JsonRequired]
    [Column("ID_TYP_VOZIDLA")]
    public int IdTypVozidla { get; set; }

    [DisplayName("Značka")]
    public string ZnackaNazev { get; set; } = string.Empty;

    [DisplayName("Typ vozidla")]
    public string TypVozidlaNazev { get; set; } = string.Empty;

    public override string ToString() => Nazev;
}