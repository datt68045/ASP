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
    public async Task<IActionResult> Submit([Bind(Prefix = "OrderForm")] ConsultationOrder orderForm, string? CartItemsJson = null)
    {
        if (!string.IsNullOrWhiteSpace(CartItemsJson))
        {
            orderForm.CartItemsJson = CartItemsJson;
        }

        if (!ModelState.IsValid)
        {
            var vm = _catalogService.BuildHomeViewModel();
            vm.OrderForm = orderForm;
            TempData["FormError"] = "Vui lòng kiểm tra lại các thông tin bắt buộc trong biểu mẫu.";
            return View("~/Views/Home/Index.cshtml", vm);
        }

        var savedOrder = await _orderStoreService.SaveOrderAsync(orderForm);
        TempData["ClearCartOnSuccess"] = "1";
        return RedirectToAction(nameof(Confirmation), new { id = savedOrder.MaHD ?? 1 });
    }

    [HttpGet("/Order/Confirmation/{id}")]
    [HttpGet("/Order/ThankYou")]
    public IActionResult Confirmation(string? id, string? code = null)
    {
        var lookupKey = !string.IsNullOrWhiteSpace(id) ? id : (code ?? "1");
        var order = _orderStoreService.GetOrderByCode(lookupKey);
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
