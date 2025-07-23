using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BaseController : ControllerBase
  {
    protected readonly IUnitOfWork Work;
    protected readonly IMapper mapper;

  

    public BaseController(IUnitOfWork unitOfWork, IMapper mapper)
    {
      Work = unitOfWork;
      this.mapper = mapper;
    }
  }
}
