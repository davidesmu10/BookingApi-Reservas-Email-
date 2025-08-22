using EcomApi.DTOs;
using EcomApi.InMemory;
using EcomApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Category>> GetAll() => Ok(StaticStore.Categories);

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult<Category> Create(CategoryCreateDto dto)
    {
        var c = new Category { Id = StaticStore.CategorySeq++, Name = dto.Name, Description = dto.Description };
        StaticStore.Categories.Add(c);
        return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Category> GetById(int id)
    {
        var c = StaticStore.Categories.FirstOrDefault(x => x.Id == id);
        return c is null ? NotFound() : Ok(c);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public ActionResult<Category> Update(int id, CategoryCreateDto dto)
    {
        var c = StaticStore.Categories.FirstOrDefault(x => x.Id == id);
        if (c is null) return NotFound();
        c.Name = dto.Name; c.Description = dto.Description;
        return Ok(c);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var c = StaticStore.Categories.FirstOrDefault(x => x.Id == id);
        if (c is null) return NotFound();
        StaticStore.Categories.Remove(c);
        return NoContent();
    }
}
