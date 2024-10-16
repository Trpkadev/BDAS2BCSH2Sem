using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("VOZIDLA")]
public class Vehicle
{
    [Key]
    [Column("ID_VOZIDLO")]
    public int IdVozidlo { get; set; }
    [Column("ROK_VYROBY")]
    public short RokVyroby { get; set; }
    [Column("NAJETE_KILOMETRY")]
    public int NajeteKilometry { get; set; }
    [Column("KAPACITA")]
    public int Kapacita { get; set; }
    [Column("MA_KLIMATIZACI")]
    public bool MaKlimatizaci { get; set; }
    [Column("ID_GARAZ")]
    public int IdGaraz { get; set; }
    [Column("ID_MODEL")]
    public int IdModel { get; set; }
}
