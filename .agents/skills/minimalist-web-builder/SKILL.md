---
name: minimalist-web-builder
description: Quy trình xây dựng website, Portfolio và Landing Page trên nền tảng ASP.NET Core với thiết kế tối giản chuẩn Apple, tối ưu chuyển đổi và code sạch. Kích hoạt khi người dùng yêu cầu tạo trang web mới, thêm trang hoặc thiết kế giao diện.
---

# Quy trình Xây dựng Web ASP.NET Core Tối Giản & Chuyên Nghiệp

Khi người dùng yêu cầu xây dựng một trang web hoặc tính năng mới, hãy thực hiện nghiêm ngặt theo 4 bước sau:

## Bước 1: Phân tích & Đề xuất Sitemap (Kiến trúc sư Web)
1. Đọc kỹ toàn bộ dữ liệu hoặc ý tưởng của người dùng.
2. Đề xuất cấu trúc trang (Sitemap) rõ ràng theo mục tiêu chuyển đổi (ví dụ: Hero Banner, Giới thiệu/Lộ trình, Danh sách Dự án/Sản phẩm nổi bật, Lý do lựa chọn, Feedback khách hàng, Form liên hệ/Đăng ký).
3. Đề xuất bảng mã màu tối giản (Minimalist Palette) gồm: Màu nền chính, Màu nền phụ, Màu chữ và Màu nút bấm (CTA).

## Bước 2: Xây dựng Giao diện Chuẩn Production (UI/UX)
1. Viết code giao diện trong các tệp `.cshtml` của ASP.NET Core sử dụng Tailwind CSS (nhúng qua CDN hoặc cấu hình sẵn).
2. Áp dụng nguyên tắc khoảng trắng rộng rãi, lưới (Grid) cân đối, hình ảnh hiển thị lớn, sắc nét.
3. Thiết kế các khối nội dung (Card dự án/sản phẩm) có hiệu ứng `hover:-translate-y-1 transition-all duration-300` mượt mà, tinh tế.
4. Thiết kế các nút Kêu gọi hành động (CTA / Mua ngay / Liên hệ) thật nổi bật nhưng sang trọng.

## Bước 3: Lập trình Backend ASP.NET Core & Xử lý Form
1. Tạo đầy đủ `Models`, `Controllers` và `Views` theo chuẩn MVC.
2. Nếu trang web có Form đăng ký/liên hệ của khách hàng:
   - Xây dựng xử lý lưu dữ liệu vào Database (Entity Framework Core SQLite) HOẶC hướng dẫn/tích hợp gửi dữ liệu trực tiếp về **Google Sheets (qua Apps Script Webhook)** nếu người dùng muốn quản lý danh sách khách hàng đơn giản.

## Bước 4: Tự rà soát (Refinement Pass) & Chuẩn bị Deploy
1. Kiểm tra lại toàn bộ căn lề (`margin`, `padding`), độ đồng nhất của các nút bấm và giao diện trên màn hình điện thoại (Mobile view).
2. Chạy lệnh `dotnet build` trên terminal để đảm bảo dự án không có bất kỳ lỗi biên dịch nào.
3. Sẵn sàng hỗ trợ người dùng đẩy code (commit & push) lên GitHub để triển khai (deploy) ra đường dẫn thực tế.
