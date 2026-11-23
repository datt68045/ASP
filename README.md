# DTGear — Website Thương Mại Điện Tử Laptop, PC, Linh Kiện & Phụ Kiện (ASP.NET Core MVC & EF Core)

Website thương mại điện tử **DTGear** được xây dựng bằng **ASP.NET Core 8.0 MVC** kết hợp **Entity Framework Core** (hỗ trợ song song **SQL Server** và **SQLite**) và giao diện **Bootstrap 5 tùy chỉnh tối giản** theo chuẩn thẩm mỹ **Apple Store Minimalist (Light Mode)**.

## 1. Kiến trúc Cơ sở dữ liệu & Entity Models (`Models/` & `Data/ApplicationDbContext.cs`)
- **Tự động thích ứng môi trường (Hybrid SQL Server / SQLite):**
  - Trên Windows / Visual Studio có SQL Server: Sử dụng chuỗi kết nối `DefaultConnection` (`Server=(local);Database=QLBH_DTGear;Trusted_Connection=True;TrustServerCertificate=True;`).
  - Trên Linux / Docker (Render.com) hoặc máy chưa bật SQL Server: Tự động chuyển sang **SQLite** (`Data Source=dtgear.db`) và gọi `context.Database.EnsureCreated()` cùng `SeedData.Initialize(context)` khi khởi động.
- **4 Bảng Thực thể Chuẩn Nghiệp Vụ Quản Lý Bán Hàng:**
  1. **`SANPHAM` (`Models/SanPham.cs`):** `MaSP` (`[Key]`, `MaxLength(10)`), `TenSP` (`[Required]`, `MaxLength(200)`), `GiaNhap`, `GiaBan`, `SoLuong`, `DonViTinh`, `KhuyenMai`, `GioiThieu`, `DacDiem`, `HinhAnh`, `GhiChu`, `NgayNhap`.
  2. **`KHACHHANG` (`Models/KhachHang.cs`):** `MaKH` (`[Key]`, lấy theo SĐT), `Ho`, `Ten`, `Phone`, `DiaChi`, `Email`, `GhiChu`, `NgayLap`.
  3. **`HOADON` (`Models/HoaDon.cs`):** `MaHD` (`[Key]`, `int` tự tăng), `MaKH` (Khóa ngoại), `NgayLap`, `Total`, `ThanhToan`.
  4. **`CT_HOADON` (`Models/ChiTietHoaDon.cs`):** Khóa chính phức hợp `(MaHD, MaSP)`, `SoLuong`.

## 2. Phân hệ Quản trị Sản phẩm & Đơn hàng (`/Admin/Products` & `/Admin/Orders`)
- **Quản lý Sản phẩm (`Controllers/AdminProductsController.cs`):**
  - Danh sách sản phẩm (`/Admin/Products`): Hiển thị bảng Bootstrap 5 gồm STT, Hình ảnh (`50x50px`), Mã SP, Tên sản phẩm, Giá nhập, Giá bán, Số lượng, ĐVT, Khuyến mãi, Ngày nhập, Tìm kiếm nhanh và thao tác Sửa / Xóa.
  - Thêm mới (`/Admin/Products/Create`): Kiểm tra trùng `MaSP` (*"Mã sản phẩm đã tồn tại"*), hỗ trợ **Upload hình ảnh thật (`IFormFile`)** lưu trực tiếp vào `wwwroot/images/`.
  - Cập nhật (`/Admin/Products/Edit/{id}`): Khóa ô `MaSP` (`readonly`), giữ nguyên ảnh cũ nếu không chọn ảnh mới.
  - Xóa (`/Admin/Products/Delete/{id}`): Hộp thoại xác nhận `confirm` trước khi xóa khỏi Database.
- **Quản lý Đơn hàng (`Controllers/AdminOrdersController.cs`):**
  - Xem danh sách hóa đơn (`HOADON`), thông tin khách hàng (`KHACHHANG`), bảng chi tiết món đã đặt (`CT_HOADON`) và nút đánh dấu **Đã thanh toán**.

## 3. Hướng dẫn chạy dự án
```bash
dotnet build
dotnet run
```
Mở trình duyệt tại địa chỉ `http://localhost:5000` (hoặc truy cập trực tiếp `/Admin/Products` và `/Admin/Orders`).
