using DTGear.Models;
using Microsoft.EntityFrameworkCore;

namespace DTGear.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.SanPhams.Any())
        {
            var initialProducts = GetInitialSanPhams();
            context.SanPhams.AddRange(initialProducts);
            context.SaveChanges();
        }

        if (!context.KhachHangs.Any())
        {
            var kh1 = new KhachHang
            {
                MaKH = "0123456789",
                Ho = "Nguyễn Văn",
                Ten = "A",
                Phone = "0123456789",
                DiaChi = "Hẻm 99, Hành Tinh Xanh, Sao Thiên Vương",
                Email = "nguyenvana@dtgear.vn",
                GhiChu = "Khách hàng VIP thuộc Vũ trụ NPC • Nhờ shop đi dây gọn tông trắng",
                NgayLap = DateTime.Now.AddDays(-2)
            };

            var kh2 = new KhachHang
            {
                MaKH = "0987654321",
                Ho = "Trần Thị",
                Ten = "B",
                Phone = "0987654321",
                DiaChi = "Vành Đai Băng Số 7, Quận 3, Sao Thổ",
                Email = "npc@saohoa.space",
                GhiChu = "Giao hàng bằng tàu con thoi • Nhận giờ hành chính",
                NgayLap = DateTime.Now.AddDays(-1)
            };

            context.KhachHangs.AddRange(kh1, kh2);
            context.SaveChanges();

            var hd1 = new HoaDon
            {
                MaKH = "0123456789",
                NgayLap = DateTime.Now.AddHours(-5),
                Total = 18990000 + 2190000,
                ThanhToan = true
            };

            var hd2 = new HoaDon
            {
                MaKH = "0987654321",
                NgayLap = DateTime.Now.AddHours(-2),
                Total = 15890000 + 390000,
                ThanhToan = false
            };

            context.HoaDons.AddRange(hd1, hd2);
            context.SaveChanges();

            context.ChiTietHoaDons.AddRange(
                new ChiTietHoaDon { MaHD = hd1.MaHD, MaSP = "PC002", SoLuong = 1 },
                new ChiTietHoaDon { MaHD = hd1.MaHD, MaSP = "ACC005", SoLuong = 1 },
                new ChiTietHoaDon { MaHD = hd2.MaHD, MaSP = "LAP002", SoLuong = 1 },
                new ChiTietHoaDon { MaHD = hd2.MaHD, MaSP = "ACC002", SoLuong = 1 }
            );
            context.SaveChanges();
        }
    }

    public static List<SanPham> GetInitialSanPhams() => new()
    {
        // ==================== 1. LAPTOP ====================
        new SanPham
        {
            MaSP = "LAP001",
            TenSP = "ASUS Vivobook Go 15 Ryzen 5",
            GiaNhap = 10200000,
            GiaBan = 11990000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Mỏng nhẹ 1.6kg, bản lề mở 180°, tối ưu học tập & làm việc",
            DacDiem = "Ryzen 5 7520U • RAM 16GB • SSD 512GB • 15.6\" FHD",
            HinhAnh = "products/lap-vivobook-15.jpg",
            GhiChu = "Laptop|Laptop Văn phòng|Sinh viên - Văn phòng|office||||0|true|lap-vivobook-15",
            NgayNhap = DateTime.Now.AddDays(-12)
        },
        new SanPham
        {
            MaSP = "LAP002",
            TenSP = "Lenovo IdeaPad Slim 5 14 OLED",
            GiaNhap = 13800000,
            GiaBan = 15890000,
            SoLuong = 12,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Vỏ nhôm nguyên khối, màn OLED 100% DCI-P3 sắc nét",
            DacDiem = "Core i5-13420H • RAM 16GB • SSD 512GB • 14\" WUXGA OLED",
            HinhAnh = "products/lap-ideapad-slim5.jpg",
            GhiChu = "Laptop|Laptop Văn phòng|Bán chạy Văn phòng|office||||0|true|lap-ideapad-slim5",
            NgayNhap = DateTime.Now.AddDays(-11)
        },
        new SanPham
        {
            MaSP = "LAP003",
            TenSP = "MacBook Air 13 inch M3 2024",
            GiaNhap = 24200000,
            GiaBan = 26990000,
            SoLuong = 10,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Thời lượng pin 18 giờ, thiết kế nhôm tái chế siêu mỏng 11.3mm",
            DacDiem = "Apple M3 8-Core • RAM 16GB • SSD 256GB • Liquid Retina",
            HinhAnh = "products/lap-macbook-air-m3.jpg",
            GhiChu = "Laptop|Laptop Mỏng nhẹ|Mỏng nhẹ Cao cấp|office||||0|true|lap-macbook-air-m3",
            NgayNhap = DateTime.Now.AddDays(-10)
        },
        new SanPham
        {
            MaSP = "LAP004",
            TenSP = "Lenovo Legion Slim 5 Gaming",
            GiaNhap = 26500000,
            GiaBan = 29490000,
            SoLuong = 10,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Tản nhiệt ColdFront 5.0 mát mẻ, màn hình chuẩn 100% sRGB",
            DacDiem = "Ryzen 7 7840HS • RTX 4060 8GB • RAM 16GB • 16\" 2K 165Hz",
            HinhAnh = "products/lap-legion-slim5.jpg",
            GhiChu = "Laptop|Laptop Gaming - Đồ họa|Gaming & Đồ họa|gaming||||0|true|lap-legion-slim5",
            NgayNhap = DateTime.Now.AddDays(-9)
        },
        new SanPham
        {
            MaSP = "LAP005",
            TenSP = "ASUS ROG Zephyrus G16 Studio OLED",
            GiaNhap = 40800000,
            GiaBan = 44990000,
            SoLuong = 8,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Cắt CNC nguyên khối chuẩn Studio, cân mọi tác vụ 3D & AI",
            DacDiem = "Core Ultra 9 185H • RTX 4070 8GB • RAM 32GB • 16\" 2.5K 240Hz",
            HinhAnh = "products/lap-zephyrus-g16.jpg",
            GhiChu = "Laptop|Laptop Gaming - Đồ họa|Đồ họa Chuyên nghiệp|creator||||0|true|lap-zephyrus-g16",
            NgayNhap = DateTime.Now.AddDays(-8)
        },
        new SanPham
        {
            MaSP = "LAP006",
            TenSP = "Dell Inspiron 14 5440 Business",
            GiaNhap = 14400000,
            GiaBan = 16490000,
            SoLuong = 14,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Bàn phím gõ êm, bảo hành tận nơi Dell ProSupport",
            DacDiem = "Core 5 120U • RAM 16GB DDR5 • SSD 512GB • 14\" FHD+",
            HinhAnh = "products/lap-dell-inspiron-14.jpg",
            GhiChu = "Laptop|Laptop Văn phòng|Bền bỉ Doanh nghiệp|office||||0|true|lap-dell-inspiron-14",
            NgayNhap = DateTime.Now.AddDays(-8)
        },

        // ==================== 2. PC LẮP SẴN ====================
        new SanPham
        {
            MaSP = "PC001",
            TenSP = "DTGear Office Compact i3",
            GiaNhap = 6300000,
            GiaBan = 7490000,
            SoLuong = 10,
            DonViTinh = "Bộ",
            KhuyenMai = true,
            GioiThieu = "Khởi động Windows 5 giây, siêu êm cho kế toán & văn phòng",
            DacDiem = "Core i3-12100 • RAM 16GB • SSD 500GB NVMe • Nguồn 500W",
            HinhAnh = "products/pc-office-minimal.jpg",
            GhiChu = "PC|PC Văn phòng|Tiết kiệm Ngân sách|office||||0|true|pc-office-minimal",
            NgayNhap = DateTime.Now.AddDays(-7)
        },
        new SanPham
        {
            MaSP = "PC002",
            TenSP = "DTGear Studio Gaming i5 RTX 4060",
            GiaNhap = 16800000,
            GiaBan = 18990000,
            SoLuong = 10,
            DonViTinh = "Bộ",
            KhuyenMai = true,
            GioiThieu = "Đi dây giấu cáp gọn gàng, chiến mượt mọi tựa game Full HD / 2K",
            DacDiem = "Core i5-13400F • B760M • RAM 16GB • RTX 4060 8GB • SSD 1TB",
            HinhAnh = "products/pc-gaming-4060.jpg",
            GhiChu = "PC|PC Gaming|Cấu hình Quốc dân|gaming||||0|true|pc-gaming-4060",
            NgayNhap = DateTime.Now.AddDays(-7)
        },
        new SanPham
        {
            MaSP = "PC003",
            TenSP = "DTGear ProArt Workstation i7",
            GiaNhap = 33200000,
            GiaBan = 36990000,
            SoLuong = 10,
            DonViTinh = "Bộ",
            KhuyenMai = true,
            GioiThieu = "Tối ưu Render V-Ray, Corona, Lumion, dựng phim 4K mượt mà",
            DacDiem = "Core i7-14700K • Z790 • RAM 32GB DDR5 • RTX 4070 Super 12GB",
            HinhAnh = "products/pc-creator-4070s.jpg",
            GhiChu = "PC|PC Đồ họa|Đồ họa - Kiến trúc|creator||||0|true|pc-creator-4070s",
            NgayNhap = DateTime.Now.AddDays(-7)
        },

        // ==================== 3. LINH KIỆN PC ====================
        // CPU
        new SanPham
        {
            MaSP = "CPU001",
            TenSP = "CPU Intel Core i3-12100F (4 Nhân 8 Luồng)",
            GiaNhap = 1750000,
            GiaBan = 2090000,
            SoLuong = 20,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Lựa chọn số 1 cho cấu hình văn phòng & gaming tiết kiệm",
            DacDiem = "Socket LGA1700 • Xung nhịp tối đa 4.3GHz • Cache 12MB",
            HinhAnh = "products/cpu-i3-12100f.jpg",
            GhiChu = "Component|CPU|CPU Phổ thông|office|LGA1700|DDR4|65|false|cpu-i3-12100f",
            NgayNhap = DateTime.Now.AddDays(-6)
        },
        new SanPham
        {
            MaSP = "CPU002",
            TenSP = "CPU Intel Core i5-13400F (10 Nhân 16 Luồng)",
            GiaNhap = 4100000,
            GiaBan = 4690000,
            SoLuong = 18,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Hiệu năng trên giá thành xuất sắc cho cả Gaming & Đồ họa tầm trung",
            DacDiem = "Socket LGA1700 • 4.6GHz Turbo • Hỗ trợ DDR4/DDR5",
            HinhAnh = "products/cpu-i5-13400f.jpg",
            GhiChu = "Component|CPU|CPU Bán chạy|gaming|LGA1700|DDR5|115|true|cpu-i5-13400f",
            NgayNhap = DateTime.Now.AddDays(-6)
        },
        new SanPham
        {
            MaSP = "CPU003",
            TenSP = "CPU Intel Core i7-14700K (20 Nhân 28 Luồng)",
            GiaNhap = 8900000,
            GiaBan = 9890000,
            SoLuong = 12,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Sức mạnh đa nhân vượt trội cho Kiến trúc sư, Kỹ sư 3D & Editor",
            DacDiem = "Socket LGA1700 • 5.6GHz Turbo • Cache 33MB",
            HinhAnh = "products/cpu-i7-14700k.jpg",
            GhiChu = "Component|CPU|Đồ họa & Render|creator|LGA1700|DDR5|190|true|cpu-i7-14700k",
            NgayNhap = DateTime.Now.AddDays(-6)
        },
        new SanPham
        {
            MaSP = "CPU004",
            TenSP = "CPU AMD Ryzen 7 7800X3D (8 Nhân 16 Luồng 3D V-Cache)",
            GiaNhap = 9400000,
            GiaBan = 10490000,
            SoLuong = 10,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Vi xử lý chơi game tốt nhất thế giới với bộ nhớ đệm 3D V-Cache",
            DacDiem = "Socket AM5 • 5.0GHz • 96MB L3 Cache • Chuẩn DDR5",
            HinhAnh = "products/cpu-r7-7800x3d.jpg",
            GhiChu = "Component|CPU|Vua Gaming|gaming|AM5|DDR5|120|false|cpu-r7-7800x3d",
            NgayNhap = DateTime.Now.AddDays(-6)
        },

        // Mainboard
        new SanPham
        {
            MaSP = "MAIN01",
            TenSP = "Mainboard ASUS Prime H610M-K D4",
            GiaNhap = 1520000,
            GiaBan = 1790000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Bo mạch chủ chuẩn mực, tiết kiệm chi phí cho PC văn phòng",
            DacDiem = "Socket LGA1700 • mATX • 2 khe RAM DDR4 • Khe M.2 PCIe 3.0",
            HinhAnh = "products/main-h610m.jpg",
            GhiChu = "Component|Mainboard|Văn phòng Bền bỉ|office|LGA1700|DDR4|35|false|main-h610m",
            NgayNhap = DateTime.Now.AddDays(-5)
        },
        new SanPham
        {
            MaSP = "MAIN02",
            TenSP = "Mainboard MSI MAG B760M Mortar WiFi II DDR5",
            GiaNhap = 3950000,
            GiaBan = 4490000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Thiết kế tản nhiệt màu bạc tinh tế, cấp nguồn 14 Phase ổn định",
            DacDiem = "Socket LGA1700 • 4 khe DDR5 • WiFi 6E • Tản nhiệt VRM bạc",
            HinhAnh = "products/main-b760m-wifi.jpg",
            GhiChu = "Component|Mainboard|Mainboard Quốc dân|gaming|LGA1700|DDR5|45|true|main-b760m-wifi",
            NgayNhap = DateTime.Now.AddDays(-5)
        },
        new SanPham
        {
            MaSP = "MAIN03",
            TenSP = "Mainboard ASUS ProArt Z790-Creator WiFi DDR5",
            GiaNhap = 8900000,
            GiaBan = 9990000,
            SoLuong = 10,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Tối giản sang trọng dành riêng cho kiến trúc sư và nhà sáng tạo",
            DacDiem = "Socket LGA1700 • ATX • Thunderbolt 4 • LAN 10Gb & 2.5Gb",
            HinhAnh = "products/main-z790-proart.jpg",
            GhiChu = "Component|Mainboard|Chuẩn Đồ họa Studio|creator|LGA1700|DDR5|55|false|main-z790-proart",
            NgayNhap = DateTime.Now.AddDays(-5)
        },
        new SanPham
        {
            MaSP = "MAIN04",
            TenSP = "Mainboard Gigabyte B650M AORUS Elite AX DDR5",
            GiaNhap = 4300000,
            GiaBan = 4890000,
            SoLuong = 10,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Tương thích hoàn hảo với dòng CPU AMD Ryzen 7000/9000 Series",
            DacDiem = "Socket AM5 • 4 khe DDR5 • PCIe 5.0 M.2 • WiFi 6E",
            HinhAnh = "products/main-b650m-aorus.jpg",
            GhiChu = "Component|Mainboard|Tối ưu AMD AM5|gaming|AM5|DDR5|45|false|main-b650m-aorus",
            NgayNhap = DateTime.Now.AddDays(-5)
        },

        // RAM
        new SanPham
        {
            MaSP = "RAM001",
            TenSP = "RAM Kingston Fury Beast 16GB (2x8GB) DDR4 3200MHz",
            GiaNhap = 890000,
            GiaBan = 1090000,
            SoLuong = 25,
            DonViTinh = "Bộ",
            KhuyenMai = true,
            GioiThieu = "Đa nhiệm mượt mà 20+ tab trình duyệt và ứng dụng văn phòng",
            DacDiem = "16GB Dual Channel • DDR4 3200MHz • Tản nhiệt nhôm đen",
            HinhAnh = "products/ram-16gb-ddr4.jpg",
            GhiChu = "Component|RAM|Tiết kiệm Ổn định|office||DDR4|10|false|ram-16gb-ddr4",
            NgayNhap = DateTime.Now.AddDays(-4)
        },
        new SanPham
        {
            MaSP = "RAM002",
            TenSP = "RAM Corsair Vengeance 32GB (2x16GB) DDR5 6000MHz",
            GiaNhap = 2490000,
            GiaBan = 2890000,
            SoLuong = 20,
            DonViTinh = "Bộ",
            KhuyenMai = true,
            GioiThieu = "Không đèn LED rườm rà, băng thông cực cao cho Gaming & Đồ họa",
            DacDiem = "32GB Dual Channel • DDR5 6000MHz CL36 • Tản nhôm tối giản",
            HinhAnh = "products/ram-32gb-ddr5.jpg",
            GhiChu = "Component|RAM|Chuẩn Mới DDR5|gaming||DDR5|15|true|ram-32gb-ddr5",
            NgayNhap = DateTime.Now.AddDays(-4)
        },
        new SanPham
        {
            MaSP = "RAM003",
            TenSP = "RAM G.Skill Ripjaws S5 64GB (2x32GB) DDR5 6000MHz",
            GiaNhap = 4850000,
            GiaBan = 5490000,
            SoLuong = 12,
            DonViTinh = "Bộ",
            KhuyenMai = true,
            GioiThieu = "Dung lượng lớn chuyên trị After Effects, Revit, 3DsMax và máy ảo",
            DacDiem = "64GB (2x32GB) • DDR5 6000MHz • Low-Profile White/Black",
            HinhAnh = "products/ram-64gb-ddr5.jpg",
            GhiChu = "Component|RAM|Đồ họa Nặng 64GB|creator||DDR5|20|false|ram-64gb-ddr5",
            NgayNhap = DateTime.Now.AddDays(-4)
        },

        // VGA
        new SanPham
        {
            MaSP = "VGA001",
            TenSP = "VGA ASUS GeForce GT 1030 2GB GDDR5 (Xuất hình Văn phòng)",
            GiaNhap = 1580000,
            GiaBan = 1890000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Tiết kiệm điện chỉ 30W, hỗ trợ xuất 2 màn hình 4K sắc nét",
            DacDiem = "2GB GDDR5 • Tản nhiệt êm ái • HDMI + DisplayPort",
            HinhAnh = "products/vga-gt1030-office.jpg",
            GhiChu = "Component|VGA|Văn phòng Đa màn hình|office|||35|false|vga-gt1030-office",
            NgayNhap = DateTime.Now.AddDays(-3)
        },
        new SanPham
        {
            MaSP = "VGA002",
            TenSP = "VGA MSI GeForce RTX 4060 Ventus 2X White 8GB OC",
            GiaNhap = 7100000,
            GiaBan = 7990000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Tông trắng bạc tinh tế, mát mẻ, tiết kiệm điện chỉ 115W",
            DacDiem = "8GB GDDR6 • DLSS 3 Frame Gen • 2 Fan Torx 4.0",
            HinhAnh = "products/vga-rtx-4060.jpg",
            GhiChu = "Component|VGA|VGA Bán chạy #1|gaming|||120|true|vga-rtx-4060",
            NgayNhap = DateTime.Now.AddDays(-3)
        },
        new SanPham
        {
            MaSP = "VGA003",
            TenSP = "VGA ASUS ProArt GeForce RTX 4070 Super 12GB GDDR6X",
            GiaNhap = 15200000,
            GiaBan = 16990000,
            SoLuong = 10,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Không đèn RGB, độ hoàn thiện cơ khí chuẩn mực, cực êm khi Render",
            DacDiem = "12GB GDDR6X • Thiết kế ProArt tối giản • 3 Fan Axial-tech",
            HinhAnh = "products/vga-rtx-4070-super.jpg",
            GhiChu = "Component|VGA|Đồ họa & Gaming 2K/4K|creator|||225|true|vga-rtx-4070-super",
            NgayNhap = DateTime.Now.AddDays(-3)
        },

        // SSD
        new SanPham
        {
            MaSP = "SSD001",
            TenSP = "SSD Kingston NV2 500GB M.2 PCIe Gen4 x4",
            GiaNhap = 820000,
            GiaBan = 990000,
            SoLuong = 25,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Tốc độ gấp 6 lần SSD SATA truyền thống, bảo hành 36 tháng",
            DacDiem = "500GB • Đọc 3500MB/s • Chuẩn M.2 2280 NVMe",
            HinhAnh = "products/ssd-kingston-500gb.jpg",
            GhiChu = "Component|SSD|SSD Giá tốt|office|||5|false|ssd-kingston-500gb",
            NgayNhap = DateTime.Now.AddDays(-3)
        },
        new SanPham
        {
            MaSP = "SSD002",
            TenSP = "SSD Samsung 990 EVO 1TB M.2 NVMe PCIe Gen4",
            GiaNhap = 1950000,
            GiaBan = 2290000,
            SoLuong = 20,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Độ bền hàng đầu thế giới từ Samsung, load game và project tức thì",
            DacDiem = "1TB • Đọc 5000MB/s • Ghi 4200MB/s • Bảo hành 5 năm",
            HinhAnh = "products/ssd-samsung-990evo-1tb.jpg",
            GhiChu = "Component|SSD|Bền bỉ Tốc độ cao|gaming|||8|true|ssd-samsung-990evo-1tb",
            NgayNhap = DateTime.Now.AddDays(-3)
        },
        new SanPham
        {
            MaSP = "SSD003",
            TenSP = "SSD Samsung 990 PRO 2TB M.2 NVMe Gen4 (Đọc 7450MB/s)",
            GiaNhap = 3900000,
            GiaBan = 4490000,
            SoLuong = 12,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Dòng SSD đầu bảng chuyên xử lý file Video RAW 4K/8K và dữ liệu lớn",
            DacDiem = "2TB • Đọc 7450MB/s • Ghi 6900MB/s • Có DRAM Cache",
            HinhAnh = "products/ssd-samsung-990pro-2tb.jpg",
            GhiChu = "Component|SSD|Hiệu năng Đỉnh cao|creator|||10|false|ssd-samsung-990pro-2tb",
            NgayNhap = DateTime.Now.AddDays(-3)
        },

        // PSU
        new SanPham
        {
            MaSP = "PSU001",
            TenSP = "Nguồn Cooler Master Elite Nex N500 500W Công suất thực",
            GiaNhap = 720000,
            GiaBan = 890000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Cấp nguồn an toàn, ổn định cho các bộ PC văn phòng và phổ thông",
            DacDiem = "500W • Active PFC • Quạt 120mm êm ái • Cáp dẹt đen",
            HinhAnh = "products/psu-cm-500w.jpg",
            GhiChu = "Component|PSU|Văn phòng Ổn định|office|||500|false|psu-cm-500w",
            NgayNhap = DateTime.Now.AddDays(-2)
        },
        new SanPham
        {
            MaSP = "PSU002",
            TenSP = "Nguồn Corsair CX650 650W 80 Plus Bronze",
            GiaNhap = 1250000,
            GiaBan = 1490000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Dư sức cân mượt cấu hình Core i5 + RTX 4060 hoạt động 24/7",
            DacDiem = "650W • Hiệu suất 88% • Tụ điện cao cấp • Bảo hành 5 năm",
            HinhAnh = "products/psu-corsair-650w.jpg",
            GhiChu = "Component|PSU|Chuẩn 80 Plus Bronze|gaming|||650|false|psu-corsair-650w",
            NgayNhap = DateTime.Now.AddDays(-2)
        },
        new SanPham
        {
            MaSP = "PSU003",
            TenSP = "Nguồn Corsair RM750e 750W 80 Plus Gold Full Modular ATX 3.0",
            GiaNhap = 2350000,
            GiaBan = 2690000,
            SoLuong = 10,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Chế độ Zero RPM quạt tự ngừng quay khi tải nhẹ, bảo hành 7 năm",
            DacDiem = "750W Gold • Cáp rời 100% • Có sẵn cáp PCIe 5.0 12VHPWR",
            HinhAnh = "products/psu-corsair-750w-gold.jpg",
            GhiChu = "Component|PSU|Full Modular ATX 3.0|creator|||750|false|psu-corsair-750w-gold",
            NgayNhap = DateTime.Now.AddDays(-2)
        },

        // Case
        new SanPham
        {
            MaSP = "CASE01",
            TenSP = "Vỏ Case DeepCool CH370 Minimalist White/Black",
            GiaNhap = 890000,
            GiaBan = 1090000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Thiết kế phẳng chuẩn tối giản, đặt trên bàn làm việc cực sang",
            DacDiem = "mATX • Mặt lưới thoáng khí • Giá đỡ tai nghe tích hợp",
            HinhAnh = "products/case-deepcool-ch370.jpg",
            GhiChu = "Component|Case|Tối giản Gọn gàng|office|||0|false|case-deepcool-ch370",
            NgayNhap = DateTime.Now.AddDays(-2)
        },
        new SanPham
        {
            MaSP = "CASE02",
            TenSP = "Vỏ Case NZXT H5 Flow Matte White Studio Edition",
            GiaNhap = 1850000,
            GiaBan = 2190000,
            SoLuong = 12,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Biểu tượng của sự tối giản hiện đại, luồng gió làm mát tối ưu cho GPU",
            DacDiem = "Mid-Tower ATX • Kính cường lực trong suốt • Rãnh đi dây thông minh",
            HinhAnh = "products/case-nzxt-h5-flow.jpg",
            GhiChu = "Component|Case|Thiết kế Studio|gaming|||0|true|case-nzxt-h5-flow",
            NgayNhap = DateTime.Now.AddDays(-2)
        },
        new SanPham
        {
            MaSP = "CASE03",
            TenSP = "Vỏ Case Fractal Design North Chalk White (Ốp Gỗ Sồi Tự Nhiên)",
            GiaNhap = 3350000,
            GiaBan = 3890000,
            SoLuong = 8,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Hòa quyện hoàn hảo vào không gian nội thất Studio & Phòng làm việc",
            DacDiem = "ATX Mid-Tower • Mặt trước gỗ sồi thật • Cổng Type-C 20Gbps",
            HinhAnh = "products/case-fractal-north.jpg",
            GhiChu = "Component|Case|Tuyệt tác Kiến trúc|creator|||0|false|case-fractal-north",
            NgayNhap = DateTime.Now.AddDays(-2)
        },

        // Monitor
        new SanPham
        {
            MaSP = "MON001",
            TenSP = "Màn hình LG 24MR400-B 24 inch FHD IPS 100Hz",
            GiaNhap = 1990000,
            GiaBan = 2390000,
            SoLuong = 16,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Viền mỏng 3 cạnh, góc nhìn rộng 178°, giá cực tốt cho học tập",
            DacDiem = "24\" IPS Full HD • 100Hz mượt mà • Chống nháy & Lọc sáng xanh",
            HinhAnh = "products/mon-lg-24-100hz.jpg",
            GhiChu = "Component|Monitor|Văn phòng Bảo vệ mắt|office|||0|true|mon-lg-24-100hz",
            NgayNhap = DateTime.Now.AddDays(-2)
        },
        new SanPham
        {
            MaSP = "MON002",
            TenSP = "Màn hình ASUS TUF VG27AQ3A 27 inch 2K Fast IPS 180Hz",
            GiaNhap = 5100000,
            GiaBan = 5890000,
            SoLuong = 14,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Hình ảnh 2K sắc nét kết hợp tần số quét 180Hz siêu mượt",
            DacDiem = "27\" QHD (2560x1440) • Fast IPS 180Hz 1ms • 130% sRGB",
            HinhAnh = "products/mon-asus-27-2k-180hz.jpg",
            GhiChu = "Component|Monitor|Gaming 2K 180Hz|gaming|||0|true|mon-asus-27-2k-180hz",
            NgayNhap = DateTime.Now.AddDays(-2)
        },
        new SanPham
        {
            MaSP = "MON003",
            TenSP = "Màn hình Đồ họa ASUS ProArt PA279CRV 27 inch 4K USB-C",
            GiaNhap = 9950000,
            GiaBan = 11490000,
            SoLuong = 10,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Cân màu sẵn từ nhà máy đạt chuẩn Calman Verified cho Designer",
            DacDiem = "27\" 4K UHD IPS • 99% DCI-P3 & Adobe RGB • USB-C PD 96W",
            HinhAnh = "products/mon-proart-27-4k.jpg",
            GhiChu = "Component|Monitor|Chuẩn màu ΔE < 2|creator|||0|true|mon-proart-27-4k",
            NgayNhap = DateTime.Now.AddDays(-2)
        },

        // ==================== 4. PHỤ KIỆN & GEAR ====================
        new SanPham
        {
            MaSP = "ACC001",
            TenSP = "USB Kingston DataTraveler Exodia M 64GB USB 3.2",
            GiaNhap = 135000,
            GiaBan = 190000,
            SoLuong = 40,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Sao chép tài liệu, đồ án và tạo bộ cài Windows nhanh chóng, bền bỉ",
            DacDiem = "Dung lượng 64GB • Chuẩn USB 3.2 Gen 1 • Nắp trượt bảo vệ",
            HinhAnh = "products/acc-usb-kingston-64gb.jpg",
            GhiChu = "Accessory|USB|Bán chạy Sinh viên|accessory|||0|true|acc-usb-kingston-64gb",
            NgayNhap = DateTime.Now.AddDays(-1)
        },
        new SanPham
        {
            MaSP = "ACC002",
            TenSP = "USB 2 Đầu SanDisk Ultra Dual Drive Luxe 128GB (Type-C & USB-A)",
            GiaNhap = 290000,
            GiaBan = 390000,
            SoLuong = 35,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Chuyển dữ liệu trực tiếp giữa Laptop, PC, MacBook và Điện thoại",
            DacDiem = "128GB • Đầu cắm Type-C & USB-A • Đọc 150MB/s",
            HinhAnh = "products/acc-usb-sandisk-128gb.jpg",
            GhiChu = "Accessory|USB|Vỏ Kim loại 2 Đầu|accessory|||0|true|acc-usb-sandisk-128gb",
            NgayNhap = DateTime.Now.AddDays(-1)
        },
        new SanPham
        {
            MaSP = "ACC003",
            TenSP = "Tai nghe Nhét tai Sony MDR-EX155AP Có Mic Đàm thoại",
            GiaNhap = 210000,
            GiaBan = 290000,
            SoLuong = 30,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Đeo êm tai cả ngày, học trực tuyến và nghe nhạc âm thanh trong trẻo",
            DacDiem = "In-Ear nhỏ gọn • Màng loa 9mm • Tích hợp Mic đàm thoại rõ nét",
            HinhAnh = "products/acc-headphone-sony-in-ear.jpg",
            GhiChu = "Accessory|Tai nghe|Họp Online & Học tập|accessory|||0|true|acc-headphone-sony-in-ear",
            NgayNhap = DateTime.Now.AddDays(-1)
        },
        new SanPham
        {
            MaSP = "ACC004",
            TenSP = "Tai nghe Chụp tai Không dây Logitech G435 Lightspeed White",
            GiaNhap = 1050000,
            GiaBan = 1290000,
            SoLuong = 20,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Tông trắng xám tối giản, trọng lượng siêu nhẹ không gây mỏi cổ",
            DacDiem = "Wireless Lightspeed + Bluetooth • Mic kép Beamforming • Pin 18h",
            HinhAnh = "products/acc-headphone-logitech-g435.jpg",
            GhiChu = "Accessory|Tai nghe|Siêu nhẹ 165g|accessory|||0|true|acc-headphone-logitech-g435",
            NgayNhap = DateTime.Now.AddDays(-1)
        },
        new SanPham
        {
            MaSP = "ACC005",
            TenSP = "Tai nghe Chụp tai HyperX Cloud III Studio Gaming",
            GiaNhap = 1790000,
            GiaBan = 2190000,
            SoLuong = 18,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Độ chi tiết âm thanh xuất sắc cho cả chơi game FPS lẫn dựng video",
            DacDiem = "Màng loa 53mm đặt nghiêng • Đệm tai Memory Foam • Mic lọc ồn",
            HinhAnh = "products/acc-headphone-hyperx-cloud3.jpg",
            GhiChu = "Accessory|Tai nghe|Âm thanh Chuẩn mực|accessory|||0|true|acc-headphone-hyperx-cloud3",
            NgayNhap = DateTime.Now.AddDays(-1)
        },
        new SanPham
        {
            MaSP = "ACC006",
            TenSP = "Chuột Không dây Logitech Pebble Mouse 2 M350s Tonal White",
            GiaNhap = 420000,
            GiaBan = 550000,
            SoLuong = 25,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Thiết kế mỏng nhẹ bỏ gọn túi xách, lý tưởng cho văn phòng & thư viện",
            DacDiem = "Bluetooth kết nối 3 thiết bị • Giảm 90% tiếng ồn Click • Pin 24 tháng",
            HinhAnh = "products/acc-mouse-logitech-m350s.jpg",
            GhiChu = "Accessory|Chuột|Click Siêu êm|accessory|||0|true|acc-mouse-logitech-m350s",
            NgayNhap = DateTime.Now.AddDays(-1)
        },
        new SanPham
        {
            MaSP = "ACC007",
            TenSP = "Chuột Công thái học Logitech MX Master 3S Pale Gray",
            GiaNhap = 1980000,
            GiaBan = 2390000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Tăng tốc độ làm việc cho Lập trình viên, Kiến trúc sư & Designer",
            DacDiem = "Cảm biến 8000 DPI trên kính • Con lăn từ tính MagSpeed • Quiet Click",
            HinhAnh = "products/acc-mouse-mx-master-3s.jpg",
            GhiChu = "Accessory|Chuột|Chuẩn Chuyên gia|accessory|||0|true|acc-mouse-mx-master-3s",
            NgayNhap = DateTime.Now.AddDays(-1)
        },
        new SanPham
        {
            MaSP = "ACC008",
            TenSP = "Bàn phím Cơ Không dây Keychron K8 Pro Khung Nhôm",
            GiaNhap = 1890000,
            GiaBan = 2290000,
            SoLuong = 15,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Đã lót sẵn Foam tiêu âm êm ái, tương thích hoàn hảo cả Windows & macOS",
            DacDiem = "Layout TKL 87 phím • Bluetooth 5.1 / Type-C • Keycap PBT Double-shot",
            HinhAnh = "products/acc-kb-keychron-k8pro.jpg",
            GhiChu = "Accessory|Bàn phím|Bàn phím Cơ Studio|accessory|||0|true|acc-kb-keychron-k8pro",
            NgayNhap = DateTime.Now.AddDays(-1)
        },
        new SanPham
        {
            MaSP = "ACC009",
            TenSP = "Ổ cứng SSD Di động Samsung T7 Shield 1TB USB 3.2 (1050MB/s)",
            GiaNhap = 2250000,
            GiaBan = 2690000,
            SoLuong = 14,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Sao lưu dữ liệu đồ họa, ảnh chụp và dựng phim trực tiếp trên ổ di động",
            DacDiem = "1TB • Đọc 1050MB/s • Chống nước & bụi IP65 • Kèm cáp Type-C & USB-A",
            HinhAnh = "products/acc-ssd-portable-t7.jpg",
            GhiChu = "Accessory|Ổ cứng di động|Chống sốc IP65|accessory|||0|true|acc-ssd-portable-t7",
            NgayNhap = DateTime.Now.AddDays(-1)
        },
        new SanPham
        {
            MaSP = "ACC010",
            TenSP = "Hub Chuyển đổi Ugreen Type-C 7-in-1 Vỏ Nhôm (HDMI 4K, PD 100W)",
            GiaNhap = 490000,
            GiaBan = 650000,
            SoLuong = 20,
            DonViTinh = "Chiếc",
            KhuyenMai = true,
            GioiThieu = "Phụ kiện không thể thiếu cho các dòng Laptop mỏng nhẹ đời mới",
            DacDiem = "HDMI 4K@60Hz • 2x USB 3.0 • SD/TF Card • Sạc nhanh PD 100W",
            HinhAnh = "products/acc-hub-ugreen-7in1.jpg",
            GhiChu = "Accessory|Hub chuyển đổi|Mở rộng Kết nối|accessory|||0|false|acc-hub-ugreen-7in1",
            NgayNhap = DateTime.Now.AddDays(-1)
        }
    };
}
