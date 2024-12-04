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
    public string Nazev { get; set; } = string.Empty;

    [JsonRequired]
    [Column("PRAVA")]
    [DisplayName("Práva")]
    [Range(2, 7, ErrorMessage = "Value must be between 2 and 7.")]
    public int Prava { get; set; }

    public override string ToString() => Nazev;
}