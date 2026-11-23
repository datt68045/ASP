using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTGear.Models;

[Table("CT_HOADON")]
public class ChiTietHoaDon
{
    [Display(Name = "Mã hóa đơn")]
    public int MaHD { get; set; }

    [MaxLength(10)]
    [Display(Name = "Mã sản phẩm")]
    public string MaSP { get; set; } = string.Empty;

    [Display(Name = "Số lượng")]
    public int SoLuong { get; set; } = 1;

    [ForeignKey(nameof(MaHD))]
    public HoaDon? HoaDon { get; set; }

    [ForeignKey(nameof(MaSP))]
    public SanPham? SanPham { get; set; }
}
