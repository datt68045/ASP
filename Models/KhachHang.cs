using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTGear.Models;

[Table("KHACHHANG")]
public class KhachHang
{
    [Key]
    [MaxLength(50)]
    [Display(Name = "Mã khách hàng (SĐT)")]
    public string MaKH { get; set; } = string.Empty;

    [MaxLength(100)]
    [Display(Name = "Họ và tên đệm")]
    public string? Ho { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên khách hàng.")]
    [MaxLength(100)]
    [Display(Name = "Tên khách hàng")]
    public string Ten { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    [MaxLength(30)]
    [Display(Name = "Số điện thoại")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng.")]
    [MaxLength(300)]
    [Display(Name = "Địa chỉ")]
    public string DiaChi { get; set; } = string.Empty;

    [MaxLength(150)]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    [Display(Name = "Ngày lập")]
    public DateTime NgayLap { get; set; } = DateTime.Now;

    public ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    [NotMapped]
    public string HoTenDayDu => string.IsNullOrWhiteSpace(Ho) ? Ten : $"{Ho} {Ten}".Trim();
}
