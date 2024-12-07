using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

public class StatistikaLogu
{
    [Column("TABULKA")]
    [DisplayName("Tabulka")]
    public string Tabulka { get; set; }

    [Column("POCET")]
    [DisplayName("Počet změn")]
    public int Pocet { get; set; }

    [Column("NEJCASTEJSI_ZMENA")]
    [DisplayName("Nejčastější změna")]
    public string NejcastejsiZmena { get; set; }

    [Column("POSLEDNI_ZMENA")]
    [DisplayName("Poslední změna")]
    public DateTime PosledniZmena { get; set; }
}