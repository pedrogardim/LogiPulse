namespace LogiPulse.Application.ProductCategories.Queries.ListProductCategories;

public record ListProductCategoriesItemResponse(
    Guid Id,
    string ExternalId,
    string Name
);