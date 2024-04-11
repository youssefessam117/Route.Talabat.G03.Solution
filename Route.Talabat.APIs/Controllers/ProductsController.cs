using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;

namespace Route.Talabat.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : BaseApiController
	{
		private readonly IGenaricRepository<Product> productsRepo;

		public ProductsController(IGenaricRepository<Product> productsRepo)
        {
			this.productsRepo = productsRepo;
		}
    }
}
