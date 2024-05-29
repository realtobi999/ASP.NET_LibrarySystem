using LibrarySystem.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation;

[ApiController]
/*

GET     /api/staff params: offset, limit

*/
public class StaffController : ControllerBase
{
    private readonly IServiceManager _service;

    public StaffController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("api/staff")]
    public async Task<IActionResult> GetStaffs(int limit, int offset)
    {
        var staff = await _service.StaffService.GetAll();

        if (offset > 0)
            staff = staff.Skip(offset);
        if (limit > 0)
            staff = staff.Take(limit); 

        return Ok(staff);
    }
}
