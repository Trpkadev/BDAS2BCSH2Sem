using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("SCHEMATA")]
public class Schema
{
    [Key]
    [JsonRequired]
    [Column("ID_SCHEMA")]
    public int IdSchema { get; set; }

    [JsonRequired]
    [Column("NAZEV_SCHEMATU")]
    public string NazevSchematu { get; set; }

    [JsonRequired]
    [Column("NAZEV_SOUBORU")]
    public string NazevSouboru { get; set; }

    [JsonRequired]
    [Column("TYP_SOUBORU")]
    public string TypSouboru { get; set; }

    [JsonRequired]
    [Column("VELIKOST_SOUBORU")]
    public int VelikostSouboru { get; set; }

    [JsonRequired]
    [Column("DATUM_ZMENY")]
    public DateTime DatumZmeny { get; set; }

    [JsonRequired]
    [Column("SOUBOR")]
    public byte[] Soubor { get; set; } = [];

    public override string ToString() => NazevSchematu;
}