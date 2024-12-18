﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
	public interface IGenaricRepository<T> where T : BaseEntity
	{
		Task<T?> GetByIdAsync(int id);
		Task<IReadOnlyList<T>> GetAllAsync();

		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);

		Task<T?> GetWithSpecAsync(ISpecifications<T> spec);

		Task<int> GetCountAsync(ISpecifications<T> spec);

		void Add(T entity);

		void Update(T entity);

		void Delete(T entity);

	}
}
