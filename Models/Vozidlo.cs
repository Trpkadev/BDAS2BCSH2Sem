using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("VOZIDLA")]
public class Vozidlo
{
    [Key]
    [JsonRequired]
    [Column("ID_VOZIDLO")]
    public int IdVozidlo { get; set; }

    [JsonRequired]
    [Column("ROK_VYROBY")]
    [DisplayName("Rok výroby")]
    public short RokVyroby { get; set; }

    [JsonRequired]
    [Column("NAJETE_KILOMETRY")]
    [DisplayName("Najeté kilometry")]
    public int NajeteKilometry { get; set; }

    [JsonRequired]
    [Column("KAPACITA")]
    public int Kapacita { get; set; }

    [JsonRequired]
    [Column("MA_KLIMATIZACI")]
    [DisplayName("Má klimatizaci")]
    public bool MaKlimatizaci { get; set; }

    [JsonRequired]
    [Column("ID_GARAZ")]
    public int IdGaraz { get; set; }

    [JsonRequired]
    [Column("ID_MODEL")]
    public int IdModel { get; set; }

    [Column("NAZEV_GARAZE")]
    [DisplayName("Garáž")]
    public string? NazevGaraze { get; set; }

    [Column("NAZEV_VOZIDLA")]
    [DisplayName("Vozidlo")]
    public string? NazevVozidla { get; set; }
}