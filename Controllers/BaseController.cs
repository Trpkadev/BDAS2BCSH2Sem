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
            var serializedUser = accessor.HttpContext?.Session.GetString(Resource.LOGGED_USER);
            if (serializedUser != null)
                LoggedUser = JsonConvert.DeserializeObject<Uzivatel>(serializedUser);
            var serializedUser2 = accessor.HttpContext?.Session.GetString(Resource.ACTING_USER);
            if (serializedUser2 != null)
                ActingUser = JsonConvert.DeserializeObject<Uzivatel>(serializedUser2);
        }
        catch
        {
            LoggedUser = null;
            ActingUser = null;
        }
    }

    internal Uzivatel? ActingUser { get; private set; }
    internal Uzivatel? LoggedUser { get; private set; }
    protected bool IsLoggedIn => LoggedUser != null;

    protected static int GetDecryptedId(string encryptedId)
    {
        return OurCryptography.Instance.DecryptId(encryptedId);
    }

    protected void ActBehalfInternal(int? id)
    {
        if (LoggedUser == null || !LoggedUser.HasAdminRights())
            return;
        ActingUser = id == null ? LoggedUser : _context.GetUzivatelByIdAsync((int)id).Result;
        var serializedUser = JsonConvert.SerializeObject(ActingUser);
        HttpContext.Session.SetString(Resource.ACTING_USER, serializedUser);
    }

    protected async Task<bool> LoginInternal(string username, string password)
    {
        username = username.ToLower();
        password = OurCryptography.EncryptHash(password);
        Uzivatel? user = await _context.GetUzivatelByNamePwdAsync(username, password);
        if (user == null)
            return false;
        var serializedUser = JsonConvert.SerializeObject(user);
        HttpContext.Session.SetString(Resource.LOGGED_USER, serializedUser);
        HttpContext.Session.SetString(Resource.ACTING_USER, serializedUser);
        return true;
    }

    protected void LogoutInternal()
    {
        LoggedUser = null;
        HttpContext.Session.Remove(Resource.LOGGED_USER);
        HttpContext.Session.Remove(Resource.ACTING_USER);
    }

    protected void SetErrorMessage(string? message = null) => TempData[Resource.TEMPDATA_ERROR] = message ?? Resource.GENERIC_ERROR;

    protected void SetSuccessMessage(string? message = null) => TempData[Resource.TEMPDATA_SUCCESS] = message ?? Resource.GENERIC_SUCCESS;

    protected IActionResult RedirectToHome() => RedirectToAction("Index", "Home");
}