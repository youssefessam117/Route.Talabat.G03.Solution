using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Repositories.Contract
{
	public interface IGenaricRepository<T> where T : BaseEntity
	{
		Task<T?> GetAsync(int id);
		Task<IEnumerable<T>> GetAllAsync();
	}
}
