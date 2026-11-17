using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DTGear.Models;
using DTGear.Services;

namespace DTGear.Controllers;

public class HomeController : Controller
{
    private readonly IDTGearCatalogService _catalogService;

    public HomeController(IDTGearCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    public IActionResult Index(string? category = null, string? subCategory = null, string? q = null)
    {
        var vm = _catalogService.BuildHomeViewModel(category, subCategory, q);
        return View(vm);
    }

    [HttpGet("/build-pc")]
    [HttpGet("/Home/BuildPc")]
    public IActionResult BuildPc(string? preset = null)
    {
        var vm = _catalogService.BuildHomeViewModel();
        ViewData["SelectedPreset"] = preset ?? "preset-gaming";
        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
