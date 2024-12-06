using System.ComponentModel;
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
    [Column("ID_VOZIDLO")]
    public int IdVozidlo { get; set; }

    [JsonRequired]
    [Column("DATUM")]
    public DateTime Datum { get; set; }

    [JsonRequired]
    [Column("TYP_UDRZBY")]
    [DisplayName("Typ údržby")]
    public char TypUdrzby { get; set; }

    [Column("NAZEV_VOZIDLA")]
    [DisplayName("Vozidlo")]
    public string? NazevVozidla { get; set; }

    [JsonRequired]
    [Column("KONEC_UDRZBY")]
    [DisplayName("Datum konce údržby")]
    public DateTime? KonecUdrzby { get; set; }
}

public class Cisteni : Udrzba
{
    [JsonRequired]
    [Column("UMYTO_V_MYCCE")]
    [DisplayName("Umyto v myčce")]
    public bool UmytoVMycce { get; set; }

    [JsonRequired]
    [Column("CISTENO_OZONEM")]
    [DisplayName("Čištěno ozonem")]
    public bool CistenoOzonem { get; set; }
}

public class Oprava : Udrzba
{
    [JsonRequired]
    [Column("POPIS_UKONU")]
    [DisplayName("Popis úkonu")]
    public string PopisUkonu { get; set; }

    [JsonRequired]
    [Column("CENA")]
    public double Cena { get; set; }
}