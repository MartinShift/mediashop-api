using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using MediaShop.Configuration;
using MediaShop.Data.Entities;
using Microsoft.Extensions.Options;

namespace MediaShop.Business.Services;

public class MediaService(IOptions<BlobStorageSettings> blobStorageSettings) : IMediaService
{
    private readonly BlobStorageSettings _blobStorageSettings = blobStorageSettings.Value;

    private const string _emptyImage = "http://crowdfundline.com/testing/crowdfund/assets/images/empty.png";

    public async Task<string> UploadAsync(IFormFile file)
    {
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.ContainerName);
        var guid = Guid.NewGuid();
        var blobClient = blobContainerClient.GetBlobClient($"{guid}{Path.GetExtension(file.FileName)}");
        await blobClient.UploadAsync(file.OpenReadStream(), true);
        return blobClient.Uri.ToString();

    }

    public async Task<string> UploadImageAsync(IFormFile? file)
    {
        if(file != null)
        {
            return await UploadAsync(file);
        }
        return _emptyImage;
    }

    public async Task RemoveImagesAsync(Product product)
    {
        if(product.MediaUrl != _emptyImage)
        {
            await RemoveImageAsync(product.MediaUrl);
        }
        if (product.PreviewUrl != _emptyImage)
        {
            await RemoveImageAsync(product.PreviewUrl);
        }
    }

    public async Task RemoveImageAsync(string? url)
    {
        if (url == _emptyImage)
        {
            return;
        }

        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.ContainerName);
        try
        {
            var blobClient = blobContainerClient.GetBlobClient(new Uri(url).Segments.Last());
            await blobClient.DeleteIfExistsAsync();
        }
        catch (ArgumentNullException)
        {
            return;
        }
    }
}
