using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("PRACOVNICI")]
public class Pracovnik : IUser
{
    [Key]
    [JsonRequired]
    [Column("ID_PRACOVNIK")]
    public int IdPracovnik { get; set; }

    [JsonRequired]
    [Column("UZIVATELSKE_JMENO")]
    [DisplayName("Uživatelské jméno")]
    public string UzivatelskeJmeno { get; set; } = string.Empty;

    [JsonRequired]
    [Column("HESLO")]
    public string Heslo { get; set; } = string.Empty;

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
    [Column("ID_ROLE")]
    [Range(2, 7, ErrorMessage = "Value must be between 2 and 7.")]
    public int IdRole { get; set; }

    [JsonRequired]
    [Column("HODINOVA_MZDA")]
    [DisplayName("Hodinová mzda")]
    [Range(0, 999999999, ErrorMessage = "Value must be between 0 and 999999999.")]
    public int? HodinovaMzda { get; set; }

    [JsonRequired]
    [Column("ID_NADRIZENY")]
    public int? IdNadrizeny { get; set; }

    [JsonIgnore]
    [DisplayName("Nadřízený")]
    public Pracovnik? Nadrizeny { get; set; }

    public Role? Role { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Pracovnik pracovnik &&
               IdPracovnik == pracovnik.IdPracovnik &&
               UzivatelskeJmeno == pracovnik.UzivatelskeJmeno &&
               Heslo == pracovnik.Heslo &&
               IdRole == pracovnik.IdRole;
    }

    public override string ToString() => $"{Jmeno} {Prijmeni}";

    public override int GetHashCode() => HashCode.Combine(IdPracovnik, UzivatelskeJmeno, Heslo, IdRole);
}