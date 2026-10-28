
using LibraryApi.Application.DTOs.Authors;

namespace LibraryApi.Application.DTOs.Books;

public record BookDetailResponse(
    int Id,
    string Title,
    string? Description,
    int PageCount,
    string? Excerpt,
    DateTimeOffset PublishDate,
    IReadOnlyList<AuthorSummaryResponse> Authors
);
