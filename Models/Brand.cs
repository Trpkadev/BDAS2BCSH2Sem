using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("ZNACKY")]
public class Brand
{
    [Key]
    [Column("ID_ZNACKA")]
    public int IdZnacka { get; set; }

    [Column("NAZEV")]
    public string Nazev { get; set; }
}