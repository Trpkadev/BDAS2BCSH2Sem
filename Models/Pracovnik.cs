using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("PRACOVNICI")]
public class Pracovnik
{
    [Key]
    [JsonRequired]
    [Column("ID_PRACOVNIK")]
    public int IdPracovnik { get; set; }

    [JsonRequired]
    [Column("JMENO")]
    [DisplayName("Jméno")]
    public string Jmeno { get; set; } = string.Empty;

    [JsonRequired]
    [Column("PRIJMENI")]
    [DisplayName("Příjmení")]
    public string Prijmeni { get; set; } = string.Empty;

    [JsonRequired]
    [Column("TELEFONNI_CISLO")]
    [DisplayName("Telefonní číslo")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string TelefonniCislo { get; set; } = string.Empty;

    [JsonRequired]
    [Column("EMAIL")]
    [DisplayName("Email")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [JsonRequired]
    [Column("RODNE_CISLO")]
    [DisplayName("Rodné číslo")]
    [RegularExpression(@"\d{6}/\d{4}", ErrorMessage = "Invalid birth number.")]
    public string RodneCislo { get; set; } = string.Empty;

    [JsonRequired]
    [Column("HODINOVA_MZDA")]
    [DisplayName("Hodinová mzda")]
    [Range(0, 999999999, ErrorMessage = "Value must be between 0 and 999999999.")]
    public int HodinovaMzda { get; set; }

    [JsonRequired]
    [Column("ID_NADRIZENY")]
    public int? IdNadrizeny { get; set; }

    [Column("JMENO_NADRIZENEHO")]
    [DisplayName("Nadřízený")]
    public string? JmenoNadrizeneho { get; set; } = string.Empty;

    [Column("ID_UZIVATEL")]
    public int? IdUzivatel { get; set; }

    [Column("UZIVATELSKE_JMENO")]
    [DisplayName("Uživatel")]
    public string? UzivatelskeJmeno { get; set; } = string.Empty;

    [Column("SKRYTE_UDAJE")]
    [DisplayName("Skrýt údaje?")]
    public bool SkryteUdaje { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Pracovnik pracovnik &&
               IdPracovnik == pracovnik.IdPracovnik;
    }

    public override string ToString() => $"{Jmeno} {Prijmeni}";

    public override int GetHashCode() => HashCode.Combine(IdPracovnik);
}