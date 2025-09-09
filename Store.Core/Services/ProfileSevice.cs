

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Store.Core.DTO.Account;
using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.Core.Services
{
  public class ProfileSevice : IProfileService
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ProfileSevice(UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
    {
      _userManager = userManager;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDTO dto)
    {
      var user =await _userManager.FindByIdAsync(userId);
      if(user is null) return false;

      var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
      return result.Succeeded;  

    }

    public async Task<bool> DeleteAccountAsync(string userId)
    {
      var user =await _userManager.FindByIdAsync(userId);
      if(user is null) return false;

      var result = await _userManager.DeleteAsync(user);
      return result.Succeeded;
    }

    public async Task<ProfileDTO?> GetProfileAsync(string userId)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null) return null;

      var profile = _mapper.Map<ProfileDTO>(user);
      var address = await _unitOfWork.AddressRepository.GetAddressesAsync(userId);
      profile.Address = _mapper.Map<List<AddressDTO>>(address);
      return profile;
    }

    public async Task<ProfileDTO?> UpdateProfileAsync(string userId, UpdateProfileDTO dto)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if(user is null) return null;

      user.DispalyName = dto.DispalyName;
      user.PhoneNumber = dto.PhoneNumber;
      var result = await _userManager.UpdateAsync(user);

      if(!result.Succeeded) return null;

      var profile= _mapper.Map<ProfileDTO>(user);

      var address = await _unitOfWork.AddressRepository.GetAddressesAsync(userId);
      profile.Address = _mapper.Map<List<AddressDTO>>(address);
      return profile;

    }
  }
}
