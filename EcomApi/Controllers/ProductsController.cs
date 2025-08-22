using EcomApi.DTOs;
using EcomApi.InMemory;
using EcomApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public ActionResult<object> GetAll([FromQuery] int? categoryId, [FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var data = StaticStore.Products.AsQueryable();
        if (categoryId.HasValue) data = data.Where(p => p.CategoryId == categoryId.Value);
        if (!string.IsNullOrWhiteSpace(q)) data = data.Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
        var total = data.Count();
        var items = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return Ok(new { total, page, pageSize, items });
    }

    [HttpGet("{id:int}")]
    public ActionResult<Product> GetById(int id)
        => StaticStore.Products.FirstOrDefault(p => p.Id == id) is Product p ? Ok(p) : NotFound();

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult<Product> Create(ProductCreateDto dto)
    {
        if (!StaticStore.Categories.Any(c => c.Id == dto.CategoryId))
            return BadRequest("Categoría inválida.");
        var p = new Product
        {
            Id = StaticStore.ProductSeq++,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId
        };
        StaticStore.Products.Add(p);
        return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public ActionResult<Product> Update(int id, ProductUpdateDto dto)
    {
        var p = StaticStore.Products.FirstOrDefault(x => x.Id == id);
        if (p is null) return NotFound();
        if (dto.CategoryId.HasValue && !StaticStore.Categories.Any(c => c.Id == dto.CategoryId)) return BadRequest("Categoría inválida.");

        p.Name = dto.Name ?? p.Name;
        p.Description = dto.Description ?? p.Description;
        p.Price = dto.Price ?? p.Price;
        p.Stock = dto.Stock ?? p.Stock;
        p.CategoryId = dto.CategoryId ?? p.CategoryId;
        return Ok(p);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var p = StaticStore.Products.FirstOrDefault(x => x.Id == id);
        if (p is null) return NotFound();
        StaticStore.Products.Remove(p);
        return NoContent();
    }
}
