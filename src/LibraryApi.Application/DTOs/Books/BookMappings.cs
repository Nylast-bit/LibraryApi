using LibraryApi.Application.DTOs.Authors;
using LibraryApi.Domain.Entities;

namespace LibraryApi.Application.DTOs.Books;

public static class BookMappings
{
    public static BookResponse ToResponse(this Book book) =>
        new(
            book.Id,
            book.Title,
            book.Description,
            book.PageCount,
            book.Excerpt,
            book.PublishDate
        );

    public static BookDetailResponse ToDetailResponse(this Book book, IEnumerable<Author> authors) =>
        new(
            book.Id,
            book.Title,
            book.Description,
            book.PageCount,
            book.Excerpt,
            book.PublishDate,
            authors.Select(a => a.ToSummaryResponse()).ToList()
        );
}