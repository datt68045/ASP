using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTGear.Models;

[Table("SANPHAM")]
public class SanPham
{
    [Key]
    [MaxLength(10, ErrorMessage = "Mã sản phẩm tối đa 10 ký tự.")]
    [Required(ErrorMessage = "Vui lòng nhập Mã sản phẩm.")]
    [Display(Name = "Mã sản phẩm")]
    public string MaSP { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập Tên sản phẩm.")]
    [MaxLength(200, ErrorMessage = "Tên sản phẩm tối đa 200 ký tự.")]
    [Display(Name = "Tên sản phẩm")]
    public string TenSP { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Giá nhập (VNĐ)")]
    public decimal GiaNhap { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập Giá bán.")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Giá bán (VNĐ)")]
    public decimal GiaBan { get; set; }

    [Display(Name = "Số lượng tồn")]
    public int SoLuong { get; set; } = 10;

    [MaxLength(50, ErrorMessage = "Đơn vị tính tối đa 50 ký tự.")]
    [Display(Name = "Đơn vị tính")]
    public string? DonViTinh { get; set; } = "Chiếc";

    [Display(Name = "Khuyến mãi")]
    public bool KhuyenMai { get; set; } = true;

    [Display(Name = "Giới thiệu")]
    public string? GioiThieu { get; set; }

    [Display(Name = "Đặc điểm / Thông số")]
    public string? DacDiem { get; set; }

    [Display(Name = "Hình ảnh")]
    public string? HinhAnh { get; set; }

    [Display(Name = "Ghi chú / Phân loại")]
    public string? GhiChu { get; set; }

    [Display(Name = "Ngày nhập")]
    public DateTime NgayNhap { get; set; } = DateTime.Now;

    public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
}
