using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("UDRZBY")]
public class Udrzba
{
    [Key]
    [JsonRequired]
    [Column("ID_UDRZBA")]
    public int IdUdrzba { get; set; }

    [JsonRequired]
    [Column("DATUM")]
    public DateTime Datum { get; set; }

    [JsonRequired]
    [Column("ID_VOZIDLO")]
    public int IdVozidlo { get; set; }
}