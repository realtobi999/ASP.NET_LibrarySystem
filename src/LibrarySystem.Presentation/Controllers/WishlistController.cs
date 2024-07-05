using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Dtos.Wishlists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/**

POST    /api/wishlist

**/
public class WishlistController : ControllerBase
{
    private readonly IServiceManager _service;

    public WishlistController(IServiceManager service)
    {
        _service = service;
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPost("api/wishlist")]
    public async Task<IActionResult> CreateWishlist([FromBody] CreateWishlistDto createWishlistDto)
    {
        var wishlist = await _service.Wishlist.Create(createWishlistDto);

        return Created(string.Format("/api/wishlist/{0}", wishlist.Id), null);
    }
}
