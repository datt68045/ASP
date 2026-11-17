---
name: visual-assets-consistency
description: Quy chuẩn lựa chọn, định dạng và đồng bộ hóa Icon, Ảnh sản phẩm và Hình ảnh minh họa theo phong cách tối giản chuẩn Apple Studio. Kích hoạt khi thêm hình ảnh, thẻ sản phẩm, banner hoặc icon vào giao diện ASP.NET Core.
---

# Quy chuẩn Đồng bộ Hình ảnh & Icon (Visual Assets Consistency)

Khi chèn bất kỳ Icon, Ảnh sản phẩm hay Hình ảnh minh họa nào vào dự án ASP.NET Core, bắt buộc tuân thủ nghiêm ngặt 3 quy tắc sau để đảm bảo giao diện đạt chuẩn thẩm mỹ Studio:

## 1. Quy chuẩn Icon (Chống trộn lẫn & đồng nhất 100%)
- **Một thư viện duy nhất:** Chỉ sử dụng duy nhất 1 bộ thư viện icon tối giản cho toàn bộ dự án (ưu tiên **Lucide Icons** qua CDN hoặc **Bootstrap Icons**). Tuyệt đối không tự vẽ SVG chắp vá hoặc dùng emoji thay thế icon chức năng.
- **Một phong cách duy nhất:** Chỉ dùng phong cách **nét mảnh (Outlined / Line icons)** với độ dày nét đồng nhất (`stroke-width: 1.5px` hoặc `1.75px`). **NGHIÊM CẤM** trộn lẫn icon nét mảnh (Outlined) với icon tô đặc (Solid/Filled) trên cùng một giao diện.
- **Kích thước & Căn chỉnh chuẩn:**
  - Icon trong nút bấm và thanh điều hướng: cố định `18px` hoặc `20px`.
  - Icon tính năng / thẻ dịch vụ: cố định `24px`.
  - Luôn căn giữa thẳng hàng với văn bản (`display: inline-flex; align-items: center; gap: 8px;`) và khóa tỷ lệ khung hình để không bao giờ bị bóp méo.

## 2. Quy chuẩn Đồng bộ Hình ảnh & Ảnh Sản phẩm
- **Đồng nhất tông màu & ánh sáng (Base Exposure & Tone):** Toàn bộ hình ảnh sử dụng trên website phải cùng một phong cách nhiếp ảnh: **sáng sủa, ánh sáng tự nhiên mềm mại (soft studio lighting), độ bão hòa màu vừa phải, nền sạch tối giản**. Không đặt ảnh tối u ám cạnh ảnh sáng chói.
- **Từ khóa tìm kiếm ảnh mẫu (Unsplash / Pexels / AI Placeholder):** Khi chèn URL ảnh mẫu hoặc tạo dữ liệu giả lập (Seed Data) cho sản phẩm/bài viết, luôn dùng từ khóa mô tả phong cách đồng nhất (ví dụ: `minimalist product photography on light gray background`, `clean modern workspace studio light`).
- **Quy chuẩn khung ảnh sản phẩm (Apple Store Style):**
  - Đặt ảnh sản phẩm bên trong khung nền xám nhạt đặc trưng (`background-color: #F5F5F7;`) với khoảng đệm (`padding: 24px`) rộng rãi xung quanh để làm nổi bật sản phẩm.

## 3. Khóa Tỷ lệ Khung hình & Chống Vỡ Layout (CSS Standard)
- **Bắt buộc khóa tỷ lệ (`aspect-ratio`):** Mọi thẻ sản phẩm hoặc danh sách bài viết nằm cạnh nhau bắt buộc phải dùng chung một tỷ lệ khung hình cố định trong CSS:
  - Ảnh sản phẩm: `aspect-ratio: 1 / 1;` (vuông) hoặc `aspect-ratio: 4 / 5;`.
  - Ảnh bài viết / Dự án: `aspect-ratio: 16 / 9;` hoặc `aspect-ratio: 4 / 3;`.
- **Chống méo ảnh & Hiệu ứng Hover:** Luôn đi kèm thuộc tính `width: 100%; height: 100%; object-fit: cover;` để ảnh tự động cắt vừa khung mà không bao giờ bị kéo dãn hay làm lệch chiều cao các thẻ bên cạnh. Khi di chuột vào thẻ, chỉ cho ảnh phóng to cực nhẹ (`transform: scale(1.03); transition: transform 0.4s ease;`) bên trong khung `overflow: hidden`.
- **Luôn có ảnh dự phòng (Fallback):** Luôn thêm thuộc tính `alt` mô tả rõ ràng và xử lý `onerror` (hoặc dùng ảnh placeholder sạch) để trang web không bao giờ hiện biểu tượng ảnh lỗi bị gãy.
