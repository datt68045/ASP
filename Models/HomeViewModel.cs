namespace DTGear.Models;

public class HomeViewModel
{
    public List<NeedSegmentCard> NeedSegments { get; set; } = new();
    public List<Product> AllProducts { get; set; } = new();
    public List<Product> FeaturedLaptops { get; set; } = new();
    public List<Product> FeaturedPcs { get; set; } = new();
    public List<Product> FeaturedComponents { get; set; } = new();
    public List<Product> FeaturedAccessories { get; set; } = new();

    // Danh sách linh kiện & phụ kiện phục vụ công cụ Build PC cùng DTGear
    public List<Product> Cpus { get; set; } = new();
    public List<Product> Mainboards { get; set; } = new();
    public List<Product> Rams { get; set; } = new();
    public List<Product> Vgas { get; set; } = new();
    public List<Product> Ssds { get; set; } = new();
    public List<Product> Psus { get; set; } = new();
    public List<Product> Cases { get; set; } = new();
    public List<Product> Monitors { get; set; } = new();
    public List<Product> BuildAccessories { get; set; } = new();
    public List<PcPreset> Presets { get; set; } = new();

    public List<CustomerReview> Reviews { get; set; } = new();
    public ConsultationOrder OrderForm { get; set; } = new();

    // Hỗ trợ lọc từ thanh tìm kiếm hoặc danh mục
    public string? ActiveCategory { get; set; }
    public string? ActiveSubCategory { get; set; }
    public string? SearchQuery { get; set; }
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
