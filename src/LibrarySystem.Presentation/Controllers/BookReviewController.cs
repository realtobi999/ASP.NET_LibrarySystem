using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

POST    /api/review
DELETE  /api/review/{review_id}
PUT     /api/review/{review_id}

*/
public class BookReviewController : ControllerBase
{
    private readonly IServiceManager _service;

    public BookReviewController(IServiceManager service)
    {
        _service = service;
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPost("api/review")]
    public async Task<IActionResult> CreateBookReview([FromBody] CreateBookReviewDto createBookReviewDto)
    {
        var _ = await _service.BookReview.Create(createBookReviewDto);

        return StatusCode(201);
    }

    [Authorize(Policy = "User")]
    [HttpDelete("api/review/{reviewId:guid}")]
    public async Task<IActionResult> DeleteBookReview(Guid reviewId)
    {
        var review = await _service.BookReview.Get(reviewId); 

        // verify that the request user id matches the user id of the review
        if (Jwt.ParseFromPayload(Jwt.Parse(HttpContext.Request.Headers.Authorization), "UserId") != review.UserId.ToString())
            throw new NotAuthorizedException("Not Authorized!");

        var affected = await _service.BookReview.Delete(review);
        
        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    }

    [Authorize(Policy = "User")]
    [HttpPut("api/review/{reviewId:guid}")]
    public async Task<IActionResult> UpdateBookReview(Guid reviewId, UpdateBookReviewDto updateBookReviewDto)
    {
        var review = await _service.BookReview.Get(reviewId); 

        // verify that the request user id matches the user id of the review
        if (Jwt.ParseFromPayload(Jwt.Parse(HttpContext.Request.Headers.Authorization), "UserId") != review.UserId.ToString())
            throw new NotAuthorizedException("Not Authorized!");

        var affected = await _service.BookReview.Update(review, updateBookReviewDto);
        
        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    }
}
