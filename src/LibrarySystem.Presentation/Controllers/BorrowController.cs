﻿using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/**

GET     /api/borrow
POST    /api/borrow

**/
public class BorrowController : ControllerBase
{
    private readonly IServiceManager _service;

    public BorrowController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("api/borrow")]
    public async Task<IActionResult> GetBorrows(int limit, int offset)
    {
        var borrows = await _service.Borrow.GetAll();

        if (offset > 0)
            borrows = borrows.Skip(offset);
            
        if (limit > 0)
            borrows = borrows.Take(limit); 

        return Ok(borrows.Select(b => b.ToDto()));
    }

    [HttpPost("api/borrow")]
    public async Task<IActionResult> CreateBorrow([FromBody] CreateBorrowDto createBorrowDto)
    {
        var borrow = await _service.Borrow.Create(createBorrowDto);

        return Created(string.Format("/api/borrow/{0}", borrow.Id), null);
    }
}
