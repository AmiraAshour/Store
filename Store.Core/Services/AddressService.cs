
using AutoMapper;
using Store.Core.DTO.Account;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.Core.Services
{
  public class AddressService : IAddressService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    public async Task<RetriveAddressDTO?> AddAddressAsync(AddressDTO addressDTO, string userId)
    {
      var address = _mapper.Map<Address>(addressDTO);
      address.AppUserId = userId;
      var reselt= await _unitOfWork.AddressRepository.AddAddressAsync(address);
      return _mapper.Map<RetriveAddressDTO>(reselt);
    }

    public async Task<bool> DeleteAddressAsync(int id, string userId)
    {
      var existAddress = await GetAddressByIdAsync(id);
      if (existAddress == null)
      {
        throw new Exception("Address not found.");
      }
      if (existAddress.AppUserId != userId)
      {
        throw new Exception("You are not authorized to delete this address.");
      }
      return await _unitOfWork.AddressRepository.DeleteAddressAsync(id);
    }


    public async Task<Address?> GetAddressByIdAsync(int id)
    {
      return await _unitOfWork.AddressRepository.GetAddressById(id);
    }

    public async Task<List<RetriveAddressDTO>?> GetAllAddressesAsync(string userId)
    {
      var result= await _unitOfWork.AddressRepository.GetAddressesAsync(userId);
      return _mapper.Map<List<RetriveAddressDTO>>(result);
    }

    public async Task<RetriveAddressDTO?> UpdateAddressAsync(int addresId, AddressDTO addressDto, string userId)
    {
      var existAddress = await GetAddressByIdAsync(addresId);
      if (existAddress is null || existAddress.AppUserId != userId)
        throw new Exception("Address not found.");
      var address = _mapper.Map(addressDto, existAddress);
      address.AppUserId = userId;
      address.Id= addresId;
      var result= await _unitOfWork.AddressRepository.UpdateAddressAsync(address);
      return _mapper.Map<RetriveAddressDTO>(result);
    }

  }
}
