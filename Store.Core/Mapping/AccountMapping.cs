

using AutoMapper;
using Store.Core.DTO.Account;
using Store.Core.Entities.UserEntity;

namespace Store.Core.Mapping
{
  public class AccountMapping:Profile
  {
    public AccountMapping()
    {
      CreateMap<AppUser,ProfileDTO>().ReverseMap();
    }
  }
}
