using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;

namespace Route.Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenaricRepository<Product> productsRepo;

		public ProductsController(IGenaricRepository<Product> productsRepo)
        {
			this.productsRepo = productsRepo;
		}

		// / api/products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await productsRepo.GetAllAsync();

			///JsonResult result = new JsonResult(products);
			///OkResult result = new OkResult(products);

			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var products = await productsRepo.GetAsync(id);

			if (products is null)
			{
				return NotFound( new {message = "Not found" , statsCode = 404}); // 404 
			}

			return Ok(products); // 200 
		}

    }
}
