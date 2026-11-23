using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DTGear.Data;
using DTGear.Models;
using Microsoft.EntityFrameworkCore;

namespace DTGear.Services;

public interface IOrderStoreService
{
    Task<ConsultationOrder> SaveOrderAsync(ConsultationOrder order);
    ConsultationOrder? GetOrderByCode(string orderCode);
    List<ConsultationOrder> GetAllOrders();
}

public class OrderStoreService : IOrderStoreService
{
    private readonly ApplicationDbContext _context;
    private readonly IDTGearCatalogService _catalogService;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OrderStoreService> _logger;

    public OrderStoreService(
        ApplicationDbContext context,
        IDTGearCatalogService catalogService,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<OrderStoreService> logger)
    {
        _context = context;
        _catalogService = catalogService;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<ConsultationOrder> SaveOrderAsync(ConsultationOrder order)
    {
        // 1. Chuẩn hóa Số điện thoại làm MaKH
        var cleanPhone = string.IsNullOrWhiteSpace(order.PhoneNumber)
            ? "0123456789"
            : order.PhoneNumber.Trim();
        var maKh = cleanPhone.Length > 50 ? cleanPhone[..50] : cleanPhone;

        // Tách Họ và Tên từ CustomerName
        var fullName = (order.CustomerName ?? "Nguyễn Văn A").Trim();
        string ho = "";
        string ten = fullName;
        var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (nameParts.Length > 1)
        {
            ten = nameParts[^1];
            ho = string.Join(" ", nameParts[..^1]);
        }

        var diaChi = !string.IsNullOrWhiteSpace(order.CustomerAddress)
            ? order.CustomerAddress.Trim()
            : (!string.IsNullOrWhiteSpace(order.DeliveryMethod)
                ? $"{order.DeliveryMethod} • Hẻm 99, Hành Tinh Xanh, Sao Thiên Vương"
                : "Hẻm 99, Hành Tinh Xanh, Sao Thiên Vương");

        var ghiChuKh = $"[{order.UsageNeed}] {order.ConfigurationSummary}" +
                       (!string.IsNullOrWhiteSpace(order.CustomerNote) ? $" | Ghi chú: {order.CustomerNote}" : "");

        // 2. Lưu hoặc cập nhật KHACHHANG
        var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaKH == maKh);
        if (khachHang == null)
        {
            khachHang = new KhachHang
            {
                MaKH = maKh,
                Ho = ho,
                Ten = ten,
                Phone = cleanPhone,
                DiaChi = diaChi,
                Email = string.IsNullOrWhiteSpace(order.Email) ? "nguyenvana@dtgear.vn" : order.Email.Trim(),
                GhiChu = ghiChuKh,
                NgayLap = DateTime.Now
            };
            _context.KhachHangs.Add(khachHang);
        }
        else
        {
            khachHang.Ho = ho;
            khachHang.Ten = ten;
            khachHang.Phone = cleanPhone;
            khachHang.DiaChi = diaChi;
            if (!string.IsNullOrWhiteSpace(order.Email))
            {
                khachHang.Email = order.Email.Trim();
            }
            khachHang.GhiChu = ghiChuKh;
        }

        await _context.SaveChangesAsync();

        // 3. Phân tích danh sách sản phẩm từ CartItemsJson hoặc ConfigurationSummary để lưu CT_HOADON và trừ tồn kho
        var matchedItems = ExtractOrderItems(order);
        decimal calculatedTotal = 0;

        foreach (var (sp, qty) in matchedItems)
        {
            calculatedTotal += sp.GiaBan * qty;
        }

        decimal finalTotal = order.TotalEstimatedAmount > 0
            ? order.TotalEstimatedAmount
            : calculatedTotal;

        // 4. Tạo mới HOADON
        var hoaDon = new HoaDon
        {
            MaKH = khachHang.MaKH,
            NgayLap = DateTime.Now,
            Total = finalTotal,
            ThanhToan = false
        };

        _context.HoaDons.Add(hoaDon);
        await _context.SaveChangesAsync();

        // 5. Lưu từng món vào CT_HOADON kèm trừ tồn kho SoLuong
        foreach (var (sp, qty) in matchedItems)
        {
            var ct = new ChiTietHoaDon
            {
                MaHD = hoaDon.MaHD,
                MaSP = sp.MaSP,
                SoLuong = qty
            };
            _context.ChiTietHoaDons.Add(ct);

            // Trừ tồn kho an toàn
            sp.SoLuong = Math.Max(0, sp.SoLuong - qty);
        }

        await _context.SaveChangesAsync();

        order.MaHD = hoaDon.MaHD;
        order.OrderCode = $"#{hoaDon.MaHD}";
        order.TotalEstimatedAmount = hoaDon.Total;
        order.CreatedAt = hoaDon.NgayLap;

        // Tùy chọn gửi về Google Sheets Webhook nếu được cấu hình
        var webhookUrl = _configuration["DTGear:GoogleSheetsWebhookUrl"];
        if (!string.IsNullOrWhiteSpace(webhookUrl))
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var payload = JsonSerializer.Serialize(new
                {
                    order.OrderCode,
                    order.CustomerName,
                    order.PhoneNumber,
                    order.Email,
                    order.UsageNeed,
                    order.EstimatedBudget,
                    order.DeliveryMethod,
                    order.ConfigurationSummary,
                    order.TotalEstimatedAmount,
                    order.CustomerNote,
                    CreatedAt = order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                });
                using var content = new StringContent(payload, Encoding.UTF8, "application/json");
                await client.PostAsync(webhookUrl, content);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Không thể gửi đồng bộ đơn hàng sang Google Sheets Webhook.");
            }
        }

        return order;
    }

    private List<(SanPham Product, int Quantity)> ExtractOrderItems(ConsultationOrder order)
    {
        var result = new Dictionary<string, (SanPham Product, int Quantity)>(StringComparer.OrdinalIgnoreCase);

        // Cách 1: Đọc từ CartItemsJson nếu client gửi lên
        if (!string.IsNullOrWhiteSpace(order.CartItemsJson))
        {
            try
            {
                using var doc = JsonDocument.Parse(order.CartItemsJson);
                if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var elem in doc.RootElement.EnumerateArray())
                    {
                        var id = elem.TryGetProperty("id", out var idProp) ? idProp.GetString() : null;
                        var name = elem.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;
                        var qty = elem.TryGetProperty("qty", out var qtyProp) && qtyProp.TryGetInt32(out var q) ? Math.Max(1, q) : 1;

                        var sp = _catalogService.ResolveSanPham(id ?? "") ?? _catalogService.ResolveSanPham(name ?? "");
                        if (sp != null)
                        {
                            if (result.TryGetValue(sp.MaSP, out var existing))
                            {
                                result[sp.MaSP] = (sp, existing.Quantity + qty);
                            }
                            else
                            {
                                result[sp.MaSP] = (sp, qty);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Fallback sang phân tích text bên dưới
            }
        }

        // Cách 2: Phân tích từ ConfigurationSummary (Build PC hoặc Giỏ hàng điền vào Textarea)
        if (result.Count == 0 && !string.IsNullOrWhiteSpace(order.ConfigurationSummary))
        {
            var allProducts = _context.SanPhams.ToList();
            var lines = order.ConfigurationSummary.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                int qty = 1;
                var qtyMatch = Regex.Match(line, @"\(SL:\s*(\d+)\)", RegexOptions.IgnoreCase);
                if (qtyMatch.Success && int.TryParse(qtyMatch.Groups[1].Value, out var parsedQty))
                {
                    qty = Math.Max(1, parsedQty);
                }

                var matchedSp = allProducts
                    .OrderByDescending(p => p.TenSP.Length)
                    .FirstOrDefault(p =>
                        line.Contains(p.TenSP, StringComparison.OrdinalIgnoreCase) ||
                        line.Contains(p.MaSP, StringComparison.OrdinalIgnoreCase));

                if (matchedSp != null)
                {
                    if (result.TryGetValue(matchedSp.MaSP, out var existing))
                    {
                        result[matchedSp.MaSP] = (matchedSp, existing.Quantity + qty);
                    }
                    else
                    {
                        result[matchedSp.MaSP] = (matchedSp, qty);
                    }
                }
            }
        }

        // Nếu khách chỉ gõ yêu cầu tư vấn tự do chưa khớp mã SP nào -> gắn 1 sản phẩm đại diện hoặc để trống
        return result.Values.ToList();
    }

    public ConsultationOrder? GetOrderByCode(string orderCode)
    {
        if (string.IsNullOrWhiteSpace(orderCode)) return null;

        // Trích xuất số MaHD từ chuỗi (VD: "1", "#1", "HD-1", "DTG-1")
        var cleanDigits = new string(orderCode.Where(char.IsDigit).ToArray());
        HoaDon? hd = null;

        if (int.TryParse(cleanDigits, out var maHd))
        {
            hd = _context.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefault(h => h.MaHD == maHd);
        }

        if (hd == null)
        {
            hd = _context.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPham)
                .OrderByDescending(h => h.MaHD)
                .FirstOrDefault();
        }

        return hd != null ? MapHoaDonToConsultationOrder(hd) : null;
    }

    public List<ConsultationOrder> GetAllOrders()
    {
        var hoaDons = _context.HoaDons
            .Include(h => h.KhachHang)
            .Include(h => h.ChiTietHoaDons)
                .ThenInclude(ct => ct.SanPham)
            .OrderByDescending(h => h.NgayLap)
            .ThenByDescending(h => h.MaHD)
            .ToList();

        return hoaDons.Select(MapHoaDonToConsultationOrder).ToList();
    }

    private static ConsultationOrder MapHoaDonToConsultationOrder(HoaDon hd)
    {
        var kh = hd.KhachHang;
        var summaryLines = new List<string>();

        if (hd.ChiTietHoaDons != null && hd.ChiTietHoaDons.Count > 0)
        {
            foreach (var ct in hd.ChiTietHoaDons)
            {
                var spName = ct.SanPham?.TenSP ?? ct.MaSP;
                var donGia = ct.SanPham?.GiaBan ?? 0;
                var thanhTien = string.Format("{0:N0} ₫", donGia * ct.SoLuong).Replace(",", ".");
                summaryLines.Add($"• [{ct.MaSP}] {spName} (SL: {ct.SoLuong}) = {thanhTien}");
            }
        }
        else if (!string.IsNullOrWhiteSpace(kh?.GhiChu))
        {
            summaryLines.Add(kh.GhiChu);
        }

        return new ConsultationOrder
        {
            MaHD = hd.MaHD,
            OrderCode = $"#{hd.MaHD}",
            CustomerName = kh != null ? kh.HoTenDayDu : hd.MaKH,
            PhoneNumber = kh?.Phone ?? hd.MaKH,
            Email = kh?.Email ?? "nguyenvana@dtgear.vn",
            CustomerAddress = kh?.DiaChi ?? "Hẻm 99, Hành Tinh Xanh, Sao Thiên Vương",
            UsageNeed = "Đơn đặt hàng DTGear",
            EstimatedBudget = "Theo hóa đơn",
            DeliveryMethod = kh?.DiaChi ?? "Giao hàng & lắp đặt tận nơi",
            ConfigurationSummary = summaryLines.Count > 0
                ? string.Join("\n", summaryLines)
                : "Đơn đặt hàng thiết bị phần cứng tại DTGear",
            TotalEstimatedAmount = hd.Total,
            CustomerNote = kh?.GhiChu,
            IsPaid = hd.ThanhToan,
            CreatedAt = hd.NgayLap
        };
    }
}
