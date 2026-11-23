using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DTGear.Controllers;

/// <summary>
/// Bộ lọc bảo vệ toàn bộ các Controller/Action Quản trị:
/// Chỉ cho phép tài khoản Admin (datt68045@gmail.com) đã đăng nhập truy cập.
/// Nếu chưa đăng nhập hoặc là tài khoản Customer, tự động chuyển hướng về trang Login kèm thông báo.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!AccountController.IsCurrentUserAdmin(context.HttpContext))
        {
            var tempDataFactory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();
            if (tempDataFactory != null)
            {
                var tempData = tempDataFactory.GetTempData(context.HttpContext);
                tempData["ErrorMessage"] = "Vui lòng đăng nhập bằng tài khoản Quản trị viên để thực hiện chức năng này";
            }

            var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
            return;
        }

        base.OnActionExecuting(context);
    }
}
