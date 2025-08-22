using EcomApi.DTOs;
using EcomApi.InMemory;
using EcomApi.Models;
using EcomApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController(PaymentSimulator pay) : ControllerBase
{
    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")!.Value);

    [HttpPost("checkout")]
    public ActionResult<Order> Checkout(CheckoutRequest dto)
    {
        var cart = StaticStore.CartItems.Where(ci => ci.UserId == CurrentUserId).ToList();
        if (!cart.Any()) return BadRequest("Carrito vacío.");

        var items = new List<OrderItem>();
        decimal total = 0;
        foreach (var ci in cart)
        {
            var p = StaticStore.Products.FirstOrDefault(x => x.Id == ci.ProductId);
            if (p is null) return BadRequest($"Producto {ci.ProductId} no existe.");
            if (p.Stock < ci.Quantity) return BadRequest($"Sin stock suficiente: {p.Name}");
            total += p.Price * ci.Quantity;
            items.Add(new OrderItem { Id = StaticStore.OrderItemSeq++, ProductId = p.Id, Quantity = ci.Quantity, UnitPrice = p.Price });
        }

        var (approved, reference) = pay.Charge(dto.CardNumber, total);
        if (!approved) return BadRequest("Pago rechazado.");

        // descuenta stock
        foreach (var ci in cart)
        {
            var p = StaticStore.Products.First(x => x.Id == ci.ProductId);
            p.Stock -= ci.Quantity;
        }

        var order = new Order
        {
            Id = StaticStore.OrderSeq++,
            UserId = CurrentUserId,
            Total = total,
            Status = "Paid",
            Items = items,
            PaymentReference = reference
        };
        StaticStore.Orders.Add(order);
        StaticStore.CartItems.RemoveAll(ci => ci.UserId == CurrentUserId);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet]
    public ActionResult<object> MyOrders([FromQuery] string? status, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var q = StaticStore.Orders.Where(o => o.UserId == CurrentUserId).AsQueryable();
        if (!string.IsNullOrWhiteSpace(status)) q = q.Where(o => o.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        if (from.HasValue) q = q.Where(o => o.CreatedAt >= from.Value);
        if (to.HasValue) q = q.Where(o => o.CreatedAt <= to.Value);
        return Ok(q.OrderByDescending(o => o.CreatedAt).ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Order> GetById(int id)
    {
        var o = StaticStore.Orders.FirstOrDefault(o => o.Id == id && o.UserId == CurrentUserId);
        return o is null ? NotFound() : Ok(o);
    }

    [HttpPost("{id:int}/cancel")]
    public IActionResult Cancel(int id)
    {
        var o = StaticStore.Orders.FirstOrDefault(o => o.Id == id && o.UserId == CurrentUserId);
        if (o is null) return NotFound();
        if (o.Status != "Paid" && o.Status != "Pending") return BadRequest("No se puede cancelar.");
        o.Status = "Cancelled";
        return Ok(o);
    }
}
