using BCSH2BDAS2.Data;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[Route("Stops")]
public class StopsController(TransportationContext context) : Controller
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
        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleIndex, await _context.Stops.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY").ToListAsync()));
    }

    [Route("DetailsModel")]
    [HttpPost]
    public async Task<IActionResult> DetailsModel(int id)
    {
        Stop? zastavka = await _context.Stops.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
        if (zastavka == null)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDetails));
        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleDetails, zastavka));
    }

    [Route("DeleteModel")]
    [HttpPost]
    public async Task<IActionResult> DeleteModel(int id)
    {
        var zastavka = await _context.Stops.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
        if (zastavka == null)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDelete));

        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleDelete, zastavka));
    }

    [Route("EditModel")]
    [HttpPost]
    public async Task<IActionResult> EditModel(int id)
    {
        Stop? zastavka = await _context.Stops.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
        if (zastavka == null)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleEdit, StrCls.Instance.TitleEdit));
        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleEdit, zastavka));
    }

    [Route("CreateModel")]
    [HttpPost]
    public IActionResult CreateModel()
    {
        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleCreate));
    }

    //// POST: Stops/Create
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create([Bind("IdZastavka,Nazev,SouradniceX,SouradniceY,IdPasmo")] Stop zastavka)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        await _context.Database.ExecuteSqlRawAsync("INSERT INTO ST69612.ZASTAVKY (ID_ZASTAVKA, NAZEV, SOURADNICE_X, SOURADNICE_Y, ID_PASMO) VALUES ({0}, {1}, {2}, {3}, {4})", zastavka.IdZastavka, zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo);
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(zastavka);
    //}

    //// POST: Stops/Edit/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("IdZastavka,Nazev,SouradniceX,SouradniceY,IdPasmo")] Stop zastavka)
    //{
    //    if (id != zastavka.IdZastavka)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            await _context.Database.ExecuteSqlRawAsync("UPDATE ST69612.ZASTAVKY SET NAZEV = {0}, SOURADNICE_X = {1}, SOURADNICE_Y = {2}, ID_PASMO = {3} WHERE ID_ZASTAVKA = {4}", zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo, zastavka.IdZastavka);
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!ZastavkaExists(zastavka.IdZastavka))
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
    //    return View(zastavka);
    //}

    //// POST: Stops/Delete/5
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //    var zastavka = await _context.Stops.FindAsync(id);
    //    if (zastavka != null)
    //    {
    //        await _context.Database.ExecuteSqlRawAsync("DELETE FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id);
    //    }

    //    return RedirectToAction(nameof(Index));
    //}

    //private bool ZastavkaExists(int id)
    //{
    //    return _context.Stops.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).Any();
    //}
}