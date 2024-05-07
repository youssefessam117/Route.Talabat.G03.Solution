﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository basketRepo;
		private readonly IGenaricRepository<Product> productRepo;
		private readonly IGenaricRepository<DeliveryMethod> deliveryMethodRepo;
		private readonly IGenaricRepository<Order> orderRepo;

		public OrderService(
			IBasketRepository basketRepo,
			IGenaricRepository<Product> productRepo,
			IGenaricRepository<DeliveryMethod> deliveryMethodRepo,
			IGenaricRepository<Order> orderRepo
			)
		{
			this.basketRepo = basketRepo;
			this.productRepo = productRepo;
			this.deliveryMethodRepo = deliveryMethodRepo;
			this.orderRepo = orderRepo;
		}
		public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			// 1.Get Basket From Baskets Repo

			var basket = await basketRepo.GetBasketAsync(basketId);


			// 2. Get Selected Items at Basket From Products Repo

			var orderItems = new List<OrderItem>();

			if (basket?.Items?.Count > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = await productRepo.GetAsync(item.Id);
					var productItemOrdered = new ProductItemOrdered(item.Id,product.Name,product.PictureUrl);

					var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

					orderItems.Add(orderItem);
				}
			}

			// 3. Calculate SubTotal

			var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

			// 4. Get Delivery Method From DeliveryMethods Repo

			//var deliveryMethod = await deliveryMethodRepo.GetAsync(deliveryMethodId);


			// 5. Create Order 

			var order = new Order(
				buyerEmail: buyerEmail,
				shippingAddress: shippingAddress,
				deliveryMethodId: deliveryMethodId,
				items: orderItems,
				subtotal: subtotal
				);

			orderRepo.Add(order);

			// 6. Save To Database [TODO]

			return order;
		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
