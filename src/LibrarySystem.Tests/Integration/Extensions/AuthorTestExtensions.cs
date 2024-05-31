using Bogus;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests;

public static class AuthorTestExtensions
{
    private static readonly Faker<Author> _authorFaker = new Faker<Author>()
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.Name, f => f.Name.FullName())
        .RuleFor(a => a.Description, f => f.Lorem.Paragraph())
        .RuleFor(a => a.Birthday, f => f.Date.Past(50, DateTime.Now.AddYears(-20))); // Assuming authors are at least 20 years old

    public static Author WithFakeData(this Author author)
    {
        return _authorFaker.Generate();
    }

    public static CreateAuthorDto ToCreateAuthorDto(this Author author)
    {
        return new CreateAuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Description = author.Description,
            Birthday = author.Birthday,
            ProfilePicture = Convert.ToBase64String(author.ProfilePicture ?? Array.Empty<byte>()) // Convert byte[] to base64 string
        };
    }
}
