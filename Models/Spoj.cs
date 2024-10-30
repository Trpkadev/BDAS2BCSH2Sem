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
    [Column("JEDE_VE_VSEDNI_DEN")]
    public bool JedeVeVsedniDen { get; set; }

    [JsonRequired]
    [Column("JEDE_V_SOBOTU")]
    public bool JedeVSobotu { get; set; }

    [JsonRequired]
    [Column("JEDE_V_NEDELI")]
    public bool JedeVNedeli { get; set; }

    [JsonRequired]
    [Column("GARANTOVANA_NIZKOPODLAZNI")]
    public bool GarantovanaNizkopodlazni { get; set; } // TODO: Přejmenovat

    [JsonRequired]
    [Column("ID_LINKA")]
    public int IdLinka { get; set; }
}