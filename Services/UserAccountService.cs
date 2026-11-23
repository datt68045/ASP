using DTGear.Models;

namespace DTGear.Services;

public interface IUserAccountService
{
    UserProfile? GetUserByEmail(string? email);
    UserProfile? Authenticate(string email, string password);
    UserProfile Register(RegisterViewModel model);
    UserProfile UpdateProfile(string currentEmail, string fullName, string phoneNumber, string defaultAddress);
    UserProfile GetDefaultDemoUser();
    bool IsAdminEmail(string? email);
}

public class UserAccountService : IUserAccountService
{
    public const string AdminEmail = "datt68045@gmail.com";
    public const string AdminPassword = "ok123";

    private readonly List<UserProfile> _users = new();
    private readonly object _lock = new();

    public UserAccountService()
    {
        SeedDefaultUsers();
    }

    public bool IsAdminEmail(string? email) =>
        !string.IsNullOrWhiteSpace(email) &&
        email.Trim().Equals(AdminEmail, StringComparison.OrdinalIgnoreCase);

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
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return null;

        var cleanEmail = email.Trim();

        lock (_lock)
        {
            // 1. Kiểm tra tài khoản Quản trị viên (Admin): datt68045@gmail.com / ok123
            if (IsAdminEmail(cleanEmail))
            {
                if (password != AdminPassword)
                {
                    return null;
                }

                var adminUser = _users.FirstOrDefault(u => IsAdminEmail(u.Email));
                if (adminUser != null)
                {
                    adminUser.Role = "Admin";
                    return adminUser;
                }
            }

            // 2. Kiểm tra tài khoản đã đăng ký
            var existing = _users.FirstOrDefault(u => u.Email.Equals(cleanEmail, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                if (!string.Equals(existing.Password, password, StringComparison.Ordinal))
                {
                    return null;
                }
                return existing;
            }

            // 3. Nếu người dùng đăng nhập bằng một email khách hàng mới -> Tạo tài khoản Customer
            var namePart = cleanEmail.Split('@')[0].Replace(".", " ").Replace("_", " ");
            var displayName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(namePart);
            var created = new UserProfile
            {
                Id = $"USR-{Random.Shared.Next(1000, 9999)}",
                FullName = string.IsNullOrWhiteSpace(displayName) ? "Khách hàng DTGear" : displayName,
                Email = cleanEmail,
                Password = password,
                PhoneNumber = "0912345678",
                DefaultAddress = "Đường Bình Nhâm 29, Lái Thiêu, Thuận An, Bình Dương",
                Role = "Customer",
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
            var cleanEmail = model.Email.Trim();
            var isAdmin = IsAdminEmail(cleanEmail);

            var existing = _users.FirstOrDefault(u => u.Email.Equals(cleanEmail, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.FullName = model.FullName.Trim();
                existing.PhoneNumber = model.PhoneNumber.Trim();
                existing.DefaultAddress = model.DefaultAddress.Trim();
                if (!isAdmin)
                {
                    existing.Password = model.Password;
                    existing.Role = "Customer";
                }
                return existing;
            }

            var newUser = new UserProfile
            {
                Id = $"USR-{Random.Shared.Next(1000, 9999)}",
                FullName = model.FullName.Trim(),
                Email = cleanEmail,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber.Trim(),
                DefaultAddress = model.DefaultAddress.Trim(),
                Role = isAdmin ? "Admin" : "Customer",
                MemberTier = isAdmin ? "Quản trị viên DTGear" : "DTGear Studio Member",
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
        // Tài khoản Quản trị viên (Admin) mặc định: datt68045@gmail.com / ok123
        _users.Add(new UserProfile
        {
            Id = "ADM-0001",
            FullName = "Quản trị viên DTGear",
            Email = AdminEmail,
            Password = AdminPassword,
            PhoneNumber = "0908 123 456",
            DefaultAddress = "Đường Bình Nhâm 29, Phường Lái Thiêu, Thành phố Thuận An, Tỉnh Bình Dương",
            Role = "Admin",
            MemberTier = "Quản trị viên Hệ thống (Admin)",
            JoinedAt = DateTime.Now.AddMonths(-12),
            SavedBuilds = GetDefaultSavedBuilds()
        });

        // Tài khoản Khách hàng (Customer) mẫu
        _users.Add(new UserProfile
        {
            Id = "USR-2048",
            FullName = "Trần Minh Quân",
            Email = "thanhtruc.work@gmail.com",
            Password = "123456",
            PhoneNumber = "0912345678",
            DefaultAddress = "Đường Bình Nhâm 29, Lái Thiêu, Thuận An, Bình Dương",
            Role = "Customer",
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
