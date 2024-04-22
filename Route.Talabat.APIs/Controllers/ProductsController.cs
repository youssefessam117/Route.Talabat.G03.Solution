using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.Dtos;
using Route.Talabat.APIs.Errors;
using Route.Talabat.APIs.Helpers;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_specs;

namespace Route.Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenaricRepository<Product> productsRepo;
		private readonly IMapper mapper;
		private readonly IGenaricRepository<ProductBrand> brandsRepo;
		private readonly IGenaricRepository<ProductCategory> categoriesRepo;

		public ProductsController(
			IGenaricRepository<Product> productsRepo,
			IMapper mapper,
			IGenaricRepository<ProductBrand> brandsRepo,
			IGenaricRepository<ProductCategory> categoriesRepo
			)
        {
			this.productsRepo = productsRepo;
			this.mapper = mapper;
			this.brandsRepo = brandsRepo;
			this.categoriesRepo = categoriesRepo;
		}

		// / api/products
		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(specParams);
			var products = await productsRepo.GetAllWithSpecAsync(spec);

			///JsonResult result = new JsonResult(products);
			///OkResult result = new OkResult(products);

			var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			var countSpec = new ProductWithFilterationForCountSpecification(specParams);

			var count = await productsRepo.GetCountAsync(countSpec);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize,count, data));
		}

		[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(id);
			var products = await productsRepo.GetWithSpecAsync(spec);

			if (products is null)
			{
				return NotFound(new ApiResponse(404)); // 404 
			}

			return Ok(mapper.Map<Product,ProductToReturnDto>(products)); // 200 
		}

		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await brandsRepo.GetAllAsync();
			return Ok(brands);
		}

		[HttpGet("categories")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await categoriesRepo.GetAllAsync();
			return Ok(categories);
		}

    }
}
