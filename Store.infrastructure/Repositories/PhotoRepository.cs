using Store.Core.Entities.Product;
using Store.Core.Interfaces;
using Store.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.infrastructure.Repositories
{
  public class PhotoRepository : GenericReposeitory<Photo>, IPhotoRepository
  {
    public PhotoRepository(AppDbContext context) : base(context)
    {
    }
  }
}
