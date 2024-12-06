using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

public class PracovnikHiearchie
{
    [Column("UROVEN")]
    public int Uroven { get; set; }

    [Column("ID_PRACOVNIK")]
    public int IdPracovnik { get; set; }

    [Column("JMENO")]
    public string Jmeno { get; set; }

    [Column("PRIJMENI")]
    public string Prijmeni { get; set; }

    [Column("ID_NADRIZENY")]
    public int? IdNadrizeny { get; set; }
}
