using LibrarySystem.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IPictureService
{
    Task<IEnumerable<Picture>> Extract(IFormCollection files);
    Task<int> Create(Picture picture);
}
