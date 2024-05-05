using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.Order_Aggregate
{
	public enum OrderStatus
	{
		//[EnumMember]
		Pending,
		PaymentReceived,
		PaymentFailed
	}
}
