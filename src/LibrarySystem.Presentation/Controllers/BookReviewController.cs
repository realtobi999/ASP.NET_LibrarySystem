using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
[Route("api/review")]
/*

POST    /api/review
DELETE  /api/review/{review_id}
PUT     /api/review/{review_id}

*/
public class BookReviewController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IMapperManager _mapper;

    public BookReviewController(IServiceManager service, IMapperManager mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPost("")]
    public async Task<IActionResult> CreateBookReview([FromBody] CreateBookReviewDto createBookReviewDto)
    {
        var review = _mapper.BookReview.Map(createBookReviewDto);

        await _service.BookReview.CreateAsync(review);

        return StatusCode(201);
    }

    [Authorize(Policy = "User")]
    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> DeleteBookReview(Guid reviewId)
    {
        var review = await _service.BookReview.GetAsync(reviewId);

        // verify that the request user id matches the user id of the review
        if (JwtUtils.ParseFromPayload(JwtUtils.Parse(HttpContext.Request.Headers.Authorization), "UserId") != review.UserId.ToString())
        {
            throw new NotAuthorized401Exception();
        }

        await _service.BookReview.DeleteAsync(review);

        return NoContent();
    }

    [Authorize(Policy = "User")]
    [HttpPut("{reviewId:guid}")]
    public async Task<IActionResult> UpdateBookReview(Guid reviewId, UpdateBookReviewDto updateBookReviewDto)
    {
        var review = await _service.BookReview.GetAsync(reviewId);

        // verify that the request user id matches the user id of the review
        if (JwtUtils.ParseFromPayload(JwtUtils.Parse(HttpContext.Request.Headers.Authorization), "UserId") != review.UserId.ToString())
        {
            throw new NotAuthorized401Exception();
        }

        review.Update(updateBookReviewDto);
        await _service.BookReview.UpdateAsync(review);

        return NoContent();
    }
}