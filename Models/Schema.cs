using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("SCHEMATA")]
public class Schema
{
    [Key]
    [Column("ID_SCHEMA")]
    [JsonRequired]
    public int IdSchema { get; set; }

    [Column("NAZEV_SCHEMATU")]
    [DisplayName("Název schématu")]
    public string NazevSchematu { get; set; } = string.Empty;

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
    public DateTime? DatumZmeny { get; set; }

    [Column("SOUBOR", TypeName = "BLOB")]
    public byte[]? Soubor { get; set; }

    [Column("ID_UZIVATEL")]
    public int? IdUzivatel { get; set; }

    [Column("UZIVATELSKE_JMENO")]
    [DisplayName("Uživatelské jméno")]
    public string? UzivatelskeJmeno { get; set; }

    [Column("ID_PRACOVNIK")]
    public int? IdPracovnik { get; set; }

    [Column("JMENO_PRACOVNIKA")]
    [DisplayName("Jméno pracovníka")]
    public string? JmenoPracovnika { get; set; }

    [NotMapped]
    public IFormFile? UploadedFile { get; set; }

    public override string ToString() => NazevSchematu;
}