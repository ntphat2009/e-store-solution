using CKSource.CKFinder.Connector.Core.Nodes;
using Microsoft.AspNetCore.Mvc;
using Store.WebService.DTO;
using Store.WebService.Services;
using Store.WebService.Services.Interfaces;
using System.IO;

namespace Store.Web.Areas.Admin.Controllers

{
	[Area("Admin")]

	public class CategoryController : Controller
	{
		private readonly ICategoryWebService categoryWebService;
		private readonly IWebHostEnvironment env;

		public CategoryController(IWebHostEnvironment env, ICategoryWebService categoryWebService)
		{
			this.categoryWebService = categoryWebService;
			this.env = env;
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
		[Route("quan-li-nganh-hang")]
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var jwt = HttpContext.Session.GetString("jwtadmin");
			if (jwt == null)
			{
				return RedirectToRoute("login");
			}
			var categoryList = await categoryWebService.GetAllCategory(page, pageSize);
			ViewBag.TotalCategory = categoryList.Count;
			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return Json(categoryList);
			}
			return View(categoryList);
		}
		[HttpPost]
		[Route("quan-li-nganh-hang/them-nganh-hang")]
		public async Task<IActionResult> CategoryAdd(IFormFile formFile, CategoryDTO categoryDTO)
		{

			if (!string.IsNullOrEmpty(formFile?.FileName))
			{
				categoryDTO.ImageURL = formFile.FileName;
				var result = await categoryWebService.InsertOrUpdateCategory(categoryDTO);
				if (result != "200")
				{
					ViewBag.ErrorAddCate = result;
				}
				UploadImage(formFile);
				return RedirectToAction("Index");
			}
			ViewBag.ErrorAddCate = "chưa có hình ảnh";
			return RedirectToAction("Index");
		}
		[Route("quan-li-nganh-hang/cap-nhat-nganh-hang")]
		public async Task<IActionResult> CategoryEdit(string categoryUrl)
		{
			ViewBag.CategoryInfo = await categoryWebService.GetCategoryByURL(categoryUrl);
			return View();
		}
		[HttpPost]
		[Route("quan-li-nganh-hang/cap-nhat-nganh-hang")]
		public async Task<IActionResult> CategoryEdit(IFormFile formFile, CategoryDTO categoryDTO)
		{
			var result = string.Empty;
			if (!string.IsNullOrEmpty(formFile?.FileName))
			{
				categoryDTO.ImageURL = formFile.FileName;
				result = await categoryWebService.InsertOrUpdateCategory(categoryDTO);
				if (result != "200")
				{
					ViewBag.ErrorEditCate = result;
				}
				UploadImage(formFile);
				return RedirectToAction("Index");
			}
			result = await categoryWebService.InsertOrUpdateCategory(categoryDTO);
			if (result != "200")
			{
				ViewBag.ErrorEditCate = result;
			}
			return RedirectToAction("Index");
		}
		[HttpPut]
		[Route("quan-li-nganh-hang/xoa-nganh-hang")]
		public async Task<IActionResult> CategoryDelete(string categoryUrl)
		{
			var result = await categoryWebService.DeleteCategory(categoryUrl);
			if (result != "200")
			{
				return Json(new { success = false, error = result });
			}
			return Json(new { success = true });
		}
		[HttpPut]
		[Route("quan-li-nganh-hang/xoa-nhieu-nganh-hang")]
		public async Task<IActionResult> CategoriesDelete(List<string> categoryUrls)
		{

			if (categoryUrls != null)
			{
				foreach (var categoryUrl in categoryUrls)
				{
					var result = await categoryWebService.DeleteCategory(categoryUrl);
					ViewBag.DeletedMessage = result;
				}
				return Json(new { success = true });
			}
			return Json(new { success = false });

		}
		[Route("quan-li-nganh-hang/{categoryUrl}")]
		public async Task<IActionResult> CategoryDetail(string categoryUrl)
		{
			var result = await categoryWebService.GetCategoryByURL(categoryUrl);
			return View(result);
		}
	}
}
