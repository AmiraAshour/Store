using AutoMapper;
using Store.Core.DTO.Account;
using Microsoft.Extensions.Logging;
using Store.Core.Entities.UserEntity;
using Store.Core.Interfaces.ServiceInterfaces;

namespace Store.Core.Services
{
  public class AddressService : IAddressService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddressService> _logger;

    public AddressService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddressService> logger)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<RetriveAddressDTO?> AddAddressAsync(AddressDTO addressDTO, string userId)
    {
      _logger.LogInformation("Adding address for user {UserId}", userId);
      var address = _mapper.Map<Address>(addressDTO);
      address.AppUserId = userId;
      var reselt = await _unitOfWork.AddressRepository.AddAddressAsync(address);
      _logger.LogInformation("Address added with ID {AddressId} for user {UserId}", reselt?.Id, userId);
      return _mapper.Map<RetriveAddressDTO>(reselt);
    }

    public async Task<bool> DeleteAddressAsync(int id, string userId)
    {
      _logger.LogInformation("Deleting address {AddressId} for user {UserId}", id, userId);
      var existAddress = await GetAddressByIdAsync(id);
      if (existAddress == null)
      {
        _logger.LogWarning("Address {AddressId} not found for user {UserId}", id, userId);
        throw new Exception("Address not found.");
      }
      if (existAddress.AppUserId != userId)
      {
        _logger.LogWarning("User {UserId} not authorized to delete address {AddressId}", userId, id);
        throw new Exception("You are not authorized to delete this address.");
      }
      var result = await _unitOfWork.AddressRepository.DeleteAddressAsync(id);
      _logger.LogInformation("Address {AddressId} deleted for user {UserId}: {Result}", id, userId, result);
      return result;
    }

    public async Task<Address?> GetAddressByIdAsync(int id)
    {
      _logger.LogInformation("Retrieving address by ID {AddressId}", id);
      return await _unitOfWork.AddressRepository.GetAddressById(id);
    }

    public async Task<List<RetriveAddressDTO>?> GetAllAddressesAsync(string userId)
    {
      _logger.LogInformation("Retrieving all addresses for user {UserId}", userId);
      var result = await _unitOfWork.AddressRepository.GetAddressesAsync(userId);
      _logger.LogInformation("Found {Count} addresses for user {UserId}", result?.Count ?? 0, userId);
      return _mapper.Map<List<RetriveAddressDTO>>(result);
    }

    public async Task<RetriveAddressDTO?> UpdateAddressAsync(int addresId, AddressDTO addressDto, string userId)
    {
      _logger.LogInformation("Updating address {AddressId} for user {UserId}", addresId, userId);
      var existAddress = await GetAddressByIdAsync(addresId);
      if (existAddress is null || existAddress.AppUserId != userId)
      {
        _logger.LogWarning("Address {AddressId} not found or not authorized for user {UserId}", addresId, userId);
        throw new Exception("Address not found.");
      }
      var address = _mapper.Map(addressDto, existAddress);
      address.AppUserId = userId;
      address.Id = addresId;
      var result = await _unitOfWork.AddressRepository.UpdateAddressAsync(address);
      _logger.LogInformation("Address {AddressId} updated for user {UserId}", addresId, userId);
      return _mapper.Map<RetriveAddressDTO>(result);
    }
  }
}
