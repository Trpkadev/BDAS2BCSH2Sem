using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("TYPY_VOZIDEL")]
public class TypVozidla
{
    [Key]
    [Column("ID_TYP_VOZIDLA")]
    public int IdTypVozidla { get; set; }

    [Column("NAZEV")]
    public string Nazev { get; set; }
}