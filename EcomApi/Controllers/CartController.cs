using EcomApi.DTOs;
using EcomApi.InMemory;
using EcomApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")!.Value);

    [HttpGet]
    public ActionResult<IEnumerable<object>> GetMyCart()
    {
        var items = StaticStore.CartItems.Where(ci => ci.UserId == CurrentUserId)
            .Select(ci => new
            {
                ci.Id,
                ci.ProductId,
                product = StaticStore.Products.First(p => p.Id == ci.ProductId),
                ci.Quantity,
                ci.AddedAt
            });
        return Ok(items);
    }

    [HttpPost("add")]
    public IActionResult Add(AddToCartDto dto)
    {
        var prod = StaticStore.Products.FirstOrDefault(p => p.Id == dto.ProductId);
        if (prod is null) return NotFound("Producto no existe.");
        if (dto.Quantity <= 0) return BadRequest("Cantidad inválida.");

        var existing = StaticStore.CartItems.FirstOrDefault(ci => ci.UserId == CurrentUserId && ci.ProductId == dto.ProductId);
        if (existing is null)
        {
            StaticStore.CartItems.Add(new CartItem
            {
                Id = StaticStore.CartSeq++,
                UserId = CurrentUserId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            });
        }
        else
        {
            existing.Quantity += dto.Quantity;
        }
        return Ok();
    }

    [HttpPut("{cartItemId:int}")]
    public IActionResult UpdateQuantity(int cartItemId, [FromQuery] int quantity)
    {
        var item = StaticStore.CartItems.FirstOrDefault(ci => ci.Id == cartItemId && ci.UserId == CurrentUserId);
        if (item is null) return NotFound();
        if (quantity <= 0) { StaticStore.CartItems.Remove(item); return NoContent(); }
        item.Quantity = quantity;
        return Ok(item);
    }

    [HttpDelete("{cartItemId:int}")]
    public IActionResult Remove(int cartItemId)
    {
        var item = StaticStore.CartItems.FirstOrDefault(ci => ci.Id == cartItemId && ci.UserId == CurrentUserId);
        if (item is null) return NotFound();
        StaticStore.CartItems.Remove(item);
        return NoContent();
    }

    [HttpDelete("clear")]
    public IActionResult Clear()
    {
        StaticStore.CartItems.RemoveAll(ci => ci.UserId == CurrentUserId);
        return NoContent();
    }
}
