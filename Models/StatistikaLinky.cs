using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("LINKY_INFO")]
public class StatistikaLinky
{
    [Key]
    [Column("ID_LINKA")]
    public int IdLinka { get; set; }

    [Column("CISLO_LINKY")]
    [DisplayName("Číslo")]
    public int Cislo { get; set; }

    [Column("POCET_SPOJU")]
    [DisplayName("Počet spojů")]
    public int PocetSpoju { get; set; }

    [Column("PRVNI_ODJEZD")]
    [DisplayName("První odjezd")]
    public DateTime PrvniOdjezd { get; set; }

    [Column("POSLEDNI_ODJEZD")]
    [DisplayName("Poslední odjezd")]
    public DateTime PosledniOdjezd { get; set; }

    [Column("PRUM_INTERVAL")]
    [DisplayName("Průměrný interval mezi spoji")]
    public int PrumernyInterval { get; set; }
}