using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BCSH2BDAS2.Controllers;

public abstract class BaseController : Controller
{
    protected readonly TransportationContext _context;

    protected BaseController(TransportationContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        try
        {
            var serializedUser = accessor.HttpContext?.Session.GetString("LoggedUser");
            if (serializedUser != null)
                LoggedUser = JsonSerializer.Deserialize<Uzivatel>(serializedUser);
            var serializedUser2 = accessor.HttpContext?.Session.GetString("ActingUser");
            if (serializedUser2 != null)
                ActingUser = JsonSerializer.Deserialize<Uzivatel>(serializedUser2);
        }
        catch
        {
            LoggedUser = null;
            ActingUser = null;
        }
    }

    protected bool IsLoggedIn => LoggedUser != null;
    internal Uzivatel? LoggedUser { get; private set; }
    internal Uzivatel? ActingUser { get; private set; }

    protected static int GetDecryptedId(string encryptedId)
    {
        return OurCryptography.Instance.DecryptId(encryptedId);
    }

    protected bool LoginInternal(string username, string password)
    {
        username = username.ToLower();
        password = OurCryptography.EncryptHash(password);
        var user = _context.Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE WHERE JMENO = {0} AND HESLO = {1}", username, password).FirstOrDefault();
        if (user == null)
            return false;
        LoggedUser = user;
        ActingUser = user;
        var serializedUser = JsonSerializer.Serialize(user);
        HttpContext.Session.SetString("LoggedUser", serializedUser);
        HttpContext.Session.SetString("ActingUser", serializedUser);
        return true;
    }

    protected void LogoutInternal()
    {
        LoggedUser = null;
        HttpContext.Session.Remove("LoggedUser");
        HttpContext.Session.Remove("ActingUser");
    }

    protected void ActBehalfInternal(int? id)
    {
        if (LoggedUser == null || !LoggedUser.HasAtleastRole(Role.Admin))
            return;
        if (id == null)
        {
            ActingUser = LoggedUser;
        }
        else
        {
            ActingUser = _context.GetUzivatelById((int)id).Result;
        }
        var serializedUser = JsonSerializer.Serialize(ActingUser);
        HttpContext.Session.SetString("ActingUser", serializedUser);
    }
}