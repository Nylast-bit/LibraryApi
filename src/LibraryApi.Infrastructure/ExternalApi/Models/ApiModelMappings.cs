using LibraryApi.Domain.Entities;

namespace LibraryApi.Infrastructure.ExternalApi.Models;

public static class ApiModelMappings
{
    public static Author ToDomain(this AuthorApiModel model) =>
        new()
        {
            Id = model.Id,
            BookId = model.BookId,
            FirstName = model.FirstName ?? string.Empty,
            LastName = model.LastName ?? string.Empty
        };

    public static AuthorApiModel ToApiModel(this Author author) =>
        new(
            author.Id,
            author.BookId,
            author.FirstName,
            author.LastName
        );

    public static Book ToDomain(this BookApiModel model) =>
        new()
        {
            Id = model.Id,
            Title = model.Title ?? "No Title",
            Description = model.Description,
            PageCount = model.PageCount,
            Excerpt = model.Excerpt,
            PublishDate = model.PublishDate
        };

    public static BookApiModel ToApiModel(this Book book) =>
        new(
            book.Id,
            book.Title,
            book.Description,
            book.PageCount,
            book.Excerpt,
            book.PublishDate
        );
}