using LibrarySystem.Application;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Dtos.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

POST    /api/book/review

*/
public class BookReviewController : ControllerBase
{
    private readonly IServiceManager _service;

    public BookReviewController(IServiceManager service)
    {
        _service = service;
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPost("/api/book/review")]
    public async Task<IActionResult> CreateBookReview([FromBody] CreateBookReviewDto createBookReviewDto)
    {
        var _ = await _service.BookReview.Create(createBookReviewDto);

        return StatusCode(201);
    }
}
