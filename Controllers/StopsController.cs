using BCSH2BDAS2.Data;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

//TODO Try/catch

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
    public async Task<IActionResult> DetailsModel(int id = -1)
    {
        if (id == -1)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDelete));
        Stop? zastavka = await _context.Stops.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
        if (zastavka == null)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDetails));
        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleDetails, zastavka));
    }

    [Route("DeleteModel")]
    [HttpPost]
    public async Task<IActionResult> DeleteModel(int id = -1)
    {
        if (id == -1)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDelete));
        Stop? zastavka = await _context.Stops.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
        if (zastavka == null)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDelete));

        return StatusCode(200, new ViewInitModel(StrCls.Instance.TitleDelete, zastavka));
    }

    [Route("EditModel")]
    [HttpPost]
    public async Task<IActionResult> EditModel(int id = -1)
    {
        if (id == -1)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDelete));
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

    [Route("CreateSubmit")]
    [HttpPost]
    public async Task<IActionResult> CreateSubmit([FromBody] Stop zastavka)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);
        await _context.Database.ExecuteSqlRawAsync("INSERT INTO ST69612.ZASTAVKY (NAZEV, SOURADNICE_X, SOURADNICE_Y, ID_PASMO) VALUES ({0}, {1}, {2}, {3})", zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo);
        return StatusCode(200);
    }

    [Route("DeleteSubmit")]
    [HttpPost]
    public async Task<IActionResult> DeleteSubmit(int id = -1)
    {
        if (id == -1)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDelete));
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id);
        return StatusCode(200);
    }

    [Route("EditSubmit")]
    [HttpPost]
    public async Task<IActionResult> EditSubmit([FromBody] Stop zastavka)
    {
        if (!ModelState.IsValid)
            return StatusCode(404, new ViewInitModel(StrCls.Instance.TitleDelete));
        await _context.Database.ExecuteSqlRawAsync("UPDATE ST69612.ZASTAVKY SET NAZEV = {0}, SOURADNICE_X = {1}, SOURADNICE_Y = {2}, ID_PASMO = {3} WHERE ID_ZASTAVKA = {4}", zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo, zastavka.IdZastavka);
        return StatusCode(200);
    }
}