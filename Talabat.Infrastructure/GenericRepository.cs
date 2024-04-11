using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure
{
	public class GenericRepository<T> : IGenaricRepository<T> where T : BaseEntity
	{
		private readonly StoreContext dbContext;

		public GenericRepository(StoreContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await dbContext.Set<T>().ToListAsync();
		}

		public async Task<T?> GetAsync(int id)
		{
			return await dbContext.Set<T>().FindAsync(id);
		}
	}
}
