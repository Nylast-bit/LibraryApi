using System.Text.Json.Serialization;

namespace LibraryApi.Infrastructure.ExternalApi.Models;

public record BookApiModel(
    int Id,
    string Title,
    string? Description,
    int PageCount,
    string? Excerpt,
    DateTimeOffset PublishDate
);