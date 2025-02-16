﻿using AutoMapper;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class OrderSkuRepository : Repository<OrderSku> , IOrderSkuRepository
	{
		private readonly IMapper _mapper;
		public OrderSkuRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
			_mapper = mapper;
		}

	}
}
