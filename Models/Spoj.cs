using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("SPOJE")]
public class Spoj
{
    [Key]
    [JsonRequired]
    [Column("ID_SPOJ")]
    public int IdSpoj { get; set; }

    [JsonRequired]
    [Column("CISLO")]
    [DisplayName("Číslo")]
    public int Cislo { get; set; }

    [JsonRequired]
    [Column("JEDE_VE_VSEDNI_DEN")]
    [DisplayName("Jede ve všední den")]
    public bool JedeVeVsedniDen { get; set; }

    [JsonRequired]
    [Column("JEDE_V_SOBOTU")]
    [DisplayName("Jede v sobotu")]
    public bool JedeVSobotu { get; set; }

    [JsonRequired]
    [Column("JEDE_V_NEDELI")]
    [DisplayName("Jede v neděli")]
    public bool JedeVNedeli { get; set; }

    [JsonRequired]
    [Column("GARANTOVANE_NIZKOPODLAZNI")]
    [DisplayName("Je garantována nízkopodlažnost")]
    public bool GarantovaneNizkopodlazni { get; set; }

    [JsonRequired]
    [Column("ID_LINKA")]
    public int IdLinka { get; set; }

    [Column("CISLO_LINKY")]
    [DisplayName("Číslo linky")]
    public int? CisloLinky { get; set; }

    public override string ToString() => $"Linka č.{CisloLinky} - Spoj č.{Cislo} ";
}