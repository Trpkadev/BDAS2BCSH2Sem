using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BCSH2BDAS2.Models;

[Table("UZIVATELE")]
public class Uzivatel
{
	[Key]
	[Column("ID_UZIVATEL")]
	public int IdUzivatel { get; set; }
	[Column("JMENO")]
	public required string Jmeno { get; set; }
	[Column("UZIVATELSKE_JMENO")]
	public required string UzivatelskeJmeno { get; set; }
	[Column("HESLO")]
	public required string Hesla { get; set; }
}
