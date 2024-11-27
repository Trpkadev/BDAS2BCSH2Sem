using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models;

[Table("UZIVATELE")]
public class Uzivatel : IUser
{
    [Key]
    [JsonRequired]
    [Column("ID_UZIVATEL")]
    public int IdUzivatel { get; set; }

    [JsonRequired]
    [Column("JMENO")]
    [DisplayName("Uživatelské jméno")]
    public string UzivatelskeJmeno { get; set; } = string.Empty;

    [JsonRequired]
    [Column("HESLO")]
    public string Heslo { get; set; } = string.Empty;

    public Role? Role { get; set; }

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