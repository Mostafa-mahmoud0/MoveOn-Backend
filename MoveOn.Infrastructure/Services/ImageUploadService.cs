using Microsoft.AspNetCore.Http;
using MoveOn.Core.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MoveOn.Infrastructure.Services;

public class ImageUploadService : IImageUploadService
{
    private readonly string _uploadsFolder;

    public ImageUploadService()
    {
        // Create uploads folder if it doesn't exist
        _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(_uploadsFolder))
        {
            Directory.CreateDirectory(_uploadsFolder);
        }
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No file uploaded.");
        }

        // Validate file type (only allow images)
        if (!IsImageFile(file))
        {
            throw new ArgumentException("Only image files are allowed.");
        }

        // Generate unique filename
        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(_uploadsFolder, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return relative URL
        return $"/uploads/{fileName}";
    }

    private bool IsImageFile(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        return allowedExtensions.Contains(fileExtension) && 
               file.ContentType.StartsWith("image/");
    }
}