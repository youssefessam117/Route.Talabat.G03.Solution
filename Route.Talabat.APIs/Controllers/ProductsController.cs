using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_specs;

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
			var spec = new ProductWithBrandAndCategorySpecifications();
			var products = await productsRepo.GetAllWithSpecAsync(spec);

			///JsonResult result = new JsonResult(products);
			///OkResult result = new OkResult(products);

			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(id);
			var products = await productsRepo.GetWithSpecAsync(spec);

			if (products is null)
			{
				return NotFound( new {message = "Not found" , statsCode = 404}); // 404 
			}

			return Ok(products); // 200 
		}

    }
}
