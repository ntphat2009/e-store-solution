using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Store.WebService.DTO;
using Store.WebService.Services;
using Store.WebService.Services.Interfaces;

namespace Store.Web.Areas.Admin.Controllers

{
    [Area("Admin")]
    public class AuthenController : Controller
    {
        private readonly IAuthenWebService _authenWebService;

        public AuthenController(IAuthenWebService authenWebService)
        {
            _authenWebService = authenWebService;

        }
        [Route("quan-tri-vien/thong-tin-dang-nhap")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("quan-tri-vien/thong-tin-dang-nhap")]
        public async Task<IActionResult> Index(SignInDTO signInDTO)
        {
            var responseStatus = await _authenWebService.SignIn(signInDTO);
            var errorMessage = "";
            if (responseStatus == "400")
            {
                errorMessage = "Mật khẩu không được để trống";
                ViewBag.ErrorMessage = errorMessage;
                return View("Index");
            }
            else if (responseStatus == "500")
            {
                errorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                ViewBag.ErrorMessage = errorMessage;
                return View("Index");
            }
            else if (responseStatus == "0")
            {
                errorMessage = "Mật khẩu không được để trống";
                ViewBag.ErrorMessage = errorMessage;
                return View("Index");
            }
            else
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(2)
                };
                HttpContext.Session.SetString("jwtadmin", responseStatus);
                //Response.Cookies.Append("jwtadmin", responseStatus, cookieOptions);
                return RedirectToRoute("quan-tri-vien");
            }
        }
    }
}
