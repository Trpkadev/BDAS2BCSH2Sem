using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("ZNACKY")]
public class Znacka
{
    [Key]
    [Column("ID_ZNACKA")]
    public int IdZnacka { get; set; }
    [Column("NAZEV")]
    public required string Nazev { get; set; }
}
