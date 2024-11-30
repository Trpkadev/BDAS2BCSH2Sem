﻿using System.ComponentModel;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("JIZDNI_RADY")]
public class JizdniRad
{
    [Key]
    [JsonRequired]
    [Column("ID_JIZDNI_RAD")]
    public int IdJizdniRad { get; set; }

    [JsonRequired]
    [Column("CAS_PRIJEZDU")]
    [DisplayName("Čas příjezdu")]
    public DateTime? CasPrijezdu { get; set; }

    [JsonRequired]
    [Column("CAS_ODJEZDU")]
    [DisplayName("Čas odjezdu")]
    public DateTime CasOdjezdu { get; set; }

    [JsonRequired]
    [Column("ID_ZASTAVKA")]
    public int IdZastavka { get; set; }

    [JsonRequired]
    [Column("ID_SPOJ")]
    public int IdSpoj { get; set; }

    [Column("NAZEV_ZASTAVKY")]
    [DisplayName("Název zastávky")]
    public string? NazevZastavky { get; set; }
}