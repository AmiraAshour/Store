using System.Collections;

namespace Store.API.Helper
{
  public class Pagination<T> where T : class
  {
    public Pagination(int pageNumber, int totalCount, int pageSize, IEnumerable<T> data)
    {
      PageNumber = pageNumber;
      TotalCount = totalCount;
      PageSize = pageSize;
      Data = data;
    }

    public int PageNumber { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<T> Data { get; set; }
  }
}
