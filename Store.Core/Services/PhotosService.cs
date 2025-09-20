using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Store.Core.Interfaces.ServiceInterfaces;

namespace Store.Core.Services
{
  public class PhotosService : IPhotosService
  {
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<PhotosService> _logger;

    public PhotosService(IFileProvider fileProvider, ILogger<PhotosService> logger)
    {
      _fileProvider = fileProvider;
      _logger = logger;
    }

    public async Task<List<string>> AddPhotoAsync(IFormFileCollection files, string src)
    {
      var saveImageSrc = new List<string>();
      var directory = Path.Combine("wwwroot", "images", src);

      if (!Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
        _logger.LogInformation("Created directory: {Directory}", directory);
      }

      foreach (var file in files)
      {
        if (file.Length > 0)
        {
          var fileName = Path.GetFileName(file.FileName);
          var filePath = Path.Combine(directory, fileName);

          try
          {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
              await file.CopyToAsync(stream);
            }

            var relativePath = Path.Combine("images", src, fileName);
            saveImageSrc.Add(relativePath);

            _logger.LogInformation("Saved file: {FilePath}", relativePath);
          }
          catch (Exception ex)
          {
            _logger.LogError(ex, "Error saving file: {FileName}", fileName);
          }
        }
        else
        {
          _logger.LogWarning("Skipped empty file: {FileName}", file.FileName);
        }
      }

      return saveImageSrc;
    }

    public void DeletePhoto(string src)
    {
      var info = _fileProvider.GetFileInfo(src);

      if (info.Exists)
      {
        var filePath = info.PhysicalPath;
        try
        {
          if (File.Exists(filePath))
          {
            File.Delete(filePath);
            _logger.LogInformation("Deleted file: {FilePath}", src);
          }
          else
          {
            _logger.LogWarning("File not found for deletion: {FilePath}", src);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Error deleting file: {FilePath}", src);
        }
      }
      else
      {
        _logger.LogWarning("File info does not exist for: {FilePath}", src);
      }
    }
  }
}
