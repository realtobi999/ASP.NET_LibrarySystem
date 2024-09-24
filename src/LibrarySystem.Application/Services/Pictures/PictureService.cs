using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Application.Services.Pictures;

public class PictureService : IPictureService
{
    private readonly IRepositoryManager _repository;

    public PictureService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<int> Create(Picture picture)
    {
        _repository.Picture.Create(picture);

        return await _repository.SaveAsync();
    }

    public async Task<int> BulkCreate(IEnumerable<Picture> pictures)
    {
        foreach (var picture in pictures)
        {
            _repository.Picture.Create(picture);
        }

        return await _repository.SaveAsync();
    }

    public Task<int> BulkCreateWithEntity(IEnumerable<Picture> pictures, Guid entityId, PictureEntityType entityType)
    {
        pictures.ToList().ForEach(p => { p.EntityId = entityId; p.EntityType = entityType; });

        return this.BulkCreate(pictures);
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
        };
    }

    public Task<int> CreateWithEntity(Picture picture, Guid entityId, PictureEntityType entityType)
    {
        picture.EntityId = entityId;
        picture.EntityType = entityType;

        return this.Create(picture);
    }
}
