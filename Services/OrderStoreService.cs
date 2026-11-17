using System.Text;
using System.Text.Json;
using DTGear.Models;

namespace DTGear.Services;

public interface IOrderStoreService
{
    Task<ConsultationOrder> SaveOrderAsync(ConsultationOrder order);
    ConsultationOrder? GetOrderByCode(string orderCode);
    List<ConsultationOrder> GetAllOrders();
}

public class OrderStoreService : IOrderStoreService
{
    private readonly List<ConsultationOrder> _orders = new();
    private readonly object _lock = new();
    private readonly string _storageFilePath;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OrderStoreService> _logger;

    public OrderStoreService(
        IWebHostEnvironment env,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<OrderStoreService> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;

        var dataDir = Path.Combine(env.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dataDir);
        _storageFilePath = Path.Combine(dataDir, "dtgear_orders.json");

        LoadOrSeedOrders();
    }

    public async Task<ConsultationOrder> SaveOrderAsync(ConsultationOrder order)
    {
        lock (_lock)
        {
            if (string.IsNullOrWhiteSpace(order.OrderCode))
            {
                var randomSuffix = Random.Shared.Next(1000, 9999);
                order.OrderCode = $"DTG-{DateTime.Now:MMdd}-{randomSuffix}";
            }

            order.CreatedAt = DateTime.Now;
            _orders.Insert(0, order);
            PersistToDisk();
        }

        // Tùy chọn gửi về Google Sheets Webhook nếu được cấu hình trong appsettings.json
        var webhookUrl = _configuration["DTGear:GoogleSheetsWebhookUrl"];
        if (!string.IsNullOrWhiteSpace(webhookUrl))
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var payload = JsonSerializer.Serialize(new
                {
                    order.OrderCode,
                    order.CustomerName,
                    order.PhoneNumber,
                    order.Email,
                    order.UsageNeed,
                    order.EstimatedBudget,
                    order.DeliveryMethod,
                    order.ConfigurationSummary,
                    order.TotalEstimatedAmount,
                    order.CustomerNote,
                    CreatedAt = order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                });
                using var content = new StringContent(payload, Encoding.UTF8, "application/json");
                await client.PostAsync(webhookUrl, content);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Không thể gửi đồng bộ đơn hàng sang Google Sheets Webhook.");
            }
        }

        return order;
    }

    public ConsultationOrder? GetOrderByCode(string orderCode)
    {
        lock (_lock)
        {
            return _orders.FirstOrDefault(o => o.OrderCode.Equals(orderCode, StringComparison.OrdinalIgnoreCase));
        }
    }

    public List<ConsultationOrder> GetAllOrders()
    {
        lock (_lock)
        {
            return _orders.ToList();
        }
    }

    private void LoadOrSeedOrders()
    {
        lock (_lock)
        {
            if (File.Exists(_storageFilePath))
            {
                try
                {
                    var json = File.ReadAllText(_storageFilePath);
                    var loaded = JsonSerializer.Deserialize<List<ConsultationOrder>>(json);
                    if (loaded != null && loaded.Count > 0)
                    {
                        _orders.AddRange(loaded);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Không thể đọc file dữ liệu đơn hàng cũ, khởi tạo lại dữ liệu mẫu.");
                }
            }

            _orders.Add(new ConsultationOrder
            {
                OrderCode = "DTG-1117-8241",
                CustomerName = "Nguyễn Hoàng Nam",
                PhoneNumber = "0908 456 789",
                Email = "hoangnam.arch@gmail.com",
                UsageNeed = "Gaming - Giải trí",
                EstimatedBudget = "25 - 35 triệu VNĐ",
                DeliveryMethod = "Giao hàng & lắp đặt tận nơi",
                ConfigurationSummary = "[Build PC DTGear] CPU Intel Core i5-13400F + Mainboard MSI B760M Mortar WiFi DDR5 + RAM Corsair 32GB DDR5 + VGA MSI RTX 4060 Ventus 2X White + SSD Samsung 990 EVO 1TB + Nguồn Corsair CX650 + Case NZXT H5 Flow + Màn hình ASUS TUF 27\" 2K 180Hz + Phụ kiện: Tai nghe Logitech G435 Wireless",
                TotalEstimatedAmount = 30720000,
                CustomerNote = "Nhờ shop đi dây gọn tông trắng và cài sẵn Windows 11 bản quyền.",
                CreatedAt = DateTime.Now.AddHours(-3)
            });

            PersistToDisk();
        }
    }

    private void PersistToDisk()
    {
        try
        {
            var json = JsonSerializer.Serialize(_orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_storageFilePath, json);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Không thể lưu đơn hàng xuống file JSON.");
        }
    }
}
