using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;

namespace Route.Talabat.APIs.Controllers
{
	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository basketRepository;

		public BasketController(IBasketRepository basketRepository)
        {
			this.basketRepository = basketRepository;
		}

		[HttpGet] // GET: /api/basket?id
		public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
		{
			var basket = await basketRepository.GetBasketAsync(id);
			return Ok(basket ?? new CustomerBasket(id));
		}

		[HttpPost]// POST: /api/basket
		public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
		{
			var createdOrUpdatedBasket =  await basketRepository.UpdateBasketAsync(basket);
			if (createdOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
			return Ok(createdOrUpdatedBasket);

		}
		[HttpDelete]// DELETE : /api/basket
		public async Task DeleteBasket(string id)
		{
			await basketRepository.DeleteBasketAsync(id);
		}
    }
}
