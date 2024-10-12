using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("UDRZBY")]
public class Udrzba
{
    [Key]
    [Column("ID_UDRZBA")]
    public int IdUdrzba { get; set; }
    [Column("DATUM")]
    public DateTime Datum { get; set; }
    [Column("ID_VOZIDLO")]
    public int IdVozidlo { get; set; }
}
