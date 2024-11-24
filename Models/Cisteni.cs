using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

public class Cisteni : Udrzba
{
    [JsonRequired]
    [Column("UMYTO_V_MYCCE")]
    [DisplayName("Umyto v myčce")]
    public bool UmytoVMycce { get; set; }

    [JsonRequired]
    [Column("CISTENO_OZONEM")]
	[DisplayName("Čištěno ozónem")]
	public bool CistenoOzonem { get; set; }
}