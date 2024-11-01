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
    public string Jmeno { get; set; } = string.Empty;

    [JsonRequired]
    [Column("HESLO")]
    public string Heslo { get; set; } = string.Empty;

    [JsonRequired]
    [Column("ID_ROLE")]
    public int IdRole { get; set; }

	public bool HasAtleastRole(Role role) => IdRole >= (int)role;

	public override bool Equals(object? obj)
	{
		return obj is Uzivatel uzivatel &&
			   IdUzivatel == uzivatel.IdUzivatel &&
			   Jmeno == uzivatel.Jmeno &&
			   Heslo == uzivatel.Heslo &&
			   IdRole == uzivatel.IdRole;
	}

	public override string ToString()
    {
        return Jmeno;
    }
}