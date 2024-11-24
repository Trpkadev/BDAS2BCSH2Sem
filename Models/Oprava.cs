using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

public class Oprava : Udrzba
{
    [JsonRequired]
    [Column("POPIS_UKONU")]
	[DisplayName("Popis úkonu")]
	public string PopisUkonu { get; set; } = string.Empty;

    [JsonRequired]
    [Column("CENA")]
    public int Cena { get; set; }
}