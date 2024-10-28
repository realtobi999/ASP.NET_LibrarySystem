using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/**

GET     /api/wishlist/{wishlist_id}
POST    /api/wishlist
PUT     /api/wishlist/{wishlist_id}
DELETE  /api/wishlist/{wishlist_id}

**/
public class WishlistController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IMapperManager _mapper;

    public WishlistController(IServiceManager service, IMapperManager mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [Authorize(Policy = "User")]
    [HttpGet("api/wishlist/{wishlistId:guid}")]
    public async Task<IActionResult> GetWishlist(Guid wishlistId)
    {
        var wishlist = await _service.Wishlist.GetAsync(wishlistId);

        // verify that the request user id matches the user id of the wishlist
        if (JwtUtils.ParseFromPayload(JwtUtils.Parse(HttpContext.Request.Headers.Authorization), "UserId") != wishlist.UserId.ToString())
        {
            throw new NotAuthorized401Exception();
        }

        return Ok(wishlist);
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPost("api/wishlist")]
    public async Task<IActionResult> CreateWishlist([FromBody] CreateWishlistDto createWishlistDto)
    {
        var wishlist = _mapper.Wishlist.Map(createWishlistDto);

        await _service.Wishlist.CreateAsync(wishlist);

        return Created($"/api/wishlist/{wishlist.Id}", null);
    }

    [Authorize(Policy = "User")]
    [HttpPut("api/wishlist/{wishlistId:guid}")]
    public async Task<IActionResult> UpdateWishlist(Guid wishlistId, [FromBody] UpdateWishlistDto updateWishlistDto)
    {
        var wishlist = await _service.Wishlist.GetAsync(wishlistId);

        // verify that the request user id matches the user id of the wishlist
        if (JwtUtils.ParseFromPayload(JwtUtils.Parse(HttpContext.Request.Headers.Authorization), "UserId") != wishlist.UserId.ToString())
        {
            throw new NotAuthorized401Exception();
        }

        var books = new List<Book>();

        foreach (var bookId in updateWishlistDto.BookIds)
        {
            var book = await _service.Book.GetAsync(bookId);
            books.Add(book);
        }

        wishlist.Update(updateWishlistDto, books);
        await _service.Wishlist.UpdateAsync(wishlist);

        return NoContent();
    }

    [Authorize(Policy = "User")]
    [HttpDelete("api/wishlist/{wishlistId:guid}")]
    public async Task<IActionResult> DeleteWishlist(Guid wishlistId)
    {
        var wishlist = await _service.Wishlist.GetAsync(wishlistId);

        // verify that the request user id matches the user id of the wishlist
        if (JwtUtils.ParseFromPayload(JwtUtils.Parse(HttpContext.Request.Headers.Authorization), "UserId") != wishlist.UserId.ToString())
        {
            throw new NotAuthorized401Exception();
        }

        await _service.Wishlist.DeleteAsync(wishlist);
        return NoContent();
    }
}
