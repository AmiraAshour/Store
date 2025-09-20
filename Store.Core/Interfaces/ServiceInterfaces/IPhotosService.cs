using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IPhotosService
  {
    Task<List<string>> AddPhotoAsync(IFormFileCollection files, string src);
    void DeletePhoto( string src);
  }
}
