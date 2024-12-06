﻿using Talabat.Core.Entites.Order_Aggregate;

namespace Route.Talabat.APIs.Dtos
{
	public class OrderToReturnDto
	{
		public int Id { get; set; }
		public string BuyerEmail { get; set; }

		public DateTimeOffset OrderDate { get; set; }

		public OrderStatus Status { get; set; }
		public Address ShippingAddress { get; set; }


		public string DeliveryMethod { get; set; }

		public decimal DeliveryMethodCost { get; set; }


		public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();

		public decimal Subtotal { get; set; }

		public decimal Total { get; set; }

		public string PaymentIntentId { get; set; } = string.Empty;
	}
}
