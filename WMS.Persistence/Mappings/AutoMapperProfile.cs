
using AutoMapper;
using WMS.Application.Dtos;
using WMS.Domain.Entities;

namespace WMS.Persistence.Mappings
{
	public class AutoMapperProfile : Profile
	{
        public AutoMapperProfile()
		{
			// Customer Mappings
			CreateMap<CustomerInsertDto, Customer>()
				.ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow));
			CreateMap<CustomerUpdateDto, Customer>();

			// Line Mappings
			CreateMap<LineInsertDto, Line>()
				.ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow));
			CreateMap<LineUpdateDto, Line>();

			// Location Mappings
			CreateMap<LocationInsertDto, Location>()
				.ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow));
			CreateMap<LocationUpdateDto, Location>();

			// Order Mappings
			CreateMap<OrderInsertDto, Order>()
				.ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow));
			CreateMap<OrderUpdateDto, Order>();

			// Product Mappings
			CreateMap<ProductInsertDto, Product>()
				.ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow));
			CreateMap<ProductUpdateDto, Product>();

			// OrderSku Mappings
			CreateMap<OrderSkuInsertDto, OrderSku>()
				.ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow));
			CreateMap<OrderSkuUpdateDto, OrderSku>();

			// Sku Mappings
			CreateMap<SkuInsertDto, Sku>()
				.ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow));
			CreateMap<SkuUpdateDto, Sku>();
		}
    }
}
