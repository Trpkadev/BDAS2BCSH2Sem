//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

namespace BCSH2BDAS2.Models;

//TODO Load values from DB instead of hardcoding
public enum Role
{
    UserNotLoggedIn = 0,
    User = 1,
    Maintainer = 2,
    Dispatcher = 3,
    Admin = 4,
    Owner = 5
}

//public class Role
//{
//    [Key]
//    [JsonRequired]
//    [Column("ID_ROLE")]
//    public int IdRole { get; set; }

//    [JsonRequired]
//    [Column("NAZEV")]
//    public int Nazev { get; set; }
//}