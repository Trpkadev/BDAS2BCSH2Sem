using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("ZASTAVKA")]
public class Zastavka
{
    [Key]
    [Column("ID_ZASTAVKA")]
    public int IdZastavka { get; set; }

    [Column("NAZEV")]
    public string Nazev { get; set; }

    [Column("SOURADNICE_X")]
    public double SouradniceX { get; set; }

    [Column("SOURADNICE_Y")]
    public double SouradniceY { get; set; }

    [Column("ID_PASMO")]
    public int IdPasmo { get; set; }
}