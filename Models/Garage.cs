using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("GARAZE")]
public class Garage
{
    [Key]
    [Column("ID_GARAZ")]
    public int IdGaraz { get; set; }

    [Column("NAZEV")]
    public string Nazev { get; set; }

    [Column("KAPACITA")]
    public int Kapacita { get; set; }
}