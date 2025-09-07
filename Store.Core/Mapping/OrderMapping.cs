

using AutoMapper;
using Store.Core.DTO.Account;
using Store.Core.DTO.Order;
using Store.Core.Entities;
using Store.Core.Entities.Order;

namespace Store.Core.Mapping
{
  public class OrderMapping:Profile
  {
    public OrderMapping()
    {
      CreateMap<Orders, OrderToReturnDTO>()
        .ForMember(d => d.status, o => o.MapFrom(s => s.status))
        .ReverseMap();
      CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
      CreateMap<ShippingAddress,ShipAddressDTO>().ReverseMap();
      CreateMap<Address,AddressDTO>().ReverseMap();
      CreateMap<Address, RetriveAddressDTO>().ReverseMap();

    }
  }
}
