---
name: aspnet-bootstrap-minimalist
description: Tùy chỉnh và sử dụng Bootstrap 5 mặc định trong ASP.NET Core theo phong cách tối giản chuẩn Apple (Apple Minimalist). Kích hoạt khi thiết kế giao diện Views (.cshtml) và file site.css.
---

# Quy chuẩn Thiết kế Bootstrap 5 Tối Giản cho ASP.NET Core

Khi xây dựng giao diện trong thư mục `Views/` và `wwwroot/css/site.css`, hãy tận dụng Bootstrap 5 có sẵn của ASP.NET nhưng **BẮT BUỘC** ghi đè phong cách mặc định để loại bỏ vẻ cứng nhắc:

1. **Ghi đè hệ thống màu & Font (`wwwroot/css/site.css`):**
   - Nền trang chủ đạo: `#FFFFFF` (trắng tinh) và nền phụ/thẻ: `#F5F5F7` (xám nhạt Apple).
   - Màu chữ chính: `#1D1D1F`, chữ phụ: `#86868B`.
   - Sử dụng font chữ hệ thống sắc nét: `-apple-system, BlinkMacSystemFont, "Inter", "Segoe UI", sans-serif`.
   - Ghi đè biến màu `--bs-primary` thành màu nhấn tinh tế (ví dụ: đen tuyền `#1D1D1F` hoặc xanh Apple `#0066CC`), tuyệt đối không dùng màu xanh dương mặc định chói mắt của Bootstrap.

2. **Tối giản hóa Component của Bootstrap:**
   - **Navbar (Thanh điều hướng):** Nền trắng mờ (`backdrop-filter: blur(12px)`), đường viền dưới cực mảnh (`border-bottom: 1px solid #E5E5EA`), không dùng nền tối `navbar-dark`.
   - **Cards (Thẻ sản phẩm/dự án):** Bỏ viền mặc định (`border-0`), nền `#F5F5F7`, bo góc tinh tế (`rounded-4`), thêm hiệu ứng nổi nhẹ khi di chuột (`transition: transform 0.25s ease`).
   - **Buttons (Nút bấm):** Bo góc dạng viên thuốc (`rounded-pill`) hoặc bo nhẹ (`rounded-3`), tăng khoảng đệm (`px-4 py-2`), chữ rõ ràng.
   - **Khoảng trắng (Spacing):** Sử dụng các class `py-5`, `my-5`, `g-4` của Bootstrap để tạo khoảng thở rộng rãi giữa các phần.

3. **Kiểm tra Responsive:**
   - Luôn dùng hệ thống lưới `container`, `row`, `col-md-*`, `col-lg-*` của Bootstrap để trang web hiển thị chuẩn trên cả điện thoại và máy tính.
