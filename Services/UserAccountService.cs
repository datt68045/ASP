using DTGear.Models;

namespace DTGear.Services;

public interface IUserAccountService
{
    UserProfile? GetUserByEmail(string? email);
    UserProfile? Authenticate(string email, string password);
    UserProfile Register(RegisterViewModel model);
    UserProfile UpdateProfile(string currentEmail, string fullName, string phoneNumber, string defaultAddress);
    UserProfile GetDefaultDemoUser();
}

public class UserAccountService : IUserAccountService
{
    private readonly List<UserProfile> _users = new();
    private readonly object _lock = new();

    public UserAccountService()
    {
        SeedDefaultUsers();
    }

    public UserProfile GetDefaultDemoUser()
    {
        lock (_lock)
        {
            return _users[0];
        }
    }

    public UserProfile? GetUserByEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;
        lock (_lock)
        {
            return _users.FirstOrDefault(u => u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }

    public UserProfile? Authenticate(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;
        lock (_lock)
        {
            var existing = _users.FirstOrDefault(u => u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                return existing;
            }

            // Cho phép đăng nhập dùng thử mượt mà với bất kỳ email hợp lệ nào nếu người dùng nhập mới
            var namePart = email.Split('@')[0].Replace(".", " ").Replace("_", " ");
            var displayName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(namePart);
            var created = new UserProfile
            {
                Id = $"USR-{Random.Shared.Next(1000, 9999)}",
                FullName = string.IsNullOrWhiteSpace(displayName) ? "Trần Minh Quân" : displayName,
                Email = email.Trim(),
                Password = password,
                PhoneNumber = "0912345678",
                DefaultAddress = "Đường Bình Nhâm 29, Lái Thiêu, Thuận An, Bình Dương",
                MemberTier = "DTGear Studio Member",
                JoinedAt = DateTime.Now,
                SavedBuilds = GetDefaultSavedBuilds()
            };
            _users.Add(created);
            return created;
        }
    }

    public UserProfile Register(RegisterViewModel model)
    {
        lock (_lock)
        {
            var existing = _users.FirstOrDefault(u => u.Email.Equals(model.Email.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.FullName = model.FullName.Trim();
                existing.PhoneNumber = model.PhoneNumber.Trim();
                existing.DefaultAddress = model.DefaultAddress.Trim();
                return existing;
            }

            var newUser = new UserProfile
            {
                Id = $"USR-{Random.Shared.Next(1000, 9999)}",
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                Password = model.Password,
                PhoneNumber = model.PhoneNumber.Trim(),
                DefaultAddress = model.DefaultAddress.Trim(),
                MemberTier = "DTGear Studio Member",
                JoinedAt = DateTime.Now,
                SavedBuilds = GetDefaultSavedBuilds()
            };
            _users.Add(newUser);
            return newUser;
        }
    }

    public UserProfile UpdateProfile(string currentEmail, string fullName, string phoneNumber, string defaultAddress)
    {
        lock (_lock)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(currentEmail.Trim(), StringComparison.OrdinalIgnoreCase))
                       ?? _users[0];

            if (!string.IsNullOrWhiteSpace(fullName))
                user.FullName = fullName.Trim();
            if (!string.IsNullOrWhiteSpace(phoneNumber))
                user.PhoneNumber = phoneNumber.Trim();
            if (!string.IsNullOrWhiteSpace(defaultAddress))
                user.DefaultAddress = defaultAddress.Trim();

            return user;
        }
    }

    private void SeedDefaultUsers()
    {
        _users.Add(new UserProfile
        {
            Id = "USR-1024",
            FullName = "Trần Minh Quân",
            Email = "Datt68045@gmail.com",
            Password = "123456",
            PhoneNumber = "0912345678",
            DefaultAddress = "Đường Bình Nhâm 29, Phường Lái Thiêu, Thành phố Thuận An, Tỉnh Bình Dương",
            MemberTier = "DTGear Platinum Studio",
            JoinedAt = DateTime.Now.AddMonths(-8),
            SavedBuilds = GetDefaultSavedBuilds()
        });

        _users.Add(new UserProfile
        {
            Id = "USR-2048",
            FullName = "Lê Thanh Trúc",
            Email = "thanhtruc.work@gmail.com",
            Password = "123456",
            PhoneNumber = "0908123456",
            DefaultAddress = "Đường Bình Nhâm 29, Lái Thiêu, Thuận An, Bình Dương",
            MemberTier = "DTGear Studio Member",
            JoinedAt = DateTime.Now.AddMonths(-3),
            SavedBuilds = GetDefaultSavedBuilds()
        });
    }

    private static List<SavedBuildConfig> GetDefaultSavedBuilds() => new()
    {
        new SavedBuildConfig
        {
            Id = "BLD-4060-WHITE",
            Title = "DTGear Gaming Studio RTX 4060 White",
            PresetCode = "preset-gaming",
            SpecsSummary = "Intel Core i5-13400F • B760M Mortar WiFi DDR5 • RAM 32GB DDR5 • RTX 4060 8GB White • SSD 990 EVO 1TB • Màn ASUS TUF 27\" 2K 180Hz",
            TotalPrice = 29930000,
            SavedDateText = "Lưu ngày 15/11/2026"
        },
        new SavedBuildConfig
        {
            Id = "BLD-CREATOR-4070S",
            Title = "DTGear Workstation Đồ Họa 3D & AI",
            PresetCode = "preset-creator",
            SpecsSummary = "Intel Core i7-14700K (20 Nhân) • Z790 AORUS Elite AX • RAM 32GB 6000MHz • RTX 4070 Super 12GB • SSD 990 PRO 1TB • Màn ASUS ProArt 27\" 4K ΔE<2",
            TotalPrice = 54720000,
            SavedDateText = "Lưu ngày 10/11/2026"
        }
    };
}
