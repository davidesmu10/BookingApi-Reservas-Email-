namespace EcomApi.DTOs;

public record AddToCartDto(int ProductId, int Quantity);
public record CheckoutRequest(string CardNumber);
public record OrderFilterDto(string? Status, DateTime? From, DateTime? To, int? Page = 1, int? PageSize = 20);
