using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

public class Role
{
    [Key]
    [JsonRequired]
    [Column("ID_ROLE")]
    public int IdRole { get; set; }

    [JsonRequired]
    [Column("NAZEV")]
	[DisplayName("Název")]
	public string Nazev { get; set; }

    [JsonRequired]
    [Column("PRAVA")]
    public int Prava { get; set; }

    public override string ToString() => Nazev;
}