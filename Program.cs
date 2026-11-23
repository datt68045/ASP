using System.Runtime.InteropServices;
using DTGear.Data;
using DTGear.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// Cấu hình Entity Framework Core linh hoạt 2 môi trường:
// - Ưu tiên SQL Server trên Windows nếu kết nối thành công
// - Tự động chuyển sang SQLite (dtgear.db) trên Linux/Docker hoặc khi không có SQL Server
var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection")
                  ?? "Server=(local);Database=QLBH_DTGear;Trusted_Connection=True;TrustServerCertificate=True;";

bool useSqlServer = false;
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
    string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RENDER")) &&
    string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
{
    try
    {
        var masterBuilder = new SqlConnectionStringBuilder(defaultConn)
        {
            InitialCatalog = "master",
            ConnectTimeout = 2
        };
        using var conn = new SqlConnection(masterBuilder.ConnectionString);
        conn.Open();
        useSqlServer = true;
    }
    catch
    {
        useSqlServer = false;
    }
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (useSqlServer)
    {
        options.UseSqlServer(defaultConn);
    }
    else
    {
        options.UseSqlite("Data Source=dtgear.db");
    }
});

builder.Services.AddScoped<IDTGearCatalogService, DTGearCatalogService>();
builder.Services.AddScoped<IOrderStoreService, OrderStoreService>();
builder.Services.AddSingleton<IUserAccountService, UserAccountService>();

var app = builder.Build();

// Tự động tạo bảng & nạp dữ liệu mẫu (EnsureCreated + SeedData) không bao giờ lỗi sập web
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Cảnh báo khi khởi tạo Database lần đầu, hệ thống vẫn hoạt động an toàn.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
