using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Store.Core.Interfaces;

namespace Store.Core.Services
{
  public class PhotosService : IPhotosService
  {
    private readonly IFileProvider fileProvider;
    public PhotosService(IFileProvider fileProvider)
    {
      this.fileProvider = fileProvider;
    }
    public async Task<List<string>> AddPhotoAsync(IFormFileCollection files, string src)
    {
      var saveImageSrc=new List<string>();
      var directory=Path.Combine("wwwroot", "images",src);
      if(!Directory.Exists(directory))
        Directory.CreateDirectory(directory);

      foreach (var file in files)
      {
        if (file.Length > 0)
        {
          var fileName = Path.GetFileName(file.FileName);
          var filePath = Path.Combine(directory, fileName);
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await file.CopyToAsync(stream);
            saveImageSrc.Add(Path.Combine("images", src, fileName));
          }
        }
      }
      return saveImageSrc;

    }

    public void DeletePhoto(string src)
    {
      var info=fileProvider.GetFileInfo(src);
      if (info.Exists)
      {
        var filePath = info.PhysicalPath;
        if (File.Exists(filePath))
        {
          File.Delete(filePath);
        }
      }
    }
  }
}
