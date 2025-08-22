﻿namespace EcomApi.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public string Status { get; set; } = "Pending"; // Pending | Paid | Cancelled
    public List<OrderItem> Items { get; set; } = new();
    public string? PaymentReference { get; set; }
}