using LibrarySystem.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IPictureService
{
    Task<IEnumerable<Picture>> Extract(IFormCollection files);
    Task<Picture> Extract(IFormFile file);
    Task<int> Create(Picture picture);
}
