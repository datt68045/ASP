using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTGear.Models;

[Table("HOADON")]
public class HoaDon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Mã hóa đơn")]
    public int MaHD { get; set; }

    [Required]
    [MaxLength(50)]
    [Display(Name = "Mã khách hàng")]
    public string MaKH { get; set; } = string.Empty;

    [Display(Name = "Ngày lập")]
    public DateTime NgayLap { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Tổng tiền")]
    public decimal Total { get; set; }

    [Display(Name = "Đã thanh toán")]
    public bool ThanhToan { get; set; } = false;

    [ForeignKey(nameof(MaKH))]
    public KhachHang? KhachHang { get; set; }

    public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
}
