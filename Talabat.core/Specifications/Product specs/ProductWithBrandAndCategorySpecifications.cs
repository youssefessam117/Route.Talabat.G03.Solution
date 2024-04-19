using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications.Product_specs
{
	public class ProductWithBrandAndCategorySpecifications :BaseSpecifications<Product>
	{
		// this constructor Will be used for creating an Object, that will used to get all products 
		public ProductWithBrandAndCategorySpecifications()
		{
			AddIncludes();
		}

		// this constructor Will be used for creating an Object, that will used to get spicfic product with id  
		public ProductWithBrandAndCategorySpecifications(int id )
			:base(p => p.Id == id)
		{
			AddIncludes();
		}

		private void AddIncludes()
		{
			Includes.Add(p => p.Brand);
			Includes.Add(p => p.Category);
		}
	}
}
