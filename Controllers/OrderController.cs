using Microsoft.AspNetCore.Mvc;
using DTGear.Models;
using DTGear.Services;

namespace DTGear.Controllers;

public class OrderController : Controller
{
    private readonly IOrderStoreService _orderStoreService;
    private readonly IDTGearCatalogService _catalogService;

    public OrderController(IOrderStoreService orderStoreService, IDTGearCatalogService catalogService)
    {
        _orderStoreService = orderStoreService;
        _catalogService = catalogService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit([Bind(Prefix = "OrderForm")] ConsultationOrder orderForm)
    {
        if (!ModelState.IsValid)
        {
            var vm = _catalogService.BuildHomeViewModel();
            vm.OrderForm = orderForm;
            TempData["FormError"] = "Vui lòng kiểm tra lại các thông tin bắt buộc trong biểu mẫu.";
            return View("~/Views/Home/Index.cshtml", vm);
        }

        var savedOrder = await _orderStoreService.SaveOrderAsync(orderForm);
        return RedirectToAction(nameof(Confirmation), new { id = savedOrder.OrderCode });
    }

    [HttpGet("/Order/Confirmation/{id}")]
    public IActionResult Confirmation(string id)
    {
        var order = _orderStoreService.GetOrderByCode(id);
        if (order == null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(order);
    }

    [HttpGet("/Order/Manage")]
    public IActionResult Manage()
    {
        var orders = _orderStoreService.GetAllOrders();
        return View(orders);
    }
}
