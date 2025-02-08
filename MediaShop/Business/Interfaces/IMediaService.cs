using MediaShop.Business.Models;
using MediaShop.Data.Entities;

namespace MediaShop.Business.Interfaces;

public interface IMediaService
{
   Task<string> UploadAsync(IFormFile file);

   Task<string> UploadImageAsync(IFormFile? file);

    Task RemoveImagesAsync(Product product);

    Task RemoveImageAsync(string? url);
}
