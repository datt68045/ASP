namespace DTGear.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Nhóm chính: Laptop, PC, Component, Accessory
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Phân loại chi tiết: CPU, Mainboard, RAM, VGA, SSD, PSU, Case, Monitor, Tai nghe, USB, Chuột, Bàn phím, Ổ cứng di động...
    /// </summary>
    public string SubCategory { get; set; } = string.Empty;

    /// <summary>
    /// Nhãn phân khúc hiển thị trên góc thẻ sản phẩm
    /// </summary>
    public string SegmentBadge { get; set; } = string.Empty;

    /// <summary>
    /// Nhóm nhu cầu phục vụ lọc nhanh: office, gaming, creator, accessory
    /// </summary>
    public string NeedGroup { get; set; } = string.Empty;

    public string ShortSpecs { get; set; } = string.Empty;
    public string HighlightNote { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    // Thông số kỹ thuật phục vụ kiểm tra tương thích Build PC
    public string? Socket { get; set; }
    public string? RamType { get; set; }
    public int Wattage { get; set; }
    public bool IsFeatured { get; set; } = true;

    public string FormattedPrice => string.Format("{0:N0} ₫", Price).Replace(",", ".");
    public string? FormattedOriginalPrice => OriginalPrice.HasValue
        ? string.Format("{0:N0} ₫", OriginalPrice.Value).Replace(",", ".")
        : null;
}

public class PcPreset
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Badge { get; set; } = string.Empty;
    public string CpuId { get; set; } = string.Empty;
    public string MainboardId { get; set; } = string.Empty;
    public string RamId { get; set; } = string.Empty;
    public string VgaId { get; set; } = string.Empty;
    public string SsdId { get; set; } = string.Empty;
    public string PsuId { get; set; } = string.Empty;
    public string CaseId { get; set; } = string.Empty;
    public string MonitorId { get; set; } = string.Empty;
    public List<string> AccessoryIds { get; set; } = new();
}

public class NeedSegmentCard
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string BudgetRange { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconName { get; set; } = string.Empty;
    public string HighlightItems { get; set; } = string.Empty;
}

public class CustomerReview
{
    public string CustomerName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string PurchasedItem { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string DateText { get; set; } = string.Empty;
}
