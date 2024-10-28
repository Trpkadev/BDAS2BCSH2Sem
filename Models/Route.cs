using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("LINKY")]
public class Route
{
    [Key]
    [Column("ID_LINKA")]
    public int IdLinka { get; set; }

    [Column("CISLO")]
    public int Cislo { get; set; }

    [Column("NAZEV")]
    public string Nazev { get; set; }

    [Column("ID_TYP_VOZIDLA")]
    public int IdTypVozidla { get; set; }
}