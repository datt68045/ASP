# DTGear — Website Thương Mại Điện Tử Laptop, PC, Linh Kiện & Phụ Kiện (ASP.NET Core MVC)

Website thương mại điện tử **DTGear** được xây dựng bằng **ASP.NET Core 8.0 MVC** kết hợp **Bootstrap 5 tùy chỉnh tối giản** theo chuẩn thẩm mỹ **Apple Store Minimalist (Light Mode)**.

## 1. Tính năng cốt lõi
- **Thanh điều hướng thông minh (`Views/Shared/_Navbar.cshtml`):** Nền kính mờ (`backdrop-filter: blur(12px)`), phân loại 4 nhóm sản phẩm chính (*Laptop*, *PC & Build PC*, *Linh kiện PC*, *Phụ kiện & Gear*), tích hợp ô tìm kiếm tức thì, giỏ hàng và nút nổi bật **"Build PC"**.
- **Hệ sinh thái Dữ liệu mẫu thực tế (`Services/DTGearCatalogService.cs`):**
  - **Laptop:** Văn phòng - Học tập, Mỏng nhẹ cao cấp (MacBook/OLED), Gaming - Đồ họa.
  - **PC Lắp sẵn:** PC Văn phòng êm ái, PC Gaming RTX 40-Series, PC Đồ họa Workstation.
  - **Linh kiện PC (8 nhóm):** CPU, Mainboard, RAM, VGA, SSD, Nguồn (PSU), Vỏ Case, Màn hình.
  - **Phụ kiện & Gear:** Tai nghe chụp tai/nhét tai, USB 64GB/128GB, Chuột không dây/công thái học, Bàn phím cơ, Ổ cứng SSD di động, Hub chuyển đổi Type-C.
- **Công cụ "Build PC cùng DTGear" (`Views/Shared/_PcBuilderPartial.cshtml`):**
  - Gợi ý 3 cấu hình mẫu theo nhu cầu (*Văn phòng*, *Gaming*, *Đồ họa*).
  - Tự chọn 8 linh kiện PC cốt lõi + tích chọn thêm Phụ kiện mua kèm.
  - Tự động kiểm tra tương thích phần cứng (Socket CPU/Mainboard, chuẩn RAM DDR4/DDR5, công suất Nguồn PSU) và cộng tổng chi phí theo thời gian thực.
- **Luồng Đặt hàng & Nhận tư vấn khép kín (`Controllers/OrderController.cs`):**
  - Tự động điền cấu hình từ công cụ Build PC hoặc Giỏ hàng xuống Form tư vấn.
  - Chuyển hướng sang trang Xác nhận đơn hàng (`Views/Order/Confirmation.cshtml`) và lưu trữ tại `App_Data/dtgear_orders.json` (xem danh sách tại `/Order/Manage`).

## 2. Hướng dẫn chạy dự án
```bash
dotnet build
dotnet run
```
Sau đó mở trình duyệt tại địa chỉ `http://localhost:5000`.