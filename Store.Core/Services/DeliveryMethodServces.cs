using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Core.DTO.Order;
using Store.Core.Entities.Order;
using Store.Core.Interfaces.ServiceInterfaces;

namespace Store.Core.Services
{
  public class DeliveryMethodServices : IDeliveryMethodServce
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeliveryMethodServices> _logger;
    private readonly IMapper _mapper;

    public DeliveryMethodServices(IUnitOfWork unitOfWork, ILogger<DeliveryMethodServices> logger, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _logger = logger;
      _mapper = mapper;
    }

    public async Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodAsync()
    {
      _logger.LogInformation("Fetching all delivery methods from database.");
      return await _unitOfWork.DeliveryMethodRepository.GetDeliveryMethodsAsync();
    }

    public async Task<DeliveryMethod?> GetDeliveryMethodByIdAsync(int id)
    {
      _logger.LogInformation("Fetching delivery method with ID: {Id}", id);
      return await _unitOfWork.DeliveryMethodRepository.GetByIdAsync(id);
    }

    public async Task<DeliveryMethod> AddDeliveryMethodAsync(DeliveryMethodDTO methodDTO)
    {
      _logger.LogInformation("Adding new delivery method: {Name}", methodDTO.Name);
      var method = _mapper.Map<DeliveryMethod>(methodDTO);
      await _unitOfWork.DeliveryMethodRepository.AddAsync(method);
      return method;
    }

    public async Task<DeliveryMethod?> UpdateDeliveryMethodAsync(int Id, DeliveryMethodDTO method)
    {
      _logger.LogInformation("Updating delivery method: {Id}",Id);
      var existing = await _unitOfWork.DeliveryMethodRepository.GetByIdAsync(Id);

      if (existing == null) return null;

      var updated = _mapper.Map(method, existing);
      await _unitOfWork.DeliveryMethodRepository.UpdateAsync(updated);
      return updated;
    }

    public async Task<bool> DeleteDeliveryMethodAsync(int id)
    {
      _logger.LogInformation("Deleting delivery method: {Id}", id);
      var method = await _unitOfWork.DeliveryMethodRepository.GetByIdAsync(id);
      if (method == null) return false;

      await _unitOfWork.DeliveryMethodRepository.DeleteAsync(method);

      return true;
    }
  }
}
