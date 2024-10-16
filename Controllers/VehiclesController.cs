using BCSH2BDAS2.Data;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[Route("Vehicles")]
public class VehiclesController(TransportationContext context) : Controller
{
    private readonly TransportationContext _context = context;

    [Route("")]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [Route("Details")]
    [HttpGet]
    public IActionResult Details()
    {
        return View();
    }

    [Route("Delete")]
    [HttpGet]
    public IActionResult Delete()
    {
        return View();
    }

    [Route("Edit")]
    [HttpGet]
    public IActionResult Edit()
    {
        return View();
    }

    [Route("Create")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Route("IndexModel")]
    [HttpPost]
    public async Task<IActionResult> IndexModel()
    {
        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleIndex, await _context.Vehicles.FromSqlRaw("SELECT * FROM ST69612.VOZIDLA").ToListAsync()));
    }

    [Route("DetailsModel")]
    [HttpPost]
    public async Task<IActionResult> DetailsModel(int id)
    {
        Vehicle? vehicle = await _context.Vehicles.FromSqlRaw("SELECT * FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id).FirstOrDefaultAsync();
        if (vehicle == null)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDetails));
        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleDetails, vehicle));
    }

    [Route("DeleteModel")]
    [HttpPost]
    public async Task<IActionResult> DeleteModel(int id)
    {
        Vehicle? vehicle = await _context.Vehicles.FromSqlRaw("SELECT * FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id).FirstOrDefaultAsync();
        if (vehicle == null)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDelete));

        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleDelete, vehicle));
    }

    [Route("EditModel")]
    [HttpPost]
    public async Task<IActionResult> EditModel(int id)
    {
        Vehicle? vehicle = await _context.Vehicles.FromSqlRaw("SELECT * FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id).FirstOrDefaultAsync();
        if (vehicle == null)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleEdit, StrCls.Instance.TitleEdit));
        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleEdit, vehicle));
    }

    [Route("CreateModel")]
    [HttpPost]
    public IActionResult CreateModel()
    {
        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleCreate));
    }

    //// POST: Vehicles/Create
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create([Bind("IdVozidlo,RokVyroby,NajeteKilometry,Kapacita,MaKlimatizaci,IdGaraz,IdModel")] Vehicle vozidlo)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        await _context.Database.ExecuteSqlRawAsync("INSERT INTO ST69612.VOZIDLA (ID_VOZIDLO, ROK_VYROBY, NAJETE_KILOMETRY, KAPACITA, MA_KLIMATIZACI, ID_GARAZ, ID_MODEL) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})", vozidlo.IdVozidlo, vozidlo.RokVyroby, vozidlo.NajeteKilometry, vozidlo.Kapacita, vozidlo.MaKlimatizaci, vozidlo.IdGaraz, vozidlo.IdModel);
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(vozidlo);
    //}

    //// POST: Vehicles/Edit/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("IdVozidlo,RokVyroby,NajeteKilometry,Kapacita,MaKlimatizaci,IdGaraz,IdModel")] Vehicle vozidlo)
    //{
    //    if (id != vozidlo.IdVozidlo)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            await _context.Database.ExecuteSqlRawAsync("UPDATE ST69612.VOZIDLA SET ROK_VYROBY = {0}, NAJETE_KILOMETRY = {1}, KAPACITA = {2}, MA_KLIMATIZACI = {3}, ID_GARAZ = {4}, ID_MODEL = {5} WHERE ID_VOZIDLO = {6}", vozidlo.RokVyroby, vozidlo.NajeteKilometry, vozidlo.Kapacita, vozidlo.MaKlimatizaci, vozidlo.IdGaraz, vozidlo.IdModel, vozidlo.IdVozidlo);
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!VozidloExists(vozidlo.IdVozidlo))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(vozidlo);
    //}

    //// POST: Vehicles/Delete/5
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //    var vozidlo = await _context.Vehicles.FindAsync(id);
    //    if (vozidlo != null)
    //    {
    //        await _context.Database.ExecuteSqlRawAsync("DELETE FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id);
    //    }

    //    return RedirectToAction(nameof(Index));
    //}

    //private bool VozidloExists(int id)
    //{
    //    return _context.Vehicles.FromSqlRaw("SELECT * FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id).Any();
    //}
}