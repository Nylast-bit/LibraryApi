namespace LibraryApi.Application.DTOs.Books;

public record BookResponse(
    int Id,
    string Title,
    string? Description,
    int PageCount,
    string? Excerpt,
    DateTimeOffset PublishDate
);