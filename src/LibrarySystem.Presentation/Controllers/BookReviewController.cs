﻿using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Mappers;
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
    private readonly IBookReviewMapper _mapper;

    public BookReviewController(IServiceManager service, IBookReviewMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPost("api/review")]
    public async Task<IActionResult> CreateBookReview([FromBody] CreateBookReviewDto createBookReviewDto)
    {
        var review = _mapper.CreateFromDto(createBookReviewDto);

        await _service.BookReview.CreateAsync(review);

        return StatusCode(201);
    }

    [Authorize(Policy = "User")]
    [HttpDelete("api/review/{reviewId:guid}")]
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
    [HttpPut("api/review/{reviewId:guid}")]
    public async Task<IActionResult> UpdateBookReview(Guid reviewId, UpdateBookReviewDto updateBookReviewDto)
    {
        var review = await _service.BookReview.GetAsync(reviewId);

        // verify that the request user id matches the user id of the review
        if (JwtUtils.ParseFromPayload(JwtUtils.Parse(HttpContext.Request.Headers.Authorization), "UserId") != review.UserId.ToString())
        {
            throw new NotAuthorized401Exception();
        }

        _mapper.UpdateFromDto(review, updateBookReviewDto);

        await _service.BookReview.UpdateAsync(review);

        return NoContent();
    }
}
