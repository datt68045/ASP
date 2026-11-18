using System.ComponentModel.DataAnnotations;

namespace DTGear.Models;

public class UserProfile
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string DefaultAddress { get; set; } = string.Empty;
    public string MemberTier { get; set; } = "DTGear Studio Member";
    public DateTime JoinedAt { get; set; } = DateTime.Now;
    public List<SavedBuildConfig> SavedBuilds { get; set; } = new();

    public string AvatarInitials
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FullName)) return "DT";
            var parts = FullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpperInvariant();
            return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
        }
    }
}

public class SavedBuildConfig
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string PresetCode { get; set; } = "preset-gaming";
    public string SpecsSummary { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string SavedDateText { get; set; } = string.Empty;

    public string FormattedTotalPrice => string.Format("{0:N0} ₫", TotalPrice).Replace(",", ".");
}

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email.")]
    [EmailAddress(ErrorMessage = "Định dạng Email chưa hợp lệ.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = true;
    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email.")]
    [EmailAddress(ErrorMessage = "Định dạng Email chưa hợp lệ.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng mặc định.")]
    public string DefaultAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng tạo mật khẩu.")]
    [MinLength(6, ErrorMessage = "Mật khẩu cần tối thiểu 6 ký tự.")]
    public string Password { get; set; } = string.Empty;
}

public class ProfileViewModel
{
    public UserProfile User { get; set; } = new();
    public List<ConsultationOrder> OrderHistory { get; set; } = new();
}

public class ProductSpecItem
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class ProductDetailViewModel
{
    public Product Product { get; set; } = new();
    public List<ProductSpecItem> TechnicalSpecs { get; set; } = new();
    public List<Product> RelatedProducts { get; set; } = new();
}
