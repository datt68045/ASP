using DTGear.Models;

namespace DTGear.Services;

public interface IDTGearCatalogService
{
    List<NeedSegmentCard> GetNeedSegments();
    List<Product> GetAllProducts();
    Product? GetProductById(string id);
    List<PcPreset> GetPresets();
    List<CustomerReview> GetCustomerReviews();
    HomeViewModel BuildHomeViewModel(string? category = null, string? subCategory = null, string? q = null);
}

public class DTGearCatalogService : IDTGearCatalogService
{
    private readonly List<Product> _products;
    private readonly List<NeedSegmentCard> _needSegments;
    private readonly List<PcPreset> _presets;
    private readonly List<CustomerReview> _reviews;

    public DTGearCatalogService()
    {
        _needSegments = SeedNeedSegments();
        _products = SeedProducts();
        _presets = SeedPresets();
        _reviews = SeedReviews();
    }

    public List<NeedSegmentCard> GetNeedSegments() => _needSegments;
    public List<Product> GetAllProducts() => _products;
    public Product? GetProductById(string id) => _products.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public List<PcPreset> GetPresets() => _presets;
    public List<CustomerReview> GetCustomerReviews() => _reviews;

    public HomeViewModel BuildHomeViewModel(string? category = null, string? subCategory = null, string? q = null)
    {
        var vm = new HomeViewModel
        {
            NeedSegments = _needSegments,
            AllProducts = _products,
            FeaturedLaptops = _products.Where(p => p.Category == "Laptop").ToList(),
            FeaturedPcs = _products.Where(p => p.Category == "PC").ToList(),
            FeaturedComponents = _products.Where(p => p.Category == "Component" && p.IsFeatured).ToList(),
            FeaturedAccessories = _products.Where(p => p.Category == "Accessory").ToList(),
            Cpus = _products.Where(p => p.SubCategory == "CPU").ToList(),
            Mainboards = _products.Where(p => p.SubCategory == "Mainboard").ToList(),
            Rams = _products.Where(p => p.SubCategory == "RAM").ToList(),
            Vgas = _products.Where(p => p.SubCategory == "VGA").ToList(),
            Ssds = _products.Where(p => p.SubCategory == "SSD").ToList(),
            Psus = _products.Where(p => p.SubCategory == "PSU").ToList(),
            Cases = _products.Where(p => p.SubCategory == "Case").ToList(),
            Monitors = _products.Where(p => p.SubCategory == "Monitor").ToList(),
            BuildAccessories = _products.Where(p => p.Category == "Accessory").ToList(),
            Presets = _presets,
            Reviews = _reviews,
            ActiveCategory = category,
            ActiveSubCategory = subCategory,
            SearchQuery = q
        };

        return vm;
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

    private static List<Product> SeedProducts() => new()
    {
        // ==================== 1. LAPTOP (ĐỦ PHÂN KHÚC) ====================
        new Product
        {
            Id = "lap-vivobook-15",
            Name = "ASUS Vivobook Go 15 Ryzen 5",
            Category = "Laptop",
            SubCategory = "Laptop Văn phòng",
            SegmentBadge = "Sinh viên - Văn phòng",
            NeedGroup = "office",
            ShortSpecs = "Ryzen 5 7520U • RAM 16GB • SSD 512GB • 15.6\" FHD",
            HighlightNote = "Mỏng nhẹ 1.6kg, bản lề mở 180°, tối ưu học tập & làm việc",
            Price = 11990000,
            OriginalPrice = 13490000,
            ImageUrl = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "lap-ideapad-slim5",
            Name = "Lenovo IdeaPad Slim 5 14 OLED",
            Category = "Laptop",
            SubCategory = "Laptop Văn phòng",
            SegmentBadge = "Bán chạy Văn phòng",
            NeedGroup = "office",
            ShortSpecs = "Core i5-13420H • RAM 16GB • SSD 512GB • 14\" WUXGA OLED",
            HighlightNote = "Vỏ nhôm nguyên khối, màn OLED 100% DCI-P3 sắc nét",
            Price = 15890000,
            OriginalPrice = 17290000,
            ImageUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "lap-macbook-air-m3",
            Name = "MacBook Air 13 inch M3 2024",
            Category = "Laptop",
            SubCategory = "Laptop Mỏng nhẹ",
            SegmentBadge = "Mỏng nhẹ Cao cấp",
            NeedGroup = "office",
            ShortSpecs = "Apple M3 8-Core • RAM 16GB • SSD 256GB • Liquid Retina",
            HighlightNote = "Thời lượng pin 18 giờ, thiết kế nhôm tái chế siêu mỏng 11.3mm",
            Price = 26990000,
            OriginalPrice = 28990000,
            ImageUrl = "https://images.unsplash.com/photo-1611186871348-b1ce696e52c9?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "lap-legion-slim5",
            Name = "Lenovo Legion Slim 5 Gaming",
            Category = "Laptop",
            SubCategory = "Laptop Gaming - Đồ họa",
            SegmentBadge = "Gaming & Đồ họa",
            NeedGroup = "gaming",
            ShortSpecs = "Ryzen 7 7840HS • RTX 4060 8GB • RAM 16GB • 16\" 2K 165Hz",
            HighlightNote = "Tản nhiệt ColdFront 5.0 mát mẻ, màn hình chuẩn 100% sRGB",
            Price = 29490000,
            OriginalPrice = 32990000,
            ImageUrl = "https://images.unsplash.com/photo-1603302576837-37561b2e2302?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "lap-zephyrus-g16",
            Name = "ASUS ROG Zephyrus G16 Studio OLED",
            Category = "Laptop",
            SubCategory = "Laptop Gaming - Đồ họa",
            SegmentBadge = "Đồ họa Chuyên nghiệp",
            NeedGroup = "creator",
            ShortSpecs = "Core Ultra 9 185H • RTX 4070 8GB • RAM 32GB • 16\" 2.5K 240Hz",
            HighlightNote = "Cắt CNC nguyên khối chuẩn Studio, cân mọi tác vụ 3D & AI",
            Price = 44990000,
            OriginalPrice = 48990000,
            ImageUrl = "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "lap-dell-inspiron-14",
            Name = "Dell Inspiron 14 5440 Business",
            Category = "Laptop",
            SubCategory = "Laptop Văn phòng",
            SegmentBadge = "Bền bỉ Doanh nghiệp",
            NeedGroup = "office",
            ShortSpecs = "Core 5 120U • RAM 16GB DDR5 • SSD 512GB • 14\" FHD+",
            HighlightNote = "Bàn phím gõ êm, bảo hành tận nơi Dell ProSupport",
            Price = 16490000,
            OriginalPrice = 17990000,
            ImageUrl = "https://images.unsplash.com/photo-1588872657578-7efd1f1555ed?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },

        // ==================== 2. PC LẮP SẴN DTGEAR ====================
        new Product
        {
            Id = "pc-office-minimal",
            Name = "DTGear Office Compact i3",
            Category = "PC",
            SubCategory = "PC Văn phòng",
            SegmentBadge = "Tiết kiệm Ngân sách",
            NeedGroup = "office",
            ShortSpecs = "Core i3-12100 • RAM 16GB • SSD 500GB NVMe • Nguồn 500W",
            HighlightNote = "Khởi động Windows 5 giây, siêu êm cho kế toán & văn phòng",
            Price = 7490000,
            OriginalPrice = 8290000,
            ImageUrl = "https://images.unsplash.com/photo-1587831990711-23ca6441447b?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "pc-gaming-4060",
            Name = "DTGear Studio Gaming i5 RTX 4060",
            Category = "PC",
            SubCategory = "PC Gaming",
            SegmentBadge = "Cấu hình Quốc dân",
            NeedGroup = "gaming",
            ShortSpecs = "Core i5-13400F • B760M • RAM 16GB • RTX 4060 8GB • SSD 1TB",
            HighlightNote = "Đi dây giấu cáp gọn gàng, chiến mượt mọi tựa game Full HD / 2K",
            Price = 18990000,
            OriginalPrice = 20500000,
            ImageUrl = "https://images.unsplash.com/photo-1593640408182-31c70c8268f5?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "pc-creator-4070s",
            Name = "DTGear ProArt Workstation i7",
            Category = "PC",
            SubCategory = "PC Đồ họa",
            SegmentBadge = "Đồ họa - Kiến trúc",
            NeedGroup = "creator",
            ShortSpecs = "Core i7-14700K • Z790 • RAM 32GB DDR5 • RTX 4070 Super 12GB",
            HighlightNote = "Tối ưu Render V-Ray, Corona, Lumion, dựng phim 4K mượt mà",
            Price = 36990000,
            OriginalPrice = 39500000,
            ImageUrl = "https://images.unsplash.com/photo-1547082299-de196ea013d6?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },

        // ==================== 3. LINH KIỆN PC (8 NHÓM ĐẦY ĐỦ) ====================
        // --- CPU ---
        new Product
        {
            Id = "cpu-i3-12100f",
            Name = "CPU Intel Core i3-12100F (4 Nhân 8 Luồng)",
            Category = "Component",
            SubCategory = "CPU",
            SegmentBadge = "CPU Phổ thông",
            NeedGroup = "office",
            ShortSpecs = "Socket LGA1700 • Xung nhịp tối đa 4.3GHz • Cache 12MB",
            HighlightNote = "Lựa chọn số 1 cho cấu hình văn phòng & gaming tiết kiệm",
            Price = 2090000,
            OriginalPrice = 2390000,
            ImageUrl = "https://images.unsplash.com/photo-1591799264318-7e6ef8ddb7ea?auto=format&fit=crop&w=800&q=80",
            Socket = "LGA1700",
            RamType = "DDR4",
            Wattage = 65,
            IsFeatured = false
        },
        new Product
        {
            Id = "cpu-i5-13400f",
            Name = "CPU Intel Core i5-13400F (10 Nhân 16 Luồng)",
            Category = "Component",
            SubCategory = "CPU",
            SegmentBadge = "CPU Bán chạy",
            NeedGroup = "gaming",
            ShortSpecs = "Socket LGA1700 • 4.6GHz Turbo • Hỗ trợ DDR4/DDR5",
            HighlightNote = "Hiệu năng trên giá thành xuất sắc cho cả Gaming & Đồ họa tầm trung",
            Price = 4690000,
            OriginalPrice = 5190000,
            ImageUrl = "https://images.unsplash.com/photo-1591799264318-7e6ef8ddb7ea?auto=format&fit=crop&w=800&q=80",
            Socket = "LGA1700",
            RamType = "DDR5",
            Wattage = 115,
            IsFeatured = true
        },
        new Product
        {
            Id = "cpu-i7-14700k",
            Name = "CPU Intel Core i7-14700K (20 Nhân 28 Luồng)",
            Category = "Component",
            SubCategory = "CPU",
            SegmentBadge = "Đồ họa & Render",
            NeedGroup = "creator",
            ShortSpecs = "Socket LGA1700 • 5.6GHz Turbo • Cache 33MB",
            HighlightNote = "Sức mạnh đa nhân vượt trội cho Kiến trúc sư, Kỹ sư 3D & Editor",
            Price = 9890000,
            OriginalPrice = 10590000,
            ImageUrl = "https://images.unsplash.com/photo-1591799264318-7e6ef8ddb7ea?auto=format&fit=crop&w=800&q=80",
            Socket = "LGA1700",
            RamType = "DDR5",
            Wattage = 190,
            IsFeatured = true
        },
        new Product
        {
            Id = "cpu-r7-7800x3d",
            Name = "CPU AMD Ryzen 7 7800X3D (8 Nhân 16 Luồng 3D V-Cache)",
            Category = "Component",
            SubCategory = "CPU",
            SegmentBadge = "Vua Gaming",
            NeedGroup = "gaming",
            ShortSpecs = "Socket AM5 • 5.0GHz • 96MB L3 Cache • Chuẩn DDR5",
            HighlightNote = "Vi xử lý chơi game tốt nhất thế giới với bộ nhớ đệm 3D V-Cache",
            Price = 10490000,
            OriginalPrice = 11290000,
            ImageUrl = "https://images.unsplash.com/photo-1555617981-dac3880eac6e?auto=format&fit=crop&w=800&q=80",
            Socket = "AM5",
            RamType = "DDR5",
            Wattage = 120,
            IsFeatured = false
        },

        // --- MAINBOARD ---
        new Product
        {
            Id = "main-h610m",
            Name = "Mainboard ASUS Prime H610M-K D4",
            Category = "Component",
            SubCategory = "Mainboard",
            SegmentBadge = "Văn phòng Bền bỉ",
            NeedGroup = "office",
            ShortSpecs = "Socket LGA1700 • mATX • 2 khe RAM DDR4 • Khe M.2 PCIe 3.0",
            HighlightNote = "Bo mạch chủ chuẩn mực, tiết kiệm chi phí cho PC văn phòng",
            Price = 1790000,
            OriginalPrice = 1990000,
            ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=800&q=80",
            Socket = "LGA1700",
            RamType = "DDR4",
            Wattage = 35,
            IsFeatured = false
        },
        new Product
        {
            Id = "main-b760m-wifi",
            Name = "Mainboard MSI MAG B760M Mortar WiFi II DDR5",
            Category = "Component",
            SubCategory = "Mainboard",
            SegmentBadge = "Mainboard Quốc dân",
            NeedGroup = "gaming",
            ShortSpecs = "Socket LGA1700 • 4 khe DDR5 • WiFi 6E • Tản nhiệt VRM bạc",
            HighlightNote = "Thiết kế tản nhiệt màu bạc tinh tế, cấp nguồn 14 Phase ổn định",
            Price = 4490000,
            OriginalPrice = 4890000,
            ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=800&q=80",
            Socket = "LGA1700",
            RamType = "DDR5",
            Wattage = 45,
            IsFeatured = true
        },
        new Product
        {
            Id = "main-z790-proart",
            Name = "Mainboard ASUS ProArt Z790-Creator WiFi DDR5",
            Category = "Component",
            SubCategory = "Mainboard",
            SegmentBadge = "Chuẩn Đồ họa Studio",
            NeedGroup = "creator",
            ShortSpecs = "Socket LGA1700 • ATX • Thunderbolt 4 • LAN 10Gb & 2.5Gb",
            HighlightNote = "Tối giản sang trọng dành riêng cho kiến trúc sư và nhà sáng tạo",
            Price = 9990000,
            OriginalPrice = 10990000,
            ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=800&q=80",
            Socket = "LGA1700",
            RamType = "DDR5",
            Wattage = 55,
            IsFeatured = false
        },
        new Product
        {
            Id = "main-b650m-aorus",
            Name = "Mainboard Gigabyte B650M AORUS Elite AX DDR5",
            Category = "Component",
            SubCategory = "Mainboard",
            SegmentBadge = "Tối ưu AMD AM5",
            NeedGroup = "gaming",
            ShortSpecs = "Socket AM5 • 4 khe DDR5 • PCIe 5.0 M.2 • WiFi 6E",
            HighlightNote = "Tương thích hoàn hảo với dòng CPU AMD Ryzen 7000/9000 Series",
            Price = 4890000,
            OriginalPrice = 5390000,
            ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=800&q=80",
            Socket = "AM5",
            RamType = "DDR5",
            Wattage = 45,
            IsFeatured = false
        },

        // --- RAM ---
        new Product
        {
            Id = "ram-16gb-ddr4",
            Name = "RAM Kingston Fury Beast 16GB (2x8GB) DDR4 3200MHz",
            Category = "Component",
            SubCategory = "RAM",
            SegmentBadge = "Tiết kiệm Ổn định",
            NeedGroup = "office",
            ShortSpecs = "16GB Dual Channel • DDR4 3200MHz • Tản nhiệt nhôm đen",
            HighlightNote = "Đa nhiệm mượt mà 20+ tab trình duyệt và ứng dụng văn phòng",
            Price = 1090000,
            OriginalPrice = 1290000,
            ImageUrl = "https://images.unsplash.com/photo-1562976540-1502c2145186?auto=format&fit=crop&w=800&q=80",
            RamType = "DDR4",
            Wattage = 10,
            IsFeatured = false
        },
        new Product
        {
            Id = "ram-32gb-ddr5",
            Name = "RAM Corsair Vengeance 32GB (2x16GB) DDR5 6000MHz",
            Category = "Component",
            SubCategory = "RAM",
            SegmentBadge = "Chuẩn Mới DDR5",
            NeedGroup = "gaming",
            ShortSpecs = "32GB Dual Channel • DDR5 6000MHz CL36 • Tản nhôm tối giản",
            HighlightNote = "Không đèn LED rườm rà, băng thông cực cao cho Gaming & Đồ họa",
            Price = 2890000,
            OriginalPrice = 3190000,
            ImageUrl = "https://images.unsplash.com/photo-1562976540-1502c2145186?auto=format&fit=crop&w=800&q=80",
            RamType = "DDR5",
            Wattage = 15,
            IsFeatured = true
        },
        new Product
        {
            Id = "ram-64gb-ddr5",
            Name = "RAM G.Skill Ripjaws S5 64GB (2x32GB) DDR5 6000MHz",
            Category = "Component",
            SubCategory = "RAM",
            SegmentBadge = "Đồ họa Nặng 64GB",
            NeedGroup = "creator",
            ShortSpecs = "64GB (2x32GB) • DDR5 6000MHz • Low-Profile White/Black",
            HighlightNote = "Dung lượng lớn chuyên trị After Effects, Revit, 3DsMax và máy ảo",
            Price = 5490000,
            OriginalPrice = 5990000,
            ImageUrl = "https://images.unsplash.com/photo-1562976540-1502c2145186?auto=format&fit=crop&w=800&q=80",
            RamType = "DDR5",
            Wattage = 20,
            IsFeatured = false
        },

        // --- VGA (CARD ĐỒ HỌA) ---
        new Product
        {
            Id = "vga-gt1030-office",
            Name = "VGA ASUS GeForce GT 1030 2GB GDDR5 (Xuất hình Văn phòng)",
            Category = "Component",
            SubCategory = "VGA",
            SegmentBadge = "Văn phòng Đa màn hình",
            NeedGroup = "office",
            ShortSpecs = "2GB GDDR5 • Tản nhiệt êm ái • HDMI + DisplayPort",
            HighlightNote = "Tiết kiệm điện chỉ 30W, hỗ trợ xuất 2 màn hình 4K sắc nét",
            Price = 1890000,
            OriginalPrice = 2150000,
            ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?auto=format&fit=crop&w=800&q=80",
            Wattage = 35,
            IsFeatured = false
        },
        new Product
        {
            Id = "vga-rtx-4060",
            Name = "VGA MSI GeForce RTX 4060 Ventus 2X White 8GB OC",
            Category = "Component",
            SubCategory = "VGA",
            SegmentBadge = "VGA Bán chạy #1",
            NeedGroup = "gaming",
            ShortSpecs = "8GB GDDR6 • DLSS 3 Frame Gen • 2 Fan Torx 4.0",
            HighlightNote = "Tông trắng bạc tinh tế, mát mẻ, tiết kiệm điện chỉ 115W",
            Price = 7990000,
            OriginalPrice = 8590000,
            ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?auto=format&fit=crop&w=800&q=80",
            Wattage = 120,
            IsFeatured = true
        },
        new Product
        {
            Id = "vga-rtx-4070-super",
            Name = "VGA ASUS ProArt GeForce RTX 4070 Super 12GB GDDR6X",
            Category = "Component",
            SubCategory = "VGA",
            SegmentBadge = "Đồ họa & Gaming 2K/4K",
            NeedGroup = "creator",
            ShortSpecs = "12GB GDDR6X • Thiết kế ProArt tối giản • 3 Fan Axial-tech",
            HighlightNote = "Không đèn RGB, độ hoàn thiện cơ khí chuẩn mực, cực êm khi Render",
            Price = 16990000,
            OriginalPrice = 18290000,
            ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?auto=format&fit=crop&w=800&q=80",
            Wattage = 225,
            IsFeatured = true
        },

        // --- SSD ---
        new Product
        {
            Id = "ssd-kingston-500gb",
            Name = "SSD Kingston NV2 500GB M.2 PCIe Gen4 x4",
            Category = "Component",
            SubCategory = "SSD",
            SegmentBadge = "SSD Giá tốt",
            NeedGroup = "office",
            ShortSpecs = "500GB • Đọc 3500MB/s • Chuẩn M.2 2280 NVMe",
            HighlightNote = "Tốc độ gấp 6 lần SSD SATA truyền thống, bảo hành 36 tháng",
            Price = 990000,
            OriginalPrice = 1190000,
            ImageUrl = "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?auto=format&fit=crop&w=800&q=80",
            Wattage = 5,
            IsFeatured = false
        },
        new Product
        {
            Id = "ssd-samsung-990evo-1tb",
            Name = "SSD Samsung 990 EVO 1TB M.2 NVMe PCIe Gen4",
            Category = "Component",
            SubCategory = "SSD",
            SegmentBadge = "Bền bỉ Tốc độ cao",
            NeedGroup = "gaming",
            ShortSpecs = "1TB • Đọc 5000MB/s • Ghi 4200MB/s • Bảo hành 5 năm",
            HighlightNote = "Độ bền hàng đầu thế giới từ Samsung, load game và project tức thì",
            Price = 2290000,
            OriginalPrice = 2590000,
            ImageUrl = "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?auto=format&fit=crop&w=800&q=80",
            Wattage = 8,
            IsFeatured = true
        },
        new Product
        {
            Id = "ssd-samsung-990pro-2tb",
            Name = "SSD Samsung 990 PRO 2TB M.2 NVMe Gen4 (Đọc 7450MB/s)",
            Category = "Component",
            SubCategory = "SSD",
            SegmentBadge = "Hiệu năng Đỉnh cao",
            NeedGroup = "creator",
            ShortSpecs = "2TB • Đọc 7450MB/s • Ghi 6900MB/s • Có DRAM Cache",
            HighlightNote = "Dòng SSD đầu bảng chuyên xử lý file Video RAW 4K/8K và dữ liệu lớn",
            Price = 4490000,
            OriginalPrice = 4990000,
            ImageUrl = "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?auto=format&fit=crop&w=800&q=80",
            Wattage = 10,
            IsFeatured = false
        },

        // --- NGUỒN (PSU) ---
        new Product
        {
            Id = "psu-cm-500w",
            Name = "Nguồn Cooler Master Elite Nex N500 500W Công suất thực",
            Category = "Component",
            SubCategory = "PSU",
            SegmentBadge = "Văn phòng Ổn định",
            NeedGroup = "office",
            ShortSpecs = "500W • Active PFC • Quạt 120mm êm ái • Cáp dẹt đen",
            HighlightNote = "Cấp nguồn an toàn, ổn định cho các bộ PC văn phòng và phổ thông",
            Price = 890000,
            OriginalPrice = 990000,
            ImageUrl = "https://images.unsplash.com/photo-1587202372634-32705e3bf49c?auto=format&fit=crop&w=800&q=80",
            Wattage = 500,
            IsFeatured = false
        },
        new Product
        {
            Id = "psu-corsair-650w",
            Name = "Nguồn Corsair CX650 650W 80 Plus Bronze",
            Category = "Component",
            SubCategory = "PSU",
            SegmentBadge = "Chuẩn 80 Plus Bronze",
            NeedGroup = "gaming",
            ShortSpecs = "650W • Hiệu suất 88% • Tụ điện cao cấp • Bảo hành 5 năm",
            HighlightNote = "Dư sức cân mượt cấu hình Core i5 + RTX 4060 hoạt động 24/7",
            Price = 1490000,
            OriginalPrice = 1690000,
            ImageUrl = "https://images.unsplash.com/photo-1587202372634-32705e3bf49c?auto=format&fit=crop&w=800&q=80",
            Wattage = 650,
            IsFeatured = false
        },
        new Product
        {
            Id = "psu-corsair-750w-gold",
            Name = "Nguồn Corsair RM750e 750W 80 Plus Gold Full Modular ATX 3.0",
            Category = "Component",
            SubCategory = "PSU",
            SegmentBadge = "Full Modular ATX 3.0",
            NeedGroup = "creator",
            ShortSpecs = "750W Gold • Cáp rời 100% • Có sẵn cáp PCIe 5.0 12VHPWR",
            HighlightNote = "Chế độ Zero RPM quạt tự ngừng quay khi tải nhẹ, bảo hành 7 năm",
            Price = 2690000,
            OriginalPrice = 2990000,
            ImageUrl = "https://images.unsplash.com/photo-1587202372634-32705e3bf49c?auto=format&fit=crop&w=800&q=80",
            Wattage = 750,
            IsFeatured = false
        },

        // --- VỎ CASE ---
        new Product
        {
            Id = "case-deepcool-ch370",
            Name = "Vỏ Case DeepCool CH370 Minimalist White/Black",
            Category = "Component",
            SubCategory = "Case",
            SegmentBadge = "Tối giản Gọn gàng",
            NeedGroup = "office",
            ShortSpecs = "mATX • Mặt lưới thoáng khí • Giá đỡ tai nghe tích hợp",
            HighlightNote = "Thiết kế phẳng chuẩn tối giản, đặt trên bàn làm việc cực sang",
            Price = 1090000,
            OriginalPrice = 1250000,
            ImageUrl = "https://images.unsplash.com/photo-1587831990711-23ca6441447b?auto=format&fit=crop&w=800&q=80",
            Wattage = 0,
            IsFeatured = false
        },
        new Product
        {
            Id = "case-nzxt-h5-flow",
            Name = "Vỏ Case NZXT H5 Flow Matte White Studio Edition",
            Category = "Component",
            SubCategory = "Case",
            SegmentBadge = "Thiết kế Studio",
            NeedGroup = "gaming",
            ShortSpecs = "Mid-Tower ATX • Kính cường lực trong suốt • Rãnh đi dây thông minh",
            HighlightNote = "Biểu tượng của sự tối giản hiện đại, luồng gió làm mát tối ưu cho GPU",
            Price = 2190000,
            OriginalPrice = 2490000,
            ImageUrl = "https://images.unsplash.com/photo-1587831990711-23ca6441447b?auto=format&fit=crop&w=800&q=80",
            Wattage = 0,
            IsFeatured = true
        },
        new Product
        {
            Id = "case-fractal-north",
            Name = "Vỏ Case Fractal Design North Chalk White (Ốp Gỗ Sồi Tự Nhiên)",
            Category = "Component",
            SubCategory = "Case",
            SegmentBadge = "Tuyệt tác Kiến trúc",
            NeedGroup = "creator",
            ShortSpecs = "ATX Mid-Tower • Mặt trước gỗ sồi thật • Cổng Type-C 20Gbps",
            HighlightNote = "Hòa quyện hoàn hảo vào không gian nội thất Studio & Phòng làm việc",
            Price = 3890000,
            OriginalPrice = 4290000,
            ImageUrl = "https://images.unsplash.com/photo-1587831990711-23ca6441447b?auto=format&fit=crop&w=800&q=80",
            Wattage = 0,
            IsFeatured = false
        },

        // --- MÀN HÌNH (MONITOR) ---
        new Product
        {
            Id = "mon-lg-24-100hz",
            Name = "Màn hình LG 24MR400-B 24 inch FHD IPS 100Hz",
            Category = "Component",
            SubCategory = "Monitor",
            SegmentBadge = "Văn phòng Bảo vệ mắt",
            NeedGroup = "office",
            ShortSpecs = "24\" IPS Full HD • 100Hz mượt mà • Chống nháy & Lọc sáng xanh",
            HighlightNote = "Viền mỏng 3 cạnh, góc nhìn rộng 178°, giá cực tốt cho học tập",
            Price = 2390000,
            OriginalPrice = 2790000,
            ImageUrl = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?auto=format&fit=crop&w=800&q=80",
            Wattage = 0,
            IsFeatured = true
        },
        new Product
        {
            Id = "mon-asus-27-2k-180hz",
            Name = "Màn hình ASUS TUF VG27AQ3A 27 inch 2K Fast IPS 180Hz",
            Category = "Component",
            SubCategory = "Monitor",
            SegmentBadge = "Gaming 2K 180Hz",
            NeedGroup = "gaming",
            ShortSpecs = "27\" QHD (2560x1440) • Fast IPS 180Hz 1ms • 130% sRGB",
            HighlightNote = "Hình ảnh 2K sắc nét kết hợp tần số quét 180Hz siêu mượt",
            Price = 5890000,
            OriginalPrice = 6490000,
            ImageUrl = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?auto=format&fit=crop&w=800&q=80",
            Wattage = 0,
            IsFeatured = true
        },
        new Product
        {
            Id = "mon-proart-27-4k",
            Name = "Màn hình Đồ họa ASUS ProArt PA279CRV 27 inch 4K USB-C",
            Category = "Component",
            SubCategory = "Monitor",
            SegmentBadge = "Chuẩn màu ΔE < 2",
            NeedGroup = "creator",
            ShortSpecs = "27\" 4K UHD IPS • 99% DCI-P3 & Adobe RGB • USB-C PD 96W",
            HighlightNote = "Cân màu sẵn từ nhà máy đạt chuẩn Calman Verified cho Designer",
            Price = 11490000,
            OriginalPrice = 12590000,
            ImageUrl = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?auto=format&fit=crop&w=800&q=80",
            Wattage = 0,
            IsFeatured = true
        },

        // ==================== 4. PHỤ KIỆN & GEAR (TAI NGHE, USB, CHUỘT, PHÍM...) ====================
        new Product
        {
            Id = "acc-usb-kingston-64gb",
            Name = "USB Kingston DataTraveler Exodia M 64GB USB 3.2",
            Category = "Accessory",
            SubCategory = "USB",
            SegmentBadge = "Bán chạy Sinh viên",
            NeedGroup = "accessory",
            ShortSpecs = "Dung lượng 64GB • Chuẩn USB 3.2 Gen 1 • Nắp trượt bảo vệ",
            HighlightNote = "Sao chép tài liệu, đồ án và tạo bộ cài Windows nhanh chóng, bền bỉ",
            Price = 190000,
            OriginalPrice = 250000,
            ImageUrl = "https://images.unsplash.com/photo-1618410320928-25228d811631?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "acc-usb-sandisk-128gb",
            Name = "USB 2 Đầu SanDisk Ultra Dual Drive Luxe 128GB (Type-C & USB-A)",
            Category = "Accessory",
            SubCategory = "USB",
            SegmentBadge = "Vỏ Kim loại 2 Đầu",
            NeedGroup = "accessory",
            ShortSpecs = "128GB • Đầu cắm Type-C & USB-A • Đọc 150MB/s",
            HighlightNote = "Chuyển dữ liệu trực tiếp giữa Laptop, PC, MacBook và Điện thoại",
            Price = 390000,
            OriginalPrice = 490000,
            ImageUrl = "https://images.unsplash.com/photo-1618410320928-25228d811631?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "acc-headphone-sony-in-ear",
            Name = "Tai nghe Nhét tai Sony MDR-EX155AP Có Mic Đàm thoại",
            Category = "Accessory",
            SubCategory = "Tai nghe",
            SegmentBadge = "Họp Online & Học tập",
            NeedGroup = "accessory",
            ShortSpecs = "In-Ear nhỏ gọn • Màng loa 9mm • Tích hợp Mic đàm thoại rõ nét",
            HighlightNote = "Đeo êm tai cả ngày, học trực tuyến và nghe nhạc âm thanh trong trẻo",
            Price = 290000,
            OriginalPrice = 350000,
            ImageUrl = "https://images.unsplash.com/photo-1590658268037-6bf12165a8df?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "acc-headphone-logitech-g435",
            Name = "Tai nghe Chụp tai Không dây Logitech G435 Lightspeed White",
            Category = "Accessory",
            SubCategory = "Tai nghe",
            SegmentBadge = "Siêu nhẹ 165g",
            NeedGroup = "accessory",
            ShortSpecs = "Wireless Lightspeed + Bluetooth • Mic kép Beamforming • Pin 18h",
            HighlightNote = "Tông trắng xám tối giản, trọng lượng siêu nhẹ không gây mỏi cổ",
            Price = 1290000,
            OriginalPrice = 1590000,
            ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "acc-headphone-hyperx-cloud3",
            Name = "Tai nghe Chụp tai HyperX Cloud III Studio Gaming",
            Category = "Accessory",
            SubCategory = "Tai nghe",
            SegmentBadge = "Âm thanh Chuẩn mực",
            NeedGroup = "accessory",
            ShortSpecs = "Màng loa 53mm đặt nghiêng • Đệm tai Memory Foam • Mic lọc ồn",
            HighlightNote = "Độ chi tiết âm thanh xuất sắc cho cả chơi game FPS lẫn dựng video",
            Price = 2190000,
            OriginalPrice = 2490000,
            ImageUrl = "https://images.unsplash.com/photo-1546435770-a3e426bf472b?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "acc-mouse-logitech-m350s",
            Name = "Chuột Không dây Logitech Pebble Mouse 2 M350s Tonal White",
            Category = "Accessory",
            SubCategory = "Chuột",
            SegmentBadge = "Click Siêu êm",
            NeedGroup = "accessory",
            ShortSpecs = "Bluetooth kết nối 3 thiết bị • Giảm 90% tiếng ồn Click • Pin 24 tháng",
            HighlightNote = "Thiết kế mỏng nhẹ bỏ gọn túi xách, lý tưởng cho văn phòng & thư viện",
            Price = 550000,
            OriginalPrice = 650000,
            ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "acc-mouse-mx-master-3s",
            Name = "Chuột Công thái học Logitech MX Master 3S Pale Gray",
            Category = "Accessory",
            SubCategory = "Chuột",
            SegmentBadge = "Chuẩn Chuyên gia",
            NeedGroup = "accessory",
            ShortSpecs = "Cảm biến 8000 DPI trên kính • Con lăn từ tính MagSpeed • Quiet Click",
            HighlightNote = "Tăng tốc độ làm việc cho Lập trình viên, Kiến trúc sư & Designer",
            Price = 2390000,
            OriginalPrice = 2690000,
            ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "acc-kb-keychron-k8pro",
            Name = "Bàn phím Cơ Không dây Keychron K8 Pro Khung Nhôm",
            Category = "Accessory",
            SubCategory = "Bàn phím",
            SegmentBadge = "Bàn phím Cơ Studio",
            NeedGroup = "accessory",
            ShortSpecs = "Layout TKL 87 phím • Bluetooth 5.1 / Type-C • Keycap PBT Double-shot",
            HighlightNote = "Đã lót sẵn Foam tiêu âm êm ái, tương thích hoàn hảo cả Windows & macOS",
            Price = 2290000,
            OriginalPrice = 2590000,
            ImageUrl = "https://images.unsplash.com/photo-1587829741301-dc798b83add3?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "acc-ssd-portable-t7",
            Name = "Ổ cứng SSD Di động Samsung T7 Shield 1TB USB 3.2 (1050MB/s)",
            Category = "Accessory",
            SubCategory = "Ổ cứng di động",
            SegmentBadge = "Chống sốc IP65",
            NeedGroup = "accessory",
            ShortSpecs = "1TB • Đọc 1050MB/s • Chống nước & bụi IP65 • Kèm cáp Type-C & USB-A",
            HighlightNote = "Sao lưu dữ liệu đồ họa, ảnh chụp và dựng phim trực tiếp trên ổ di động",
            Price = 2690000,
            OriginalPrice = 2990000,
            ImageUrl = "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?auto=format&fit=crop&w=800&q=80",
            IsFeatured = true
        },
        new Product
        {
            Id = "acc-hub-ugreen-7in1",
            Name = "Hub Chuyển đổi Ugreen Type-C 7-in-1 Vỏ Nhôm (HDMI 4K, PD 100W)",
            Category = "Accessory",
            SubCategory = "Hub chuyển đổi",
            SegmentBadge = "Mở rộng Kết nối",
            NeedGroup = "accessory",
            ShortSpecs = "HDMI 4K@60Hz • 2x USB 3.0 • SD/TF Card • Sạc nhanh PD 100W",
            HighlightNote = "Phụ kiện không thể thiếu cho các dòng Laptop mỏng nhẹ đời mới",
            Price = 650000,
            OriginalPrice = 790000,
            ImageUrl = "https://images.unsplash.com/photo-1625842268584-8f3296236761?auto=format&fit=crop&w=800&q=80",
            IsFeatured = false
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
            CustomerName = "Anh Minh Quân",
            Role = "Kiến trúc sư Nội thất • Quận 3, TP.HCM",
            PurchasedItem = "Bộ PC ProArt Core i7-14700K + Màn hình ASUS ProArt 4K + Chuột MX Master 3S",
            Comment = "Mình ấn tượng nhất là đội ngũ DTGear tư vấn cực kỳ đúng nhu cầu, không vẽ vời linh kiện thừa. Thùng máy Fractal North đi dây giấu cáp gọn đẹp như phòng trưng bày Apple, màn hình được kỹ thuật viên cân màu kỹ trước khi bàn giao.",
            DateText = "Đã mua tháng trước • Bảo hành 36 tháng"
        },
        new CustomerReview
        {
            CustomerName = "Bạn Thanh Trúc",
            Role = "Sinh viên Kinh tế • Thủ Đức",
            PurchasedItem = "Laptop Lenovo IdeaPad Slim 5 OLED + USB SanDisk 128GB + Tai nghe Sony",
            Comment = "Mình tìm mua laptop học tập kèm USB và tai nghe học online. Giao diện web DTGear sáng sủa, dễ so sánh giá từng phân khúc. Đặt hàng buổi sáng thì đầu giờ chiều đã nhận được máy nguyên seal kèm đầy đủ phụ kiện.",
            DateText = "Đã mua 2 tuần trước • Giao hàng hỏa tốc"
        },
        new CustomerReview
        {
            CustomerName = "Anh Hoàng Bách",
            Role = "Software Engineer & Gamer • Quận 7, TP.HCM",
            PurchasedItem = "Build PC Core i5-13400F + RTX 4060 White + Bàn phím Keychron K8 Pro",
            Comment = "Công cụ Build PC của DTGear tự kiểm tra tương thích RAM DDR5 với Mainboard và tính sẵn công suất nguồn nên mình tự chọn cấu hình chỉ mất 3 phút. Giá linh kiện lẻ và phụ kiện đi kèm rất hợp lý, minh bạch.",
            DateText = "Đã mua tuần trước • Hỗ trợ lắp đặt tận nơi"
        }
    };
}
