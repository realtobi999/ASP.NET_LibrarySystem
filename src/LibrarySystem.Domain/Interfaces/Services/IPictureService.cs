using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IPictureService
{
    Task<IEnumerable<Picture>> Extract(IFormCollection files);
    Task<Picture> Extract(IFormFile file);
    Task Create(Picture picture);
    Task CreateWithEntity(Picture picture, Guid entityId, PictureEntityType entityType);
    Task BulkCreate(IEnumerable<Picture> pictures);
    Task BulkCreateWithEntity(IEnumerable<Picture> pictures, Guid entityId, PictureEntityType entityType);
}