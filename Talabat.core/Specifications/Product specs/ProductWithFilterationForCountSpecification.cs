using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications.Product_specs
{
	public class ProductWithFilterationForCountSpecification : BaseSpecifications<Product>
	{
		public ProductWithFilterationForCountSpecification(ProductSpecParams specParams)
			: base(p =>

				 (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId.Value) &&
				 (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId.Value)
			)
		{

		}
	}
}
