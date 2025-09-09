

namespace Store.Core.DTO.Account
{
  public class ProfileDTO
  {

    public string DispalyName { get; set; }
    public string PhoneNumber { get; set; } 
    public string Email { get; set; }
    public List<AddressDTO> Address { get; set; }


  }
}
