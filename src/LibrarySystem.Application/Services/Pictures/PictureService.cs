using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Application.Services.Pictures;

public class PictureService : IPictureService
{
    private readonly IRepositoryManager _repository;

    public PictureService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task Create(Picture picture)
    {
        _repository.Picture.Create(picture);

        await _repository.SaveSafelyAsync();
    }

    public async Task BulkCreate(IEnumerable<Picture> pictures)
    {
        foreach (var picture in pictures)
        {
            _repository.Picture.Create(picture);
        }

        await _repository.SaveSafelyAsync();
    }

    public async Task BulkCreateWithEntity(IEnumerable<Picture> pictures, Guid entityId, PictureEntityType entityType)
    {
        pictures.ToList().ForEach(p =>
        {
            p.EntityId = entityId;
            p.EntityType = entityType;
        });

        await this.BulkCreate(pictures);
    }

    public async Task<IEnumerable<Picture>> Extract(IFormCollection files)
    {
        var pictures = new List<Picture>();
        foreach (var file in files.Files)
        {
            pictures.Add(await this.Extract(file));
        }

        return pictures;
    }

    public async Task<Picture> Extract(IFormFile file)
    {
        return new Picture
        {
            Id = Guid.NewGuid(),
            FileContent = await file.GetBytes(),
            FileName = file.FileName,
            MimeType = file.ContentType,
            CreatedAt = DateTimeOffset.Now
        };
    }

    public async Task CreateWithEntity(Picture picture, Guid entityId, PictureEntityType entityType)
    {
        picture.EntityId = entityId;
        picture.EntityType = entityType;

        await this.Create(picture);
    }
}