using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase {
    private readonly TagService _tagService;



    public TagController(TagService tagService) {
        _tagService = tagService;
    }





    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _tagService.GetAllAsync());






    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id) {
        var tag = await _tagService.GetByIdAsync(id);
        return tag == null ? NotFound() : Ok(tag);
    }






    [HttpPost]
    [Route("upload")]

    public async Task<IActionResult> Create([FromBody] CreateTagRequest tagDto) {
        await _tagService.CreateAsync(tagDto.Name);
        return NoContent();
    }






    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] string name) {
        await _tagService.UpdateAsync(id, name);
        return NoContent();
    }






    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) {
        var success = await _tagService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
