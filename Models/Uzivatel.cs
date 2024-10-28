using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("UZIVATELE")]
public class Uzivatel
{
    [Key]
    [Column("ID_UZIVATEL")]
    public int IdUzivatel { get; set; }

    [Column("JMENO")]
    public string Jmeno { get; set; }

    [Column("UZIVATELSKE_JMENO")]
    public string UzivatelskeJmeno { get; set; }

    [Column("HESLO")]
    public string Heslo { get; set; }
}