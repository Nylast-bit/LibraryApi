using System.Text.Json.Serialization;

namespace LibraryApi.Infrastructure.ExternalApi.Models;

public record AuthorApiModel(
    int Id,
    [property: JsonPropertyName("idBook")] int BookId,
    string? FirstName,
    string? LastName
);