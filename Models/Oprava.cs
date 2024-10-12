using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

public class Oprava : Udrzba
{
    [Column("POPIS_UKONU")]
    public required string PopisUkonu { get; set; }
    [Column("CENA")]
    public int Cena { get; set; }
}
