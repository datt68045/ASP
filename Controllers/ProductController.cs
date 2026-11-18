using Microsoft.AspNetCore.Mvc;
using DTGear.Models;
using DTGear.Services;

namespace DTGear.Controllers;

public class ProductController : Controller
{
    private readonly IDTGearCatalogService _catalogService;

    public ProductController(IDTGearCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("/Product/Details/{id}")]
    [HttpGet("/san-pham/{id}")]
    public IActionResult Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return RedirectToAction("Index", "Home");
        }

        var product = _catalogService.GetProductById(id);
        if (product == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var allProducts = _catalogService.GetAllProducts();
        var related = allProducts
            .Where(p => !p.Id.Equals(product.Id, StringComparison.OrdinalIgnoreCase) &&
                        p.SubCategory.Equals(product.SubCategory, StringComparison.OrdinalIgnoreCase))
            .Take(4)
            .ToList();

        if (related.Count < 4)
        {
            var moreFromCategory = allProducts
                .Where(p => !p.Id.Equals(product.Id, StringComparison.OrdinalIgnoreCase) &&
                            p.Category.Equals(product.Category, StringComparison.OrdinalIgnoreCase) &&
                            related.All(r => r.Id != p.Id))
                .Take(4 - related.Count)
                .ToList();
            related.AddRange(moreFromCategory);
        }

        var vm = new ProductDetailViewModel
        {
            Product = product,
            TechnicalSpecs = BuildDetailedSpecs(product),
            RelatedProducts = related
        };

        return View(vm);
    }

    private static List<ProductSpecItem> BuildDetailedSpecs(Product product)
    {
        var specs = new List<ProductSpecItem>
        {
            new() { Label = "Mã định danh (SKU)", Value = product.Id.ToUpperInvariant() },
            new() { Label = "Danh mục phần cứng", Value = $"{GetCategoryDisplayName(product.Category)} — {product.SubCategory}" },
            new() { Label = "Phân khúc khuyên dùng", Value = product.SegmentBadge }
        };

        // Phân tách chi tiết các thành phần từ ShortSpecs (ngăn cách bởi dấu •)
        var rawParts = product.ShortSpecs
            .Split('•', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (product.Category == "Laptop" || product.Category == "PC")
        {
            var labels = new[]
            {
                "Vi xử lý (CPU)",
                "Bộ nhớ RAM",
                "Lưu trữ SSD / Đồ họa",
                "Card đồ họa (GPU) / Màn hình",
                "Hiển thị / Tản nhiệt & Nguồn"
            };

            for (var i = 0; i < rawParts.Length; i++)
            {
                var label = i < labels.Length ? labels[i] : $"Thông số mở rộng #{i + 1}";
                specs.Add(new ProductSpecItem { Label = label, Value = rawParts[i] });
            }
        }
        else
        {
            for (var i = 0; i < rawParts.Length; i++)
            {
                var label = i switch
                {
                    0 => "Thông số cốt lõi",
                    1 => "Hiệu năng / Xung nhịp",
                    2 => "Chuẩn giao tiếp / Công nghệ",
                    _ => $"Thông số kỹ thuật #{i + 1}"
                };
                specs.Add(new ProductSpecItem { Label = label, Value = rawParts[i] });
            }
        }

        if (!string.IsNullOrWhiteSpace(product.Socket))
        {
            specs.Add(new ProductSpecItem { Label = "Chuẩn Socket hỗ trợ", Value = product.Socket });
        }

        if (!string.IsNullOrWhiteSpace(product.RamType))
        {
            specs.Add(new ProductSpecItem { Label = "Chuẩn bộ nhớ RAM", Value = product.RamType });
        }

        if (product.Wattage > 0)
        {
            var wattLabel = product.SubCategory == "PSU" ? "Công suất định mức (PSU)" : "Công suất tiêu thụ (TDP)";
            specs.Add(new ProductSpecItem { Label = wattLabel, Value = $"{product.Wattage} W" });
        }

        specs.Add(new ProductSpecItem
        {
            Label = "Tình trạng sản phẩm",
            Value = "Mới 100% Fullbox — Nguyên seal chính hãng tại DTGear"
        });

        specs.Add(new ProductSpecItem
        {
            Label = "Chế độ bảo hành",
            Value = "Bảo hành chính hãng 24 – 36 tháng (1 đổi 1 trong 30 ngày đầu)"
        });

        return specs;
    }

    private static string GetCategoryDisplayName(string category) => category switch
    {
        "Laptop" => "Laptop chính hãng",
        "PC" => "PC lắp sẵn DTGear Studio",
        "Component" => "Linh kiện PC",
        "Accessory" => "Phụ kiện & Gear",
        _ => category
    };
}
