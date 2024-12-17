using Microsoft.AspNetCore.Mvc;
using Store.WebService.DTO;
using Store.WebService.Services.Interfaces;

namespace Store.Web.Controllers
{
	public class AuthenticationController : Controller
	{
		private readonly IAuthenWebService _authenService;

		public AuthenticationController(IAuthenWebService authenService)
		{
			_authenService = authenService;
		}
		[Route("nguoi-dung/thong-tin-dang-nhap")]
		public async Task<IActionResult> SignIn()
		{
			return View();
		}
		[HttpPost]
		[Route("nguoi-dung/thong-tin-dang-nhap")]
		public async Task<IActionResult> SignIn(SignInDTO signInDTO)
		{
			var responseStatus = await _authenService.SignIn(signInDTO);
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
				Response.Cookies.Append("jwt", responseStatus, cookieOptions);
				return RedirectToRoute("home", new { controller = "Home", action = "Index" });
			}
		}
		[Route("nguoi-dung/dang-ky")]
		public IActionResult SignUp()
		{
			return View();
		}
		[Route("nguoi-dung/thong-tin-dang-ky")]
		public async Task<IActionResult> HandleSignUp(SignUpDTO signUpDTO)
		{
			var responseStatus = await _authenService.SignUp(signUpDTO);
			var errorMessage = "";
			if (responseStatus == "OK")
			{
				var cookieOptions = new CookieOptions
				{
					HttpOnly = true,
					Expires = DateTime.UtcNow.AddHours(2)
				};
				Response.Cookies.Append("userName", signUpDTO.UserName, cookieOptions);
				return View("WaitingConfirmEmail");
			}
			else
			{
				errorMessage = responseStatus;
				ViewBag.ErrorMessage = errorMessage;
				return View("SignUp");
			}
		}
		[Route("nguoi-dung/xac-nhan-email")]
		public IActionResult WaitingConfirmEmail()
		{
			return View();
		}
		[Route("api/nguoi-dung/check-email-confirmation")]
		public async Task<IActionResult> CheckEmailConfirmation()
		{
            var userNameCookie = Request.Cookies["userName"];
            var userInfo =await _authenService.GetUser(userNameCookie);

            if (userInfo.EmailConfirmed)
			{
                Response.Cookies.Delete("userName");
                return Json(new { isConfirmed = true });
			}
            return Json(new { isConfirmed = false });
		}

		[Route("nguoi-dung/dang-xuat")]
		public IActionResult LogOut()
		{
			Response.Cookies.Delete("jwt");
			return RedirectToRoute("home", new { controller = "Home", action = "Index" });
		}
	}
}
