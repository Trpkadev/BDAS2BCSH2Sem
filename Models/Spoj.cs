using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("SPOJE")]
public class Spoj
{
    [Key]
    [Column("ID_SPOJ")]
    public int IdSpoj { get; set; }
    [Column("JEDE_VE_VSEDNI_DEN")]
    public bool JedeVeVsedniDen { get; set; }
    [Column("JEDE_V_SOBOTU")]
    public bool JedeVSobotu { get; set; }
    [Column("JEDE_V_NEDELI")]
    public bool JedeVNedeli { get; set; }
    [Column("GARANTOVANA_NIZKOPODLAZNI")]
    public bool GarantovanaNizkopodlazni { get; set; } // TODO: Přejmenovat
    [Column("ID_LINKA")]
    public int IdLinka { get; set; }
}
