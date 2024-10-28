using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BCSH2BDAS2.Models;

[Table("UZIVATELE")]
public class Uzivatele
{
	[Key]
	[Column("ID_UZIVATEL")]
	public int IdUzivatel { get; set; }
	[Column("JMENO")]
	public required string Nazev { get; set; }
}
