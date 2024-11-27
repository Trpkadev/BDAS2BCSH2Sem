using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                LoggedUser = JsonConvert.DeserializeObject<Uzivatel>(serializedUser);
            var serializedUser2 = accessor.HttpContext?.Session.GetString("ActingUser");
            if (serializedUser2 != null)
                ActingUser = JsonConvert.DeserializeObject<Uzivatel>(serializedUser2);
        }
        catch
        {
            LoggedUser = null;
            ActingUser = null;
        }
    }

    internal IUser? ActingUser { get; private set; }
    internal IUser? LoggedUser { get; private set; }
    protected bool IsLoggedIn => LoggedUser != null;

    protected static int GetDecryptedId(string encryptedId)
    {
        return OurCryptography.Instance.DecryptId(encryptedId);
    }

    protected void ActBehalfInternal(int? id)
    {
        if (LoggedUser == null || !LoggedUser.HasAdminRights())
            return;
        ActingUser = id == null ? LoggedUser : _context.GetUzivateleByIdAsync((int)id).Result;
        var serializedUser = JsonConvert.SerializeObject(ActingUser);
        HttpContext.Session.SetString("ActingUser", serializedUser);
    }

    protected async Task<bool> LoginInternal(string username, string password)
    {
        username = username.ToLower();
        password = OurCryptography.EncryptHash(password);
        IUser? user = await _context.GetUzivatelOrPracovnikByNamePwdAsync(username, password);
        if (user == null)
            return false;
        LoggedUser = user;
        ActingUser = user;
        Role? role = user is Pracovnik pracovnik ? await _context.GetRoleByIdAsync(pracovnik.IdRole) : null;
        if (role == null)
            return false;
        user.Role = role;
        var serializedUser = JsonConvert.SerializeObject(user);
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
}