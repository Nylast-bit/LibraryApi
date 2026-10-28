using LibraryApi.Domain.Entities;

namespace LibraryApi.Application.DTOs.Authors;

public static class AuthorMappings
{
    public static AuthorResponse ToResponse(this Author author, int numberOfBooks) =>
        new(
            author.Id,
            author.BookId,
            author.FirstName,
            author.LastName,
            numberOfBooks
        );

    public static AuthorSummaryResponse ToSummaryResponse(this Author author) =>
        new(
            author.Id,
            author.FirstName,
            author.LastName
        );

    public static Author ToEntity(this CreateAuthorRequest request) =>
        new()
        {
            BookId = request.BookId,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

    public static Author ToEntity(this UpdateAuthorRequest request, int id) =>
        new()
        {
            Id = id,
            BookId = request.BookId,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
}