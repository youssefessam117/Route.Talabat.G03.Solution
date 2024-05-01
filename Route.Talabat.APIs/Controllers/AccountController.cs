using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.Dtos;
using Route.Talabat.APIs.Errors;
using System.Security.Claims;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services.Contract;

namespace Route.Talabat.APIs.Controllers
{

	public class AccountController : BaseApiController
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IAuthService authService;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IAuthService authService
			)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.authService = authService;
		}

		[HttpPost("login")] // POST : /api/account/login
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user is null) return Unauthorized(new ApiResponse(401, "Invalid Login"));

			var restult = await signInManager.CheckPasswordSignInAsync(user, model.Password,false);
			if (!restult.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await authService.CreateTokenAsync(user, userManager)
			});
		}
		[HttpPost("register")] // POST : /api/account/register 
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			var user = new ApplicationUser()
			{
				DisplayName= model.DisplayName,
				Email= model.Email,
				UserName = model.Email.Split('@')[0],
				PhoneNumber = model.Phone
			};
			var result = await userManager.CreateAsync(user,model.Password);
			if (!result.Succeeded) return BadRequest(new ApiValidationErrorResponse() { Errors = result.Errors.Select(e => e.Description) });
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await authService.CreateTokenAsync(user, userManager)
			});
		}


		[Authorize]
		[HttpGet] // GET /api/Account
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

			var user = await userManager.FindByEmailAsync(email);

			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName ?? string.Empty,
				Email = user.Email ?? string.Empty,
				Token = await authService.CreateTokenAsync(user, userManager)
			});
		}
	}
}
