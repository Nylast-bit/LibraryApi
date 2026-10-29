using LibraryApi.Application.DTOs.Authors;
using LibraryApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController(IAuthorService authorService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuthorResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var authors = await authorService.GetAllAsync(cancellationToken);
        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var author = await authorService.GetByIdAsync(id, cancellationToken);
        return author is null ? NotFound() : Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorResponse>> Create(CreateAuthorRequest request, CancellationToken cancellationToken)
    {
        var created = await authorService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuthorResponse>> Update(int id, UpdateAuthorRequest request, CancellationToken cancellationToken)
    {
        var updated = await authorService.UpdateAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await authorService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}