using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("UDRZBY")]
public class Udrzba
{
    [Key]
    [JsonRequired]
    [Column("ID_UDRZBA")]
    public int IdUdrzba { get; set; }

    [JsonRequired]
    [Column("DATUM")]
    public DateTime Datum { get; set; }

    [JsonRequired]
    [Column("ID_VOZIDLO")]
    public int IdVozidlo { get; set; }

    [Column("POPIS_UKONU")]
    public string? PopisUkonu { get; set; }

    [Column("CENA")]
    public double? Cena { get; set; }

    [Column("UMYTO_V_MYCCE")]
    public bool? UmytoVMycce { get; set; }

    [Column("CISTENO_OZONEM")]
    public bool? CistenoOzonem { get; set; }

    [JsonRequired]
    [Column("TYP_UDRZBY")]
    public char TypUdrzby { get; set; }

    [Column("NAZEV_VOZIDLA")]
    [DisplayName("Vozidlo")]
    public string? NazevVozidla { get; set; }
}