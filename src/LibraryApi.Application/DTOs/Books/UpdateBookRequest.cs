namespace LibraryApi.Application.DTOs.Books;

public record UpdateBookRequest(
    string Title,
    string? Description,
    int PageCount,
    string? Excerpt,
    DateTimeOffset PublishDate
);