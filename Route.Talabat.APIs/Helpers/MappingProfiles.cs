using AutoMapper;
using Route.Talabat.APIs.Dtos;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;

namespace Route.Talabat.APIs.Helpers
{
	public class MappingProfiles : Profile
	{

		public MappingProfiles()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
				.ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
				.ForMember(p => p.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();
			CreateMap<Address, AddressDto>().ReverseMap();


		}
	}
}
