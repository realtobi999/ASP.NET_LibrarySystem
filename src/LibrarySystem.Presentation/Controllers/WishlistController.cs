using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Exceptions.Common;
using LibrarySystem.Domain.Exceptions.HTTP;
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
        if (JwtUtils.ParseFromPayload(JwtUtils.Parse(HttpContext.Request.Headers.Authorization), "UserId") != wishlist.UserId.ToString())
            throw new NotAuthorized401Exception();

        return Ok(wishlist.ToDto());    
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPost("api/wishlist")]
    public async Task<IActionResult> CreateWishlist([FromBody] CreateWishlistDto createWishlistDto)
    {
        var wishlist = await _service.Wishlist.Create(createWishlistDto);

        return Created($"/api/wishlist/{wishlist.Id}", null);
    }

    [Authorize(Policy = "User")]
    [HttpPut("api/wishlist/{wishlistId:guid}")]
    public async Task<IActionResult> UpdateWishlist(Guid wishlistId, [FromBody] UpdateWishlistDto updateWishlistDto)
    {
        var wishlist = await _service.Wishlist.Get(wishlistId);

        // verify that the request user id matches the user id of the wishlist
        if (JwtUtils.ParseFromPayload(JwtUtils.Parse(HttpContext.Request.Headers.Authorization), "UserId") != wishlist.UserId.ToString())
            throw new NotAuthorized401Exception();

        var affected = await _service.Wishlist.Update(wishlist, updateWishlistDto);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    }

    [Authorize(Policy = "User")]
    [HttpDelete("api/wishlist/{wishlistId:guid}")]
    public async Task<IActionResult> DeleteWishlist(Guid wishlistId)
    {
        var wishlist = await _service.Wishlist.Get(wishlistId);

        // verify that the request user id matches the user id of the wishlist
        if (JwtUtils.ParseFromPayload(JwtUtils.Parse(HttpContext.Request.Headers.Authorization), "UserId") != wishlist.UserId.ToString())
            throw new NotAuthorized401Exception();

        var affected = await _service.Wishlist.Delete(wishlist);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    }
}
