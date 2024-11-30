using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("UZIVATELE")]
public class Uzivatel
{
    [Key]
    [JsonRequired]
    [Column("ID_UZIVATEL")]
    public int IdUzivatel { get; set; }

    [JsonRequired]
    [Column("UZIVATELSKE_JMENO")]
    [DisplayName("Uživatelské jméno")]
    public string UzivatelskeJmeno { get; set; } = string.Empty;

    [JsonRequired]
    [Column("HESLO")]
    public string Heslo { get; set; } = string.Empty;

    [Column("ID_ROLE")]
    public int IdRole { get; set; }

    [Column("NAZEV_ROLE")]
    [DisplayName("Role")]
    public string NazevRole { get; set; } = string.Empty;

    [Column("PRAVA")]
    public int Prava { get; set; }

    public bool HasMaintainerRights() => Prava is 3 or 6;

    public bool HasDispatchRights() => Prava is 4 or 6;

    public bool HasManagerRights() => Prava is 5 or 6;

    public bool HasAdminRights() => Prava == 6;

    public bool HasWorkerRights() => Prava is 2 or 6;

    public override bool Equals(object? obj)
    {
        return obj is Uzivatel uzivatel &&
               IdUzivatel == uzivatel.IdUzivatel &&
               UzivatelskeJmeno == uzivatel.UzivatelskeJmeno &&
               Heslo == uzivatel.Heslo;
    }

    public override string ToString() => UzivatelskeJmeno;

    public override int GetHashCode() => HashCode.Combine(IdUzivatel, UzivatelskeJmeno, Heslo);
}