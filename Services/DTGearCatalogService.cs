using DTGear.Data;
using DTGear.Models;
using Microsoft.EntityFrameworkCore;

namespace DTGear.Services;

public interface IDTGearCatalogService
{
    List<NeedSegmentCard> GetNeedSegments();
    List<Product> GetAllProducts();
    Product? GetProductById(string id);
    SanPham? ResolveSanPham(string idOrSlugOrName);
    List<PcPreset> GetPresets();
    List<CustomerReview> GetCustomerReviews();
    HomeViewModel BuildHomeViewModel(string? category = null, string? subCategory = null, string? q = null);
}

public class DTGearCatalogService : IDTGearCatalogService
{
    private readonly ApplicationDbContext _context;
    private readonly List<NeedSegmentCard> _needSegments;
    private readonly List<PcPreset> _presets;
    private readonly List<CustomerReview> _reviews;

    public DTGearCatalogService(ApplicationDbContext context)
    {
        _context = context;
        _needSegments = SeedNeedSegments();
        _presets = SeedPresets();
        _reviews = SeedReviews();
    }

    public List<NeedSegmentCard> GetNeedSegments() => _needSegments;

    public List<Product> GetAllProducts()
    {
        try
        {
            var dbItems = _context.SanPhams
                .AsNoTracking()
                .OrderByDescending(x => x.NgayNhap)
                .ThenBy(x => x.MaSP)
                .ToList();

            if (dbItems.Count > 0)
            {
                return dbItems.Select(MapSanPhamToProduct).ToList();
            }
        }
        catch
        {
            // Fallback an toàn nếu DB đang khởi tạo
        }

        return SeedData.GetInitialSanPhams().Select(MapSanPhamToProduct).ToList();
    }

    public Product? GetProductById(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return null;
        var all = GetAllProducts();
        var match = all.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (match != null) return match;

        // Hỗ trợ tìm theo MaSP (VD: LAP001) hoặc slug
        var sp = ResolveSanPham(id);
        return sp != null ? MapSanPhamToProduct(sp) : null;
    }

    public SanPham? ResolveSanPham(string idOrSlugOrName)
    {
        if (string.IsNullOrWhiteSpace(idOrSlugOrName)) return null;
        var key = idOrSlugOrName.Trim();
        var list = _context.SanPhams.ToList();

        // 1. Khớp chính xác MaSP
        var byCode = list.FirstOrDefault(s => s.MaSP.Equals(key, StringComparison.OrdinalIgnoreCase));
        if (byCode != null) return byCode;

        // 2. Khớp theo slug lưu trong GhiChu
        var bySlug = list.FirstOrDefault(s =>
        {
            var slug = ExtractSlugFromGhiChu(s.GhiChu);
            return !string.IsNullOrWhiteSpace(slug) && slug.Equals(key, StringComparison.OrdinalIgnoreCase);
        });
        if (bySlug != null) return bySlug;

        // 3. Khớp theo Tên sản phẩm
        var byName = list.FirstOrDefault(s =>
            s.TenSP.Equals(key, StringComparison.OrdinalIgnoreCase) ||
            key.Contains(s.TenSP, StringComparison.OrdinalIgnoreCase) ||
            s.TenSP.Contains(key, StringComparison.OrdinalIgnoreCase));
        return byName;
    }

    public List<PcPreset> GetPresets() => _presets;
    public List<CustomerReview> GetCustomerReviews() => _reviews;

    public HomeViewModel BuildHomeViewModel(string? category = null, string? subCategory = null, string? q = null)
    {
        var products = GetAllProducts();
        var vm = new HomeViewModel
        {
            NeedSegments = _needSegments,
            AllProducts = products,
            FeaturedLaptops = products.Where(p => p.Category == "Laptop").ToList(),
            FeaturedPcs = products.Where(p => p.Category == "PC").ToList(),
            FeaturedComponents = products.Where(p => p.Category == "Component" && p.IsFeatured).ToList(),
            FeaturedAccessories = products.Where(p => p.Category == "Accessory").ToList(),
            Cpus = products.Where(p => p.SubCategory == "CPU").ToList(),
            Mainboards = products.Where(p => p.SubCategory == "Mainboard").ToList(),
            Rams = products.Where(p => p.SubCategory == "RAM").ToList(),
            Vgas = products.Where(p => p.SubCategory == "VGA").ToList(),
            Ssds = products.Where(p => p.SubCategory == "SSD").ToList(),
            Psus = products.Where(p => p.SubCategory == "PSU").ToList(),
            Cases = products.Where(p => p.SubCategory == "Case").ToList(),
            Monitors = products.Where(p => p.SubCategory == "Monitor").ToList(),
            BuildAccessories = products.Where(p => p.Category == "Accessory").ToList(),
            Presets = _presets,
            Reviews = _reviews,
            ActiveCategory = category,
            ActiveSubCategory = subCategory,
            SearchQuery = q
        };

        return vm;
    }

    private static string? ExtractSlugFromGhiChu(string? ghiChu)
    {
        if (string.IsNullOrWhiteSpace(ghiChu) || !ghiChu.Contains('|')) return null;
        var parts = ghiChu.Split('|');
        return parts.Length >= 9 && !string.IsNullOrWhiteSpace(parts[8]) ? parts[8].Trim() : null;
    }

    public static Product MapSanPhamToProduct(SanPham sp)
    {
        string category = "Accessory";
        string subCategory = "Phụ kiện";
        string segmentBadge = sp.KhuyenMai ? "Khuyến mãi Hot" : "Chính hãng DTGear";
        string needGroup = "office";
        string? socket = null;
        string? ramType = null;
        int wattage = 0;
        bool isFeatured = sp.KhuyenMai;
        string id = sp.MaSP;

        if (!string.IsNullOrWhiteSpace(sp.GhiChu) && sp.GhiChu.Contains('|'))
        {
            var parts = sp.GhiChu.Split('|');
            if (parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0])) category = parts[0].Trim();
            if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1])) subCategory = parts[1].Trim();
            if (parts.Length > 2 && !string.IsNullOrWhiteSpace(parts[2])) segmentBadge = parts[2].Trim();
            if (parts.Length > 3 && !string.IsNullOrWhiteSpace(parts[3])) needGroup = parts[3].Trim();
            if (parts.Length > 4 && !string.IsNullOrWhiteSpace(parts[4])) socket = parts[4].Trim();
            if (parts.Length > 5 && !string.IsNullOrWhiteSpace(parts[5])) ramType = parts[5].Trim();
            if (parts.Length > 6 && int.TryParse(parts[6].Trim(), out var w)) wattage = w;
            if (parts.Length > 7 && bool.TryParse(parts[7].Trim(), out var feat)) isFeatured = feat;
            if (parts.Length > 8 && !string.IsNullOrWhiteSpace(parts[8])) id = parts[8].Trim();
        }
        else
        {
            // Tự động nhận diện nhóm sản phẩm cho các sản phẩm Admin thêm mới
            var text = $"{sp.MaSP} {sp.TenSP} {sp.GhiChu}".ToLowerInvariant();
            if (text.Contains("laptop") || text.Contains("macbook") || text.Contains("vivobook") || text.Contains("ideapad") || text.Contains("legion"))
            {
                category = "Laptop";
                subCategory = "Laptop Văn phòng";
                needGroup = "office";
            }
            else if (text.Contains("pc ") || text.StartsWith("pc") || text.Contains("workstation"))
            {
                category = "PC";
                subCategory = "PC Gaming";
                needGroup = "gaming";
            }
            else if (text.Contains("cpu") || text.Contains("core i") || text.Contains("ryzen"))
            {
                category = "Component";
                subCategory = "CPU";
                needGroup = "gaming";
            }
            else if (text.Contains("main") || text.Contains("b760") || text.Contains("z790") || text.Contains("h610"))
            {
                category = "Component";
                subCategory = "Mainboard";
                needGroup = "gaming";
            }
            else if (text.Contains("ram") || text.Contains("ddr4") || text.Contains("ddr5"))
            {
                category = "Component";
                subCategory = "RAM";
                needGroup = "gaming";
            }
            else if (text.Contains("vga") || text.Contains("rtx") || text.Contains("gtx"))
            {
                category = "Component";
                subCategory = "VGA";
                needGroup = "gaming";
            }
            else if (text.Contains("ssd") && !text.Contains("di động"))
            {
                category = "Component";
                subCategory = "SSD";
                needGroup = "gaming";
            }
            else if (text.Contains("nguồn") || text.Contains("psu"))
            {
                category = "Component";
                subCategory = "PSU";
                needGroup = "gaming";
            }
            else if (text.Contains("case") || text.Contains("vỏ"))
            {
                category = "Component";
                subCategory = "Case";
                needGroup = "gaming";
            }
            else if (text.Contains("màn hình") || text.Contains("monitor"))
            {
                category = "Component";
                subCategory = "Monitor";
                needGroup = "office";
            }
            else if (text.Contains("tai nghe"))
            {
                category = "Accessory";
                subCategory = "Tai nghe";
                needGroup = "accessory";
            }
            else if (text.Contains("chuột"))
            {
                category = "Accessory";
                subCategory = "Chuột";
                needGroup = "accessory";
            }
            else if (text.Contains("phím"))
            {
                category = "Accessory";
                subCategory = "Bàn phím";
                needGroup = "accessory";
            }
            else
            {
                category = "Accessory";
                subCategory = !string.IsNullOrWhiteSpace(sp.GhiChu) ? sp.GhiChu.Trim() : "USB";
                needGroup = "accessory";
            }
        }

        string imageUrl = "/images/dtgear-logo.png";
        if (!string.IsNullOrWhiteSpace(sp.HinhAnh))
        {
            var rawImg = sp.HinhAnh.Trim();
            if (rawImg.StartsWith("http", StringComparison.OrdinalIgnoreCase) || rawImg.StartsWith("/"))
            {
                imageUrl = rawImg;
            }
            else
            {
                imageUrl = $"/images/{rawImg.TrimStart('/')}";
            }
        }

        decimal? originalPrice = sp.KhuyenMai ? Math.Round(sp.GiaBan * 1.12m / 10000m) * 10000m : null;

        return new Product
        {
            Id = id,
            Name = sp.TenSP,
            Category = category,
            SubCategory = subCategory,
            SegmentBadge = segmentBadge,
            NeedGroup = needGroup,
            ShortSpecs = !string.IsNullOrWhiteSpace(sp.DacDiem) ? sp.DacDiem : "Chính hãng DTGear • Bảo hành 36 tháng",
            HighlightNote = !string.IsNullOrWhiteSpace(sp.GioiThieu) ? sp.GioiThieu : "Sản phẩm chính hãng nguyên seal tại hệ thống DTGear",
            Price = sp.GiaBan,
            OriginalPrice = originalPrice,
            ImageUrl = imageUrl,
            Socket = socket,
            RamType = ramType,
            Wattage = wattage,
            IsFeatured = isFeatured
        };
    }

    private static List<NeedSegmentCard> SeedNeedSegments() => new()
    {
        new NeedSegmentCard
        {
            Code = "office",
            Title = "Học tập & Văn phòng",
            BudgetRange = "Ngân sách: 7 – 16 triệu",
            Description = "Laptop mỏng nhẹ pin lâu, PC văn phòng vận hành êm ái, màn hình IPS chống mỏi mắt cho sinh viên & doanh nghiệp.",
            IconName = "briefcase",
            HighlightItems = "Laptop Sinh viên • PC Office • Màn hình 100Hz"
        },
        new NeedSegmentCard
        {
            Code = "gaming",
            Title = "Gaming & Giải trí",
            BudgetRange = "Ngân sách: 15 – 38 triệu",
            Description = "Chiến mượt Esports & AAA ở độ phân giải Full HD / 2K. Tối ưu hiệu năng CPU + Card đồ họa RTX 40-Series.",
            IconName = "gamepad-2",
            HighlightItems = "Laptop Gaming • PC RTX 4060/4070 • Màn 180Hz"
        },
        new NeedSegmentCard
        {
            Code = "creator",
            Title = "Đồ họa & Kỹ thuật",
            BudgetRange = "Ngân sách: 22 – 65+ triệu",
            Description = "Cấu hình đa nhân chuyên Render 3DsMax, AutoCAD, Lumion, Premiere kết hợp màn hình chuẩn màu ΔE < 2.",
            IconName = "layers",
            HighlightItems = "Workstation PC • RAM 32-64GB • Màn ProArt 4K"
        },
        new NeedSegmentCard
        {
            Code = "accessory",
            Title = "Phụ kiện & Gear Tiện ích",
            BudgetRange = "Chỉ từ: 190.000₫ – 2,9 triệu",
            Description = "Tai nghe chụp tai/nhét tai, USB 3.2 tốc độ cao, ổ cứng SSD di động, chuột công thái học và bàn phím cơ.",
            IconName = "headphones",
            HighlightItems = "Tai nghe • USB 64/128GB • Chuột • Bàn phím"
        }
    };

    private static List<PcPreset> SeedPresets() => new()
    {
        new PcPreset
        {
            Id = "preset-office",
            Title = "Cấu hình Học tập & Văn phòng",
            Subtitle = "Êm ái, tiết kiệm điện, khởi động siêu tốc",
            Badge = "Tiết kiệm • ~10,9 Triệu",
            CpuId = "cpu-i3-12100f",
            MainboardId = "main-h610m",
            RamId = "ram-16gb-ddr4",
            VgaId = "vga-gt1030-office",
            SsdId = "ssd-kingston-500gb",
            PsuId = "psu-cm-500w",
            CaseId = "case-deepcool-ch370",
            MonitorId = "mon-lg-24-100hz",
            AccessoryIds = new List<string> { "acc-usb-kingston-64gb", "acc-mouse-logitech-m350s" }
        },
        new PcPreset
        {
            Id = "preset-gaming",
            Title = "Cấu hình Gaming & Giải trí 2K",
            Subtitle = "Chiến mượt Esports & AAA, livestream ổn định",
            Badge = "Phổ biến nhất • ~29,9 Triệu",
            CpuId = "cpu-i5-13400f",
            MainboardId = "main-b760m-wifi",
            RamId = "ram-32gb-ddr5",
            VgaId = "vga-rtx-4060",
            SsdId = "ssd-samsung-990evo-1tb",
            PsuId = "psu-corsair-650w",
            CaseId = "case-nzxt-h5-flow",
            MonitorId = "mon-asus-27-2k-180hz",
            AccessoryIds = new List<string> { "acc-headphone-logitech-g435", "acc-kb-keychron-k8pro" }
        },
        new PcPreset
        {
            Id = "preset-creator",
            Title = "Cấu hình Đồ họa & Kiến trúc Studio",
            Subtitle = "Render 3DsMax, AutoCAD, Lumion, Premiere 4K",
            Badge = "Chuyên nghiệp • ~62,4 Triệu",
            CpuId = "cpu-i7-14700k",
            MainboardId = "main-z790-proart",
            RamId = "ram-64gb-ddr5",
            VgaId = "vga-rtx-4070-super",
            SsdId = "ssd-samsung-990pro-2tb",
            PsuId = "psu-corsair-750w-gold",
            CaseId = "case-fractal-north",
            MonitorId = "mon-proart-27-4k",
            AccessoryIds = new List<string> { "acc-mouse-mx-master-3s", "acc-ssd-portable-t7" }
        }
    };

    private static List<CustomerReview> SeedReviews() => new()
    {
        new CustomerReview
        {
            CustomerName = "Trần Minh Quân",
            Role = "Kiến trúc sư Nội thất • Thuận An, Bình Dương",
            PurchasedItem = "Bộ PC ProArt Core i7-14700K + Màn hình ASUS ProArt 4K + Chuột MX Master 3S",
            Comment = "Mình ấn tượng nhất là đội ngũ DTGear tư vấn cực kỳ đúng nhu cầu, không vẽ vời linh kiện thừa. Thùng máy Fractal North đi dây giấu cáp gọn đẹp như phòng trưng bày Apple, màn hình được kỹ thuật viên cân màu kỹ trước khi bàn giao.",
            DateText = "Đã mua tháng trước • Bảo hành 36 tháng chính hãng"
        },
        new CustomerReview
        {
            CustomerName = "Lê Thanh Trúc",
            Role = "Chuyên viên Tài chính • Lái Thiêu, Thuận An, Bình Dương",
            PurchasedItem = "Laptop Lenovo IdeaPad Slim 5 OLED + USB SanDisk 128GB + Tai nghe Sony",
            Comment = "Mình tìm mua laptop làm việc kèm USB và tai nghe họp trực tuyến. Giao diện web DTGear sáng sủa, dễ so sánh giá từng phân khúc. Đặt hàng buổi sáng thì đầu giờ chiều đã nhận được máy nguyên seal kèm đầy đủ phụ kiện.",
            DateText = "Đã mua 2 tuần trước • Giao hàng nhanh trong ngày"
        },
        new CustomerReview
        {
            CustomerName = "Hoàng Gia Bách",
            Role = "Kỹ sư Phần mềm • TP. Thủ Đức, TP.HCM",
            PurchasedItem = "Build PC Core i5-13400F + RTX 4060 White + Bàn phím Keychron K8 Pro",
            Comment = "Công cụ Build PC của DTGear tự kiểm tra tương thích RAM DDR5 với Mainboard và tính sẵn công suất nguồn nên mình tự chọn cấu hình chỉ mất 3 phút. Giá linh kiện lẻ và phụ kiện đi kèm rất hợp lý, minh bạch.",
            DateText = "Đã mua tuần trước • Hỗ trợ lắp đặt tận nơi"
        },
        new CustomerReview
        {
            CustomerName = "Phạm Minh Đức",
            Role = "Chuyên viên Thiết kế Đồ họa 3D • Bình Dương",
            PurchasedItem = "Laptop ASUS ROG Zephyrus G16 Studio OLED + Ổ cứng SSD Di động Samsung T7 1TB",
            Comment = "Màn hình OLED trên Zephyrus G16 đẹp xuất sắc, kết xuất đồ họa mượt mà. Dịch vụ hậu mãi và hotline 0908 123 456 của DTGear hỗ trợ cực kỳ nhiệt tình.",
            DateText = "Đã mua 3 ngày trước • Bảo hành 1 đổi 1"
        }
    };
}
