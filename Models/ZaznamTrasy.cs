using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("ZAZNAMY_TRASY")]
public class ZaznamTrasy
{
    [Key]
    [JsonRequired]
    [Column("ID_ZAZNAM")]
    public int IdZaznam { get; set; }

    [JsonRequired]
    [Column("CAS_PRIJEZDU")]
    public DateTime CasPrijezdu { get; set; }

    [JsonRequired]
    [Column("CAS_ODJEZDU")]
    public DateTime CasOdjezdu { get; set; }

    [JsonRequired]
    [Column("ID_VOZIDLO")]
    public int IdVozidlo { get; set; }

    [JsonRequired]
    [Column("ID_ZASTAVKA")]
    public int IdZastavka { get; set; }

    [JsonRequired]
    [Column("ID_SPOJ")]
    public int IdSpoj { get; set; }
}