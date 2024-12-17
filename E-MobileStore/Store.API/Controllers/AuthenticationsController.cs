using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.ApiService.Services.Interfaces;
using Store.Infrastructure.DTOs;
using System.Net;

namespace Store.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationsController : ControllerBase
	{
		private readonly IAuthenService _authenService;
		private readonly BaseApiResponse _response;

		public AuthenticationsController(IAuthenService authenService)
		{
			_authenService = authenService;
			_response = new BaseApiResponse();
		}
		[HttpGet("ConfirmEmail")]
		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail(string token, string email)
		{
			return await _authenService.ConfirmEmail(token, email);
		}
		[HttpPost]
		[Route("SignUp")]
		[AllowAnonymous]
		public async Task<IActionResult> SignUpAsync(UserDTO userDTO)
		{
			try
			{
				var result = await _authenService.SignUpAsync(userDTO);
				if (result.Succeeded)
				{
					_response.Result = result;
					return Ok(_response);
				}
				else
				{
					var errorMessages = result.Errors.Select(e => e.Description).ToList();
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages = errorMessages;
					_response.Result = errorMessages;
					_response.IsSuccess = false;
					return BadRequest(_response);
				}
			}
			catch (Exception ex)
			{
				_response.StatusCode = HttpStatusCode.InternalServerError;
				_response.ErrorMessages = new List<string> { ex.Message };
				_response.IsSuccess = false;
				_response.Result = ex.Message;
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}
		[HttpPost]
		[Route("SignIn")]
		public async Task<IActionResult> SignInAsync(SignInDTO signInDTO)
		{
			try
			{
				var result = await _authenService.SignInAsync(signInDTO);
				_response.Result= result;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.StatusCode = HttpStatusCode.InternalServerError;
				_response.ErrorMessages = new List<string> { ex.Message };
				_response.IsSuccess = false;
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}
		[HttpGet]
		[Route("FindUser")]
		public async Task<IActionResult> GetUserByUserName(string userName)
		{
			try
			{
				var result = await _authenService.GetUserByUserName(userName);
				_response.Result = result;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.StatusCode = HttpStatusCode.InternalServerError;
				_response.ErrorMessages = new List<string> { ex.Message };
				_response.IsSuccess = false;
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}

	}
}
