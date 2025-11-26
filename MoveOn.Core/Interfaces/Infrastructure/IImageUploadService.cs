using Microsoft.AspNetCore.Http;
using MoveOn.Core.Interfaces.Infrastructure;
using System;
using System.Threading.Tasks;

namespace MoveOn.Core.Interfaces.Infrastructure;

public interface IImageUploadService
{
    Task<string> UploadImageAsync(IFormFile file);
}