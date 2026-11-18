using Microsoft.AspNetCore.Mvc;
using DTGear.Models;
using DTGear.Services;

namespace DTGear.Controllers;

public class AccountController : Controller
{
    public const string AuthEmailCookie = "DTGear_AuthEmail";
    public const string AuthNameCookie = "DTGear_AuthName";

    private readonly IUserAccountService _accountService;
    private readonly IOrderStoreService _orderStoreService;

    public AccountController(IUserAccountService accountService, IOrderStoreService orderStoreService)
    {
        _accountService = accountService;
        _orderStoreService = orderStoreService;
    }

    /// <summary>
    /// Điểm vào khi bấm mục "Người dùng" trên thanh Menu:
    /// - Chưa đăng nhập -> chuyển tới trang Login
    /// - Đã đăng nhập -> chuyển thẳng vào trang Profile
    /// </summary>
    [HttpGet("/Account")]
    [HttpGet("/Account/Index")]
    public IActionResult Index()
    {
        var email = Request.Cookies[AuthEmailCookie];
        if (!string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction(nameof(Profile));
        }
        return RedirectToAction(nameof(Login));
    }

    [HttpGet("/Account/Login")]
    public IActionResult Login(string? returnUrl = null)
    {
        var email = Request.Cookies[AuthEmailCookie];
        if (!string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction(nameof(Profile));
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost("/Account/Login")]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _accountService.Authenticate(model.Email, model.Password);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Thông tin đăng nhập chưa chính xác.");
            return View(model);
        }

        SetAuthCookies(user, model.RememberMe);
        TempData["SuccessMessage"] = $"Chào mừng {user.FullName} đã đăng nhập vào hệ thống DTGear.";

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction(nameof(Profile));
    }

    [HttpPost("/Account/QuickDemoLogin")]
    [ValidateAntiForgeryToken]
    public IActionResult QuickDemoLogin()
    {
        var demoUser = _accountService.GetDefaultDemoUser();
        SetAuthCookies(demoUser, rememberMe: true);
        TempData["SuccessMessage"] = $"Đã đăng nhập nhanh tài khoản thành viên mẫu: {demoUser.FullName}.";
        return RedirectToAction(nameof(Profile));
    }

    [HttpGet("/Account/Register")]
    public IActionResult Register()
    {
        var email = Request.Cookies[AuthEmailCookie];
        if (!string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction(nameof(Profile));
        }

        return View(new RegisterViewModel());
    }

    [HttpPost("/Account/Register")]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _accountService.Register(model);
        SetAuthCookies(user, rememberMe: true);
        TempData["SuccessMessage"] = $"Tạo tài khoản thành công! Chào mừng {user.FullName} đến với DTGear.";
        return RedirectToAction(nameof(Profile));
    }

    [HttpGet("/Account/Profile")]
    public IActionResult Profile()
    {
        var email = Request.Cookies[AuthEmailCookie];
        if (string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction(nameof(Login));
        }

        var user = _accountService.GetUserByEmail(email) ?? _accountService.GetDefaultDemoUser();
        var orders = _orderStoreService.GetAllOrders();

        var vm = new ProfileViewModel
        {
            User = user,
            OrderHistory = orders
        };

        return View(vm);
    }

    [HttpPost("/Account/UpdateProfile")]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateProfile(string fullName, string phoneNumber, string defaultAddress)
    {
        var email = Request.Cookies[AuthEmailCookie];
        if (string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction(nameof(Login));
        }

        var updatedUser = _accountService.UpdateProfile(email, fullName, phoneNumber, defaultAddress);
        SetAuthCookies(updatedUser, rememberMe: true);
        TempData["SuccessMessage"] = "Đã cập nhật và lưu thông tin hồ sơ cá nhân thành công.";
        return RedirectToAction(nameof(Profile));
    }

    [HttpPost("/Account/Logout")]
    [HttpGet("/Account/Logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(AuthEmailCookie);
        Response.Cookies.Delete(AuthNameCookie);
        TempData["SuccessMessage"] = "Bạn đã đăng xuất khỏi tài khoản DTGear.";
        return RedirectToAction(nameof(Login));
    }

    private void SetAuthCookies(UserProfile user, bool rememberMe)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = rememberMe ? DateTimeOffset.UtcNow.AddDays(14) : DateTimeOffset.UtcNow.AddHours(12)
        };

        Response.Cookies.Append(AuthEmailCookie, user.Email, options);
        Response.Cookies.Append(AuthNameCookie, user.FullName, options);
    }
}
