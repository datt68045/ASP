---
name: ui-ux-agency-workflow
description: Quy trình thiết kế và xây dựng website chuyên nghiệp theo chuẩn UI/UX Studio (Sitemap -> Wireframe -> Options -> Design System -> Code ASP.NET Core). Kích hoạt khi bắt đầu dự án web mới hoặc thiết kế lại giao diện.
---

# Quy trình Thiết kế & Lập trình Web Chuẩn UI/UX Studio

Khi người dùng yêu cầu tạo một website mới, hãy dẫn dắt người dùng đi qua đúng 4 giai đoạn chuyên nghiệp sau (tuyệt đối không nhảy cóc sang viết code ngay):

## Giai đoạn 1: Khai thác thông tin & Sitemap (Kiến trúc thông tin)
1. Đặt 3-4 câu hỏi ngắn gọn để nắm rõ: Tên thương hiệu/dự án, Lĩnh vực hoạt động, Thế mạnh cốt lõi và Mục tiêu chính của web (bán hàng, giới thiệu, hay thu thập thông tin khách hàng).
2. Đề xuất **Sơ đồ trang web (Sitemap)**: Liệt kê danh sách các trang cần có (ví dụ: Trang chủ, Giới thiệu, Sản phẩm/Dịch vụ, Chi tiết, Liên hệ) và các khối nội dung (Sections) bên trong từng trang để người dùng duyệt.

## Giai đoạn 2: Đề xuất Phương án Bố cục & Moodboard (Wireframe & Options)
1. Đề xuất **Moodboard tối giản**: Bảng màu chính/phụ/nhấn, cặp Font chữ (tiêu đề & nội dung) và phong cách Icon (ưu tiên icon nét mảnh đồng bộ).
2. Đưa ra **2 đến 3 phương án bố cục (Layout Options)** cho Trang chủ để người dùng lựa chọn:
   - *Option 1 (An toàn & Trực quan):* Bố cục rõ ràng, dễ đọc, tập trung vào thông tin cốt lõi.
   - *Option 2 (Siêu tối giản & Nhiều khoảng trắng - Whitespace):* Tách biệt khối chữ và hình ảnh, tăng tối đa không gian thở, cảm giác sang trọng chuẩn Apple.
   - *Option 3 (Hiện đại & Nhịp điệu):* Bố cục đan xen đối xứng (trái-phải) tạo điểm nhấn thị giác nhưng vẫn gọn gàng.

## Giai đoạn 3: Thiết lập Design System & Component hóa (ASP.NET Core)
Sau khi người dùng chọn Option ưng ý, hãy xây dựng nền tảng code sạch trước khi nhân bản ra các trang con:
1. **Hệ thống khoảng cách & Lưới:** Tuân thủ hệ số **4px/8px** (`4, 8, 16, 24, 32, 48, 64px`) cho mọi `margin`/`padding` và hệ thống lưới 12 cột chuẩn Responsive (Desktop, Tablet, Mobile).
2. **Tái sử dụng Component:** Tách các thành phần dùng chung thành `Partial Views` hoặc `ViewComponents` trong ASP.NET Core (`_Navbar.cshtml`, `_Footer.cshtml`, `_CardItem.cshtml`, `_ButtonPrimary.cshtml`) để đảm bảo tính đồng nhất 100% giữa các trang.
3. **Tinh chỉnh UX thông minh:**
   - **Smart Header:** Thanh điều hướng tự động đổi sang nền trắng mờ (`backdrop-filter: blur(12px)`) có viền mảnh khi người dùng cuộn trang xuống, đảm bảo chữ luôn tương phản rõ nét.
   - **Hiệu ứng vừa đủ (Pragmatic Motion):** Chỉ dùng hiệu ứng chuyển động mượt, nhẹ (`transition: all 0.25s ease`), tránh làm animation quá phức tạp gây nặng trang hoặc rối mắt.

## Giai đoạn 4: Nhân bản Trang con & Hoàn thiện Luồng Trải nghiệm
1. Áp dụng ngôn ngữ thiết kế đã chốt ở Trang chủ sang toàn bộ các trang con.
2. **Luồng Form khép kín:** Mọi biểu mẫu (Form liên hệ/đặt hàng) sau khi gửi thành công bắt buộc phải chuyển hướng sang trang hoặc trạng thái **Xác nhận (Thank You / Confirmation)** rõ ràng.
3. Chạy `dotnet build`, kiểm tra độ cao các khối đồng đều, kiểm tra hiển thị trên màn hình điện thoại và bàn giao kèm hướng dẫn cập nhật vào `README.md`.
