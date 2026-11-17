---
name: aspnet-bootstrap-minimalist
description: Xây dựng và tùy chỉnh giao diện Bootstrap 5 mặc định trong ASP.NET Core theo phong cách tối giản chuẩn Apple (Apple Minimalist), tối ưu chuyển đổi và xử lý dữ liệu Form. Kích hoạt khi thiết kế giao diện Views (.cshtml), file site.css hoặc xây dựng tính năng trang web.
---

# Quy chuẩn Thiết kế & Xây dựng Web Bootstrap 5 Tối Giản (ASP.NET Core)

Khi xây dựng giao diện trong thư mục `Views/` và `wwwroot/css/site.css`, hãy tận dụng **Bootstrap 5 có sẵn của ASP.NET** nhưng **BẮT BUỘC** ghi đè phong cách mặc định để loại bỏ vẻ cứng nhắc, đồng thời tối ưu trải nghiệm người dùng:

## 1. Hệ thống Màu sắc & Font chữ (`wwwroot/css/site.css`)
- **Nền & Chữ:** Nền trang chủ đạo `#FFFFFF` (trắng tinh), nền phụ/thẻ `#F5F5F7` (xám nhạt Apple); màu chữ chính `#1D1D1F`, chữ phụ `#86868B`.
- **Typography:** Sử dụng font chữ hệ thống sắc nét: `-apple-system, BlinkMacSystemFont, "Inter", "Segoe UI", sans-serif`.
- **Màu nhấn (Accent Color):** Ghi đè biến `--bs-primary` thành màu nhấn tinh tế (như đen tuyền `#1D1D1F` hoặc xanh Apple `#0066CC`), tuyệt đối không dùng màu xanh dương mặc định chói mắt của Bootstrap.

## 2. Tối giản hóa Component & Tối ưu Chuyển đổi (UI/UX)
- **Navbar (Thanh điều hướng):** Nền trắng mờ (`backdrop-filter: blur(12px)`), đường viền dưới cực mảnh (`border-bottom: 1px solid #E5E5EA`), không dùng nền tối `navbar-dark`.
- **Cards (Thẻ sản phẩm/dự án):** Bỏ viền mặc định (`border-0`), nền `#F5F5F7`, bo góc tinh tế (`rounded-4`), thêm hiệu ứng nổi nhẹ mượt mà khi di chuột (`transition: transform 0.3s ease, box-shadow 0.3s ease; transform: translateY(-4px);`).
- **Buttons (Nút kêu gọi hành động - CTA):** Thiết kế nổi bật nhưng sang trọng, bo góc dạng viên thuốc (`rounded-pill`) hoặc bo nhẹ (`rounded-3`), tăng khoảng đệm (`px-4 py-2`), chữ rõ ràng để kích thích người dùng hành động.
- **Khoảng trắng & Lưới Responsive:** Sử dụng hệ thống lưới `container`, `row`, `col-md-*`, `col-lg-*` cùng các class `py-5`, `my-5`, `g-4` của Bootstrap để tạo khoảng thở rộng rãi và hiển thị chuẩn trên cả điện thoại lẫn máy tính.

## 3. Lập trình Backend & Xử lý Form Linh hoạt
- Tạo đầy đủ `Models`, `Controllers` và `Views` theo chuẩn MVC.
- **Xử lý biểu mẫu (Form đăng ký / Liên hệ):**
  - Mặc định xây dựng xử lý lưu dữ liệu vào Database (Entity Framework Core SQLite/SQL Server).
  - **Tùy chọn linh hoạt:** Nếu người dùng làm Landing Page và muốn quản lý danh sách khách hàng đơn giản, hỗ trợ tích hợp gửi dữ liệu Form trực tiếp về **Google Sheets (qua Apps Script Webhook)**.

## 4. Tự rà soát & Bàn giao
1. Rà soát lại căn lề (`margin`, `padding`), độ đồng nhất của các nút bấm và giao diện trên màn hình điện thoại (Mobile view).
2. Chạy lệnh `dotnet build` trên terminal để đảm bảo dự án không có lỗi biên dịch.
3. Sẵn sàng hỗ trợ người dùng đẩy code (`git commit` & `git push`) lên GitHub để triển khai (deploy) ra đường dẫn thực tế.
