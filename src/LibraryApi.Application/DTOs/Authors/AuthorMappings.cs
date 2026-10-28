using LibraryApi.Domain.Entities;

namespace LibraryApi.Application.DTOs.Authors;

public static class AuthorMappings
{
    public static AuthorSummaryResponse ToSummaryResponse(this Author author) =>
        new(
            author.Id,
            author.FirstName,
            author.LastName
        );
}