using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

public class Cisteni : Udrzba
{
    [Column("UMYTO_V_MYCCE")]
    public bool UmytoVMycce { get; set; }
    [Column("CISTENO_OZONEM")]
    public bool CistenoOzonem { get; set; }
}
