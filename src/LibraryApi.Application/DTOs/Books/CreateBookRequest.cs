namespace LibraryApi.Application.DTOs.Books;

public record CreateBookRequest(
    string Title,
    string? Description,
    int PageCount,
    string? Excerpt,
    DateTimeOffset PublishDate
);