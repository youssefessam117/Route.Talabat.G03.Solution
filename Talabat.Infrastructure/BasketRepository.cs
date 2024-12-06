﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Infrastructure
{
	public class BasketRepository : IBasketRepository
	{

		private readonly IDatabase database;

		public BasketRepository(IConnectionMultiplexer redis)
		{
			database = redis.GetDatabase();
		}
		public async Task<bool> DeleteBasketAsync(string basketId)
		{
			return await database.KeyDeleteAsync(basketId);
		}

		public async Task<CustomerBasket> GetBasketAsync(string basketId)
		{
			var basket = await database.StringGetAsync(basketId);
			return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
		{
			var createdOrUpdated = await database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket) , TimeSpan.FromDays(30));
			if (createdOrUpdated is false) return null;
			return await GetBasketAsync(basket.Id);
		}
	}
}
