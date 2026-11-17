using System.ComponentModel.DataAnnotations;

namespace DTGear.Models;

public class ConsultationOrder
{
    public string OrderCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập họ và tên của bạn.")]
    [StringLength(100, ErrorMessage = "Họ tên không vượt quá 100 ký tự.")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại liên hệ.")]
    [RegularExpression(@"^[0-9+\-\s]{9,15}$", ErrorMessage = "Số điện thoại chưa đúng định dạng (9-15 chữ số).")]
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Địa chỉ Email chưa hợp lệ.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn nhu cầu sử dụng chính.")]
    public string UsageNeed { get; set; } = "Học tập - Văn phòng";

    public string EstimatedBudget { get; set; } = "15 - 25 triệu VNĐ";

    public string DeliveryMethod { get; set; } = "Giao hàng & lắp đặt tận nơi";

    [Required(ErrorMessage = "Vui lòng nhập hoặc chọn cấu hình / sản phẩm cần báo giá.")]
    public string ConfigurationSummary { get; set; } = string.Empty;

    public decimal TotalEstimatedAmount { get; set; }

    public string? CustomerNote { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string FormattedTotalAmount => TotalEstimatedAmount > 0
        ? string.Format("{0:N0} ₫", TotalEstimatedAmount).Replace(",", ".")
        : "Tư vấn theo ngân sách";
}
