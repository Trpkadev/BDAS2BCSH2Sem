﻿using System.ComponentModel;
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
    [DisplayName("Příjezd")]
    public DateTime CasPrijezdu { get; set; }

    [JsonRequired]
    [Column("CAS_ODJEZDU")]
    [DisplayName("Odjezd")]
    public DateTime CasOdjezdu { get; set; }

    [JsonRequired]
    [Column("ID_VOZIDLO")]
    [DisplayName("Vozidlo")]
    public int IdVozidlo { get; set; }

    [JsonRequired]
    [Column("ID_JIZDNI_RAD")]
    [DisplayName("Jízdní řád")]
    public int IdJizdniRad { get; set; }

    [Column("NAZEV_VOZIDLA")]
    [DisplayName("Vozidlo")]
    public string? NazevVozidla { get; set; }

    [JsonIgnore]
    public bool UdrzbaInvalid { get; set; } = false;
}