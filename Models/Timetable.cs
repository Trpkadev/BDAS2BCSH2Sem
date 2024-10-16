using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[PrimaryKey(nameof(IdZastavka), nameof(IdSpoj))]
[Table("JIZDNI_RADY")]
public class Timetable
{
    [Column("CAS_PRIJEZDU")]
    public DateTime? CasPrijezdu { get; set; }

    [Column("CAS_ODJEZDU")]
    public DateTime CasOdjezdu { get; set; }

    [Column("ID_ZASTAVKA")]
    public int IdZastavka { get; set; }

    [Column("ID_SPOJ")]
    public int IdSpoj { get; set; }
}