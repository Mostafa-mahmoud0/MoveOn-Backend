using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MoveOn.Core.Interfaces;

public interface IImageUploadService
{
    Task<string> UploadImageAsync(IFormFile file);
}