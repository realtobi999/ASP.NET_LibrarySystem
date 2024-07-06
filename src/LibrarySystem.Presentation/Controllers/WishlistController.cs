using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain;
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

    [Authorize(Policy = "User")]
    [HttpGet("api/wishlist/{wishlistId:guid}")]
    public async Task<IActionResult> GetWishlist(Guid wishlistId)
    {
        var wishlist = await _service.Wishlist.Get(wishlistId);

        // verify that the request user id matches the user id of the wishlist
        if (Jwt.ParseFromPayload(Jwt.Parse(HttpContext.Request.Headers.Authorization), "UserId") != wishlist.UserId.ToString())
            throw new NotAuthorizedException("Not Authorized!");

        return Ok(wishlist.ToDto());    
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPost("api/wishlist")]
    public async Task<IActionResult> CreateWishlist([FromBody] CreateWishlistDto createWishlistDto)
    {
        var wishlist = await _service.Wishlist.Create(createWishlistDto);

        return Created(string.Format("/api/wishlist/{0}", wishlist.Id), null);
    }
}
