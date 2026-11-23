using DTGear.Data;
using DTGear.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DTGear.Controllers;

[Route("Admin/Products")]
[AdminAuthorize]
public class AdminProductsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AdminProductsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // 1. Danh sách sản phẩm (/Admin/Products)
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(string? search = null)
    {
        var query = _context.SanPhams.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var kw = search.Trim().ToLower();
            query = query.Where(p =>
                p.MaSP.ToLower().Contains(kw) ||
                p.TenSP.ToLower().Contains(kw) ||
                (p.DonViTinh != null && p.DonViTinh.ToLower().Contains(kw)) ||
                (p.GhiChu != null && p.GhiChu.ToLower().Contains(kw)));
        }

        var products = await query
            .OrderByDescending(p => p.NgayNhap)
            .ThenBy(p => p.MaSP)
            .ToListAsync();

        ViewData["SearchQuery"] = search;
        return View(products);
    }

    // 2. Thêm mới sản phẩm (GET: /Admin/Products/Create)
    [HttpGet("Create")]
    public IActionResult Create()
    {
        var model = new SanPham
        {
            SoLuong = 10,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            NgayNhap = DateTime.Now
        };
        return View(model);
    }

    // 2. Thêm mới sản phẩm (POST: /Admin/Products/Create)
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SanPham sanPham, IFormFile? imageFile)
    {
        if (!string.IsNullOrWhiteSpace(sanPham.MaSP))
        {
            sanPham.MaSP = sanPham.MaSP.Trim().ToUpperInvariant();
            bool exists = await _context.SanPhams.AnyAsync(p => p.MaSP.ToLower() == sanPham.MaSP.ToLower());
            if (exists)
            {
                ModelState.AddModelError(nameof(SanPham.MaSP), "Mã sản phẩm đã tồn tại");
            }
        }

        if (!ModelState.IsValid)
        {
            return View(sanPham);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            sanPham.HinhAnh = await SaveImageToWwwrootAsync(imageFile);
        }
        else if (string.IsNullOrWhiteSpace(sanPham.HinhAnh))
        {
            sanPham.HinhAnh = "dtgear-logo.png";
        }

        if (sanPham.NgayNhap == default)
        {
            sanPham.NgayNhap = DateTime.Now;
        }

        _context.SanPhams.Add(sanPham);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Đã thêm mới sản phẩm [{sanPham.MaSP}] {sanPham.TenSP} thành công.";
        return RedirectToAction(nameof(Index));
    }

    // 3. Cập nhật sản phẩm (GET: /Admin/Products/Edit/{id})
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return RedirectToAction(nameof(Index));
        }

        var sanPham = await _context.SanPhams.FirstOrDefaultAsync(p => p.MaSP == id);
        if (sanPham == null)
        {
            TempData["ErrorMessage"] = $"Không tìm thấy sản phẩm có mã '{id}'.";
            return RedirectToAction(nameof(Index));
        }

        return View(sanPham);
    }

    // 3. Cập nhật sản phẩm (POST: /Admin/Products/Edit/{id})
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, SanPham formModel, IFormFile? imageFile)
    {
        var existing = await _context.SanPhams.FirstOrDefaultAsync(p => p.MaSP == id);
        if (existing == null)
        {
            TempData["ErrorMessage"] = $"Không tìm thấy sản phẩm có mã '{id}'.";
            return RedirectToAction(nameof(Index));
        }

        // Đảm bảo giữ nguyên khóa chính MaSP
        ModelState.Remove(nameof(SanPham.MaSP));
        formModel.MaSP = existing.MaSP;

        if (!ModelState.IsValid)
        {
            formModel.HinhAnh = existing.HinhAnh;
            return View(formModel);
        }

        existing.TenSP = formModel.TenSP;
        existing.GiaNhap = formModel.GiaNhap;
        existing.GiaBan = formModel.GiaBan;
        existing.SoLuong = formModel.SoLuong;
        existing.DonViTinh = formModel.DonViTinh;
        existing.KhuyenMai = formModel.KhuyenMai;
        existing.GioiThieu = formModel.GioiThieu;
        existing.DacDiem = formModel.DacDiem;
        existing.GhiChu = formModel.GhiChu;
        if (formModel.NgayNhap != default)
        {
            existing.NgayNhap = formModel.NgayNhap;
        }

        // Nếu có chọn ảnh mới thì lưu ảnh mới, ngược lại giữ nguyên ảnh cũ
        if (imageFile != null && imageFile.Length > 0)
        {
            existing.HinhAnh = await SaveImageToWwwrootAsync(imageFile);
        }
        else if (!string.IsNullOrWhiteSpace(formModel.HinhAnh))
        {
            existing.HinhAnh = formModel.HinhAnh;
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Đã cập nhật sản phẩm [{existing.MaSP}] {existing.TenSP} thành công.";
        return RedirectToAction(nameof(Index));
    }

    // 4. Xóa sản phẩm (POST: /Admin/Products/Delete/{id})
    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var existing = await _context.SanPhams.FirstOrDefaultAsync(p => p.MaSP == id);
        if (existing != null)
        {
            _context.SanPhams.Remove(existing);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã xóa sản phẩm [{existing.MaSP}] {existing.TenSP} khỏi cơ sở dữ liệu.";
        }
        else
        {
            TempData["ErrorMessage"] = $"Không tìm thấy sản phẩm mã '{id}' để xóa.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveImageToWwwrootAsync(IFormFile file)
    {
        var imagesDir = Path.Combine(_env.WebRootPath, "images");
        Directory.CreateDirectory(imagesDir);

        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(ext))
        {
            ext = ".jpg";
        }

        var safeBaseName = Path.GetFileNameWithoutExtension(file.FileName);
        safeBaseName = string.Concat(safeBaseName.Where(c => char.IsLetterOrDigit(c) || c == '-' || c == '_'));
        if (string.IsNullOrWhiteSpace(safeBaseName))
        {
            safeBaseName = "sp";
        }

        var uniqueFileName = $"{safeBaseName}-{DateTime.Now:yyyyMMddHHmmss}{ext.ToLowerInvariant()}";
        var fullPath = Path.Combine(imagesDir, uniqueFileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return uniqueFileName;
    }
}
