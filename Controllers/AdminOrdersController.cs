using DTGear.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DTGear.Controllers;

[Route("Admin/Orders")]
[AdminAuthorize]
public class AdminOrdersController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminOrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Danh sách đơn hàng (/Admin/Orders)
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(string? search = null)
    {
        var query = _context.HoaDons
            .Include(h => h.KhachHang)
            .Include(h => h.ChiTietHoaDons)
                .ThenInclude(ct => ct.SanPham)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var kw = search.Trim().ToLower();
            query = query.Where(h =>
                h.MaHD.ToString().Contains(kw) ||
                h.MaKH.ToLower().Contains(kw) ||
                (h.KhachHang != null && (
                    (h.KhachHang.Ho != null && h.KhachHang.Ho.ToLower().Contains(kw)) ||
                    h.KhachHang.Ten.ToLower().Contains(kw) ||
                    h.KhachHang.Phone.ToLower().Contains(kw) ||
                    h.KhachHang.DiaChi.ToLower().Contains(kw))));
        }

        var orders = await query
            .OrderByDescending(h => h.NgayLap)
            .ThenByDescending(h => h.MaHD)
            .ToListAsync();

        ViewData["SearchQuery"] = search;
        return View(orders);
    }

    // Đánh dấu Đã thanh toán
    [HttpPost("MarkPaid/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkPaid(int id)
    {
        var order = await _context.HoaDons.FirstOrDefaultAsync(h => h.MaHD == id);
        if (order != null)
        {
            order.ThanhToan = !order.ThanhToan;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = order.ThanhToan
                ? $"Đã đánh dấu Hóa đơn #{order.MaHD} là ĐÃ THANH TOÁN."
                : $"Đã chuyển trạng thái Hóa đơn #{order.MaHD} sang CHƯA THANH TOÁN.";
        }

        return RedirectToAction(nameof(Index));
    }

    // Xóa đơn hàng nếu cần trong quản trị
    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _context.HoaDons
            .Include(h => h.ChiTietHoaDons)
            .FirstOrDefaultAsync(h => h.MaHD == id);

        if (order != null)
        {
            _context.ChiTietHoaDons.RemoveRange(order.ChiTietHoaDons);
            _context.HoaDons.Remove(order);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã xóa Hóa đơn #{id} khỏi hệ thống.";
        }

        return RedirectToAction(nameof(Index));
    }
}
