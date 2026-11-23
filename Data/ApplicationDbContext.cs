using DTGear.Models;
using Microsoft.EntityFrameworkCore;

namespace DTGear.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<SanPham> SanPhams => Set<SanPham>();
    public DbSet<KhachHang> KhachHangs => Set<KhachHang>();
    public DbSet<HoaDon> HoaDons => Set<HoaDon>();
    public DbSet<ChiTietHoaDon> ChiTietHoaDons => Set<ChiTietHoaDon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Bảng SANPHAM
        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.ToTable("SANPHAM");
            entity.HasKey(e => e.MaSP);
            entity.Property(e => e.MaSP).HasMaxLength(10);
            entity.Property(e => e.TenSP).IsRequired().HasMaxLength(200);
            entity.Property(e => e.GiaNhap).HasColumnType("decimal(18,2)");
            entity.Property(e => e.GiaBan).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.SoLuong).HasDefaultValue(10);
            entity.Property(e => e.DonViTinh).HasMaxLength(50);
            entity.Property(e => e.KhuyenMai).HasDefaultValue(true);
        });

        // Bảng KHACHHANG
        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.ToTable("KHACHHANG");
            entity.HasKey(e => e.MaKH);
            entity.Property(e => e.MaKH).HasMaxLength(50);
            entity.Property(e => e.Ten).IsRequired();
            entity.Property(e => e.Phone).IsRequired();
            entity.Property(e => e.DiaChi).IsRequired();
        });

        // Bảng HOADON
        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.ToTable("HOADON");
            entity.HasKey(e => e.MaHD);
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
            entity.HasOne(d => d.KhachHang)
                .WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKH)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Bảng CT_HOADON (Khóa chính phức hợp MaHD + MaSP)
        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.ToTable("CT_HOADON");
            entity.HasKey(e => new { e.MaHD, e.MaSP });
            entity.Property(e => e.MaSP).HasMaxLength(10);

            entity.HasOne(d => d.HoaDon)
                .WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHD)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.SanPham)
                .WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaSP)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
