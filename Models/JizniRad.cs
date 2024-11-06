using Microsoft.EntityFrameworkCore;

//using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[PrimaryKey(nameof(IdZastavka), nameof(IdSpoj))]
[Table("JIZDNI_RADY")]
public class JizniRad
{
    //TODO PK IdJizdniRad
    //[Key]
    //[JsonRequired]
    //[Column("ID_JIZDNI_RAD")]
    //public int IdJizdniRad{ get; set; }

    [JsonRequired]
    [Column("CAS_PRIJEZDU")]
    public DateTime? CasPrijezdu { get; set; }

    [JsonRequired]
    [Column("CAS_ODJEZDU")]
    public DateTime CasOdjezdu { get; set; }

    [JsonRequired]
    [Column("ID_ZASTAVKA")]
    public int IdZastavka { get; set; }

    [JsonRequired]
    [Column("ID_SPOJ")]
    public int IdSpoj { get; set; }
}