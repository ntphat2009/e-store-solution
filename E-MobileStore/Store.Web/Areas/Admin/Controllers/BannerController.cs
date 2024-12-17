using Microsoft.AspNetCore.Mvc;
using Store.WebService.DTO;
using Store.WebService.Services.Interfaces;

namespace Store.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BannerController : Controller
	{
		private readonly ICategoryWebService categoryWebService;
		private readonly IBannerWebService bannerWebService;
		private readonly IWebHostEnvironment env;

		public BannerController(IWebHostEnvironment env, IBannerWebService bannerWebService, ICategoryWebService categoryWebService)
		{
			this.categoryWebService = categoryWebService;
			this.bannerWebService = bannerWebService;
			this.env = env;
		}
		private void UploadImage(string ImageURL)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), env.WebRootPath, "uploads/images", ImageURL);
			if (!System.IO.File.Exists(path))
			{
				using var stream = new FileStream(path, FileMode.Create);
			}
		}
		[Route("quan-li-banner")]
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var jwt = HttpContext.Session.GetString("jwtadmin");
			if (jwt == null)
			{
				return RedirectToRoute("login");
			}
			var bannerList = await bannerWebService.GetAllBanner(page, pageSize);
			ViewBag.CategoryList = await categoryWebService.GetAllCategory(1, 100);
			ViewBag.TotalBanner = bannerList.Count;
			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return Json(bannerList);
			}
			return View(bannerList);
		}
		private async void UploadImage(IFormFile fileImage)
		{
			try
			{
				var path = Path.Combine(Directory.GetCurrentDirectory(), env.WebRootPath, "uploads/images", fileImage.FileName);
				if (!System.IO.File.Exists(path))
				{
					using (var stream = new FileStream(path, FileMode.Create))
					{
						await fileImage.CopyToAsync(stream);
					};
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		[HttpPost]
		[Route("quan-li-banner/them-banner")]
		public async Task<IActionResult> BannerAdd(BannerDTO bannerDTO, IFormFile formFile)
		{
			if (!string.IsNullOrEmpty(formFile?.FileName))
			{
				bannerDTO.ImageURL = formFile.FileName;
				var result = await bannerWebService.InsertOrUpdateBanner(bannerDTO);
				if (result != "200")
				{
					ViewBag.ErrorAddCate = result;
				}
				UploadImage(formFile);
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");
		}
		[Route("quan-li-banner/cap-nhat-banner")]
		public async Task<IActionResult> BannerEdit(int bannerId)
		{
			ViewBag.CategoryList = await categoryWebService.GetAllCategory(1, 100);
			ViewBag.BannerInfo = await bannerWebService.GetBannerDetail(bannerId);
			return View();
		}
		[HttpPost]
		[Route("quan-li-banner/cap-nhat-banner")]
		public async Task<IActionResult> BannerEdit(BannerDTO bannerDTO, IFormFile formFile)
		{
			var result = string.Empty;
			if (!string.IsNullOrEmpty(formFile?.FileName))
			{
				bannerDTO.ImageURL = formFile.FileName;
				result = await bannerWebService.InsertOrUpdateBanner(bannerDTO);
				if (result != "200")
				{
					ViewBag.ErrorEditCate = result;
				}
				UploadImage(formFile);
				return RedirectToAction("Index");
			}
			result = await bannerWebService.InsertOrUpdateBanner(bannerDTO);
			if (result != "200")
			{
				ViewBag.ErrorEditCate = result;
			}
			return RedirectToAction("Index");
		}
		[HttpPut]
		[Route("quan-li-banner/xoa-banner")]
		public async Task<IActionResult> BannerDelete(int bannerId)
		{
			var result = await bannerWebService.DeleteBanner(bannerId);
			ViewBag.DeletedMessage = result;
			return Json(new { success = true });
		}
		[Route("quan-li-banner/{bannerId}")]
		public async Task<IActionResult> BannerDetail(int bannerId)
		{
			var result = await bannerWebService.GetBannerDetail(bannerId);
			return View(result);
		}
	}
}
