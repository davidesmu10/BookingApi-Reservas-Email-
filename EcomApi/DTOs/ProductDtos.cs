namespace EcomApi.DTOs;

public record CategoryCreateDto(string Name, string? Description);
public record ProductCreateDto(string Name, string? Description, decimal Price, int Stock, int CategoryId);
public record ProductUpdateDto(string? Name, string? Description, decimal? Price, int? Stock, int? CategoryId);
