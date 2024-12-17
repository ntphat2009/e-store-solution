using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Common.Model;
using Store.Common.Service;
using Store.Common.Service.Interfaces;
using Store.Domain.Entities;
using Store.Infrastructure.Data;
using Store.Infrastructure.DTOs;
using Store.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;


namespace Store.Infrastructure.Repositories
{
	public class AuthenRepository : IAuthenRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IConfiguration _configuration;
		private readonly IEmailSenderService _emailSender;

		public AuthenRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IEmailSenderService emailSender)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
			_emailSender = emailSender;
		}

		public async Task<string> SignInAsync(SignInDTO signIn)
		{
			var account = await _userManager.FindByNameAsync(signIn.UserName);
			if (account == null)
			{
				throw new Exception("Account was't found");
			}
			else
			{
				var result = await _signInManager.PasswordSignInAsync(signIn.UserName, signIn.Password, false, false);
				if (!result.Succeeded)
				{
					throw new Exception("SignIn Failed");
				}
				else
				{
					var authClaims = new List<Claim>
						{
							new Claim ("UserName",signIn.UserName),
							new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
						};
					var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
					var token = new JwtSecurityToken
				   (
					   claims: authClaims,
					   issuer: _configuration["JWT:ValidIssuer"],
					   audience: _configuration["JWT:ValidAudience"],
					   expires: DateTime.UtcNow.AddHours(5),
					   signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
				   );
					return new JwtSecurityTokenHandler().WriteToken(token);
				}
			}
		}

		public async Task<IdentityResult> SignUpAsync(UserDTO user)
		{
			var applicationUser = new ApplicationUser
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				UserName = user.UserName,
			};
			bool isUsernameExitsted = await _context.Users.AnyAsync(x => x.UserName == user.UserName);
			if (isUsernameExitsted)
			{
				throw new Exception("Tên người dùng đã tồn tại");
			}
			bool isEmailExitsted = await _context.Users.AnyAsync(x => x.Email == user.Email);
			if (isEmailExitsted)
			{
				throw new Exception("Email đã tồn tại");
			}
			var result = await _userManager.CreateAsync(applicationUser, user.Password);
			if (!result.Succeeded)
			{
				return IdentityResult.Failed(result.Errors.ToArray());
			}
			var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
			var encodeToken = HttpUtility.UrlEncode(token);
			var confirmationLink = $"https://localhost:7031/api/Authentications/ConfirmEmail?token={encodeToken}&email={applicationUser.Email}";
			var message = new Message(new string[] { user.Email }, "Liên kết xác thực email", confirmationLink);
			_emailSender.SendEmail(message);
			return result;
		}
		public async Task<IActionResult> ConfirmEmail(string token, string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user != null)
			{
				var result = await _userManager.ConfirmEmailAsync(user, token);
				if (result.Succeeded)
				{
					return new OkObjectResult(
						new { Status = "Success", ErrorMessage = "Email Verified Successfully" });
				}
			}
			return new ObjectResult(
					 new { Status = "Error", ErrorMessage = "This User Does Not Exist!" })
			{
				StatusCode = StatusCodes.Status500InternalServerError
			};
		}

		public async Task<ApplicationUser> GetUserByUserName(string userName)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
			return user ?? new ApplicationUser();
		}
	}
}
