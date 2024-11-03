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
    public short RokVyroby { get; set; }

    [JsonRequired]
    [Column("NAJETE_KILOMETRY")]
    public int NajeteKilometry { get; set; }

    [JsonRequired]
    [Column("KAPACITA")]
    public int Kapacita { get; set; }

    [JsonRequired]
    [Column("MA_KLIMATIZACI")]
    public bool MaKlimatizaci { get; set; }

    [JsonRequired]
    [Column("ID_GARAZ")]
    public int IdGaraz { get; set; }

    [JsonRequired]
    [Column("ID_MODEL")]
    public int IdModel { get; set; }

    public string? GarazNazev { get; set; }
    public string? ModelNazev { get; set; }
}