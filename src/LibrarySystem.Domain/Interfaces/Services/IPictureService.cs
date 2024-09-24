using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IPictureService
{
    Task<IEnumerable<Picture>> Extract(IFormCollection files);
    Task<Picture> Extract(IFormFile file);
    Task<int> Create(Picture picture);
    Task<int> CreateWithEntity(Picture picture, Guid entityId, PictureEntityType entityType);
    Task<int> BulkCreate(IEnumerable<Picture> pictures);
    Task<int> BulkCreateWithEntity(IEnumerable<Picture> pictures, Guid entityId, PictureEntityType entityType);
}
