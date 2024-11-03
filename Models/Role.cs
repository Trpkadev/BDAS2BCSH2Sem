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