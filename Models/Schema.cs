using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("SCHEMATA")]
public class Schema
{
    [Key]
    [Column("ID_SCHEMA")]
    public int IdSchema { get; set; }

    [Column("NAZEV_SCHEMATU")]
    [DisplayName("Název schématu")]
    public string NazevSchematu { get; set; }

    [Column("NAZEV_SOUBORU")]
    [DisplayName("Název souboru")]
    public string? NazevSouboru { get; set; }

    [Column("TYP_SOUBORU")]
    [DisplayName("Typ souboru")]
    public string? TypSouboru { get; set; }

    [Column("VELIKOST_SOUBORU")]
    [DisplayName("Velikost souboru")]
    public int? VelikostSouboru { get; set; }

    [Column("DATUM_ZMENY")]
    [DisplayName("Datum změny")]
    public DateTime DatumZmeny { get; set; }

    [Column("SOUBOR")]
    public byte[]? Soubor { get; set; }

    [NotMapped]
    public IFormFile? UploadedFile { get; set; }

    public override string ToString() => NazevSchematu;
}