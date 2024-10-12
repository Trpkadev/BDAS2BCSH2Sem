using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("ZAZNAMY_TRASY")]
public class ZaznamTrasy
{
    [Key]
    [Column("ID_ZAZNAM")]
    public int IdZaznam { get; set; }
    [Column("CAS_PRIJEZDU")]
    public DateTime CasPrijezdu { get; set; }
    [Column("CAS_ODJEZDU")]
    public DateTime CasOdjezdu { get; set; }
    [Column("ID_VOZIDLO")]
    public int IdVozidlo { get; set; }
    [Column("ID_ZASTAVKA")]
    public int IdZastavka { get; set; }
    [Column("ID_SPOJ")]
    public int IdSpoj { get; set; }
}
