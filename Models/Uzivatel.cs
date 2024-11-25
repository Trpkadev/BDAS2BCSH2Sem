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
    [Column("JMENO")]
    [DisplayName("Uživatelské jméno")]
    public string Jmeno { get; set; } = string.Empty;

    [JsonRequired]
    [Column("HESLO")]
    public string Heslo { get; set; } = string.Empty;

    [JsonRequired]
    [Column("ID_ROLE")]
    [Range(0, 5, ErrorMessage = "Value must be between 0 and 5.")]
    public int IdRole { get; set; }

    [JsonRequired]
    [Column("HODINOVA_MZDA")]
    [Range(0, 999999999, ErrorMessage = "Value must be between 0 and 999999999.")]
    public int? HodinovaMzda { get; set; }

    [JsonRequired]
    [Column("ID_NADRIZENY")]
    public int? IdNadrizeny { get; set; }

    [JsonIgnore]
    public Role? Role { get; set; }

    [JsonIgnore]
    public Uzivatel? Nadrizeny { get; set; }

    public bool HasMaintainerRights() => Role?.Prava == 3 || Role?.Prava == 6;

    public bool HasDispatchRights() => Role?.Prava == 4 || Role?.Prava == 6;

    public bool HasAdminRights() => Role?.Prava == 6;

    public bool HasWorkerRights() => Role?.Prava == 2 || Role?.Prava == 6;

    public bool HasUserRights() => Role?.Prava >= 1;

    public override bool Equals(object? obj)
    {
        return obj is Uzivatel uzivatel &&
               IdUzivatel == uzivatel.IdUzivatel &&
               Jmeno == uzivatel.Jmeno &&
               Heslo == uzivatel.Heslo &&
               IdRole == uzivatel.IdRole;
    }

    public override string ToString() => Jmeno;

    public override int GetHashCode() => HashCode.Combine(IdUzivatel, Jmeno, Heslo, IdRole);
}