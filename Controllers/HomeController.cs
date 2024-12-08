﻿using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("")]
public class HomeController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("Plan")]
    public async Task<IActionResult> Plan()
    {
        try
        {
            if (ActingUser == null)
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Zastavky = await _context.GetZastavkyAsync() ?? [];
            return View();
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpPost]
    [Route("Plan")]
    public async Task<IActionResult> Plan(string from, string to, string time)
    {
        try
        {
            if (ActingUser == null)
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Plan));
            }
            int idZastavkaFrom = OurCryptography.Instance.DecryptId(from);
            int idZastavkaTo = OurCryptography.Instance.DecryptId(to);
            if (!ModelState.IsValid || idZastavkaFrom == idZastavkaTo)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Plan));
            }

            var spoje = await _context.GetSpojeAsync() ?? [];
            var jizdniRady = await _context.GetJizdniRadyAsync() ?? [];

            DateTime dateTime = DateTime.Parse(time, CultureInfo.CurrentCulture);

            var a = (await _context.VyhledaniSpojeAsync(idZastavkaFrom, idZastavkaTo, dateTime))?.Reverse() ?? [];

            List<int> zastavkyInts = [];
            List<int> spojeInts = [];

            foreach (var asdasd in a)
            {
                if (!zastavkyInts.Contains(asdasd.IdZastavkaFrom))
                {
                    zastavkyInts.Add(asdasd.IdZastavkaFrom);
                    spojeInts.Add(asdasd.IdSpojFrom);
                }
                zastavkyInts.Add(asdasd.IdZastavka);
                spojeInts.Add(asdasd.IdSpoj);
            }

            var b = new List<VyhledaniSpojeViewModel>();
            for (int i = 0; i < zastavkyInts.Count; i++)
            {
                int zastavkaId = zastavkyInts[i];
                var jizdniRad = jizdniRady.FirstOrDefault(a => a.IdZastavka == zastavkaId && a.IdSpoj == spojeInts[i]) ?? throw new Exception();
                var spoj = spoje.FirstOrDefault(spoj => spoj.IdSpoj == spojeInts[i]) ?? throw new Exception();
                b.Add(new VyhledaniSpojeViewModel
                {
                    NazevZastavky = jizdniRad.NazevZastavky ?? "",
                    NazevSpoje = spoj.ToString(),
                    CasPrijezdu = dateTime.Date + (jizdniRad.CasPrijezdu ?? jizdniRad.CasOdjezdu).TimeOfDay,
                    CasOdjezdu = dateTime.Date + jizdniRad.CasOdjezdu.TimeOfDay
                });
            }
            return View(b);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    public class VyhledaniSpojeResponseModel
    {
        [JsonProperty("ID_ZASTAVKA")]
        public int IdZastavka { get; set; }

        [JsonProperty("ID_ZASTAVKA_FROM")]
        public int IdZastavkaFrom { get; set; }

        [JsonProperty("TRIP_LENGTH")]
        public double TripLength { get; set; }

        [JsonProperty("ID_SPOJ")]
        public int IdSpoj { get; set; }

        [JsonProperty("ID_SPOJ_FROM")]
        public int IdSpojFrom { get; set; }
    }

    public class VyhledaniSpojeViewModel
    {
        [DisplayName("Zastávka")]
        public string NazevZastavky { get; set; }

        [DisplayName("Spoj")]
        public string NazevSpoje { get; set; }

        [DisplayName("Čas příjezdu")]
        public DateTime CasPrijezdu { get; set; }

        [DisplayName("Čas odjezdu")]
        public DateTime CasOdjezdu { get; set; }
    }

    [HttpGet]
    [Route("Timetable")]
    public async Task<IActionResult> Timetable(string? encryptedId)
    {
        try
        {
            if (ActingUser == null)
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Plan));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Plan));
            }

            var vm = new TimetableViewModel()
            {
                Routes = await _context.GetLinkyAsync() ?? [],
            };
            if (encryptedId == null)
                return View(vm);

            int linkaId = OurCryptography.Instance.DecryptId(encryptedId);
            var linka = await _context.GetLinkaByIdAsync(linkaId);
            if (linka == null)
            {
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
                return RedirectToAction(nameof(Index));
            }

            var jr = await _context.JizdniRady.FromSql($"SELECT JR.*, Z.NAZEV AS NAZEV_ZASTAVKY FROM JIZDNI_RADY JR JOIN SPOJE S ON S.ID_SPOJ = JR.ID_SPOJ JOIN ZASTAVKY Z ON JR.ID_ZASTAVKA = Z.ID_ZASTAVKA WHERE ID_LINKA = {linkaId}").ToListAsync();
            var zastavky = jr.Select(jr => jr.NazevZastavky).Distinct().ToList();

            vm.Timetable = [];
            vm.CisloLinky = linka.Cislo;

            foreach (var zastavka in zastavky.Cast<string>())
                vm.Timetable[zastavka] = jr.Where(jr => jr.NazevZastavky == zastavka).ToList();

            return View(vm);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }
}