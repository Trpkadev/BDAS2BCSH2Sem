using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BCSH2BDAS2.Controllers;

public abstract class BaseController : Controller
{
    protected Uzivatel? LoggedUser { get; private set; }
    protected bool IsLoggedIn => LoggedUser != null;

    protected readonly TransportationContext _context;

    protected BaseController(TransportationContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        try
        {
            var serializedUser = accessor.HttpContext?.Session.GetString("User");
            if (serializedUser != null)
            {
                LoggedUser = JsonSerializer.Deserialize<Uzivatel>(serializedUser);
                ViewData["LoggedUser"] = LoggedUser;
            }
        }
        catch
        {
            LoggedUser = null;
        }
    }

    protected bool LoginInternal(string username, string password)
    {
        username = username.ToLower();
        password = OurCryptography.Instance.EncryptHash(password);
        var user = _context.Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE WHERE JMENO = {0} AND HESLO = {1}", username, password).FirstOrDefault();
        if (user == null)
        {
            return false;
        }
        LoggedUser = user;
        var serializedUser = JsonSerializer.Serialize(user);
        HttpContext.Session.SetString("User", serializedUser);
        return true;
    }

    protected void LogoutInternal()
    {
        LoggedUser = null;
        HttpContext.Session.Remove("User");
    }
}