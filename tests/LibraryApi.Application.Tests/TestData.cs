using LibraryApi.Domain.Entities;

namespace LibraryApi.Application.Tests;

internal static class TestData
{
    public static Book Book(int id, string title = "Libro de prueba") =>
        new()
        {
            Id = id,
            Title = title,
            Description = "Descripción",
            PageCount = 100,
            Excerpt = "Fragmento",
            PublishDate = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero)
        };

    public static Author Author(int id, int bookId, string firstName, string lastName) =>
        new()
        {
            Id = id,
            BookId = bookId,
            FirstName = firstName,
            LastName = lastName
        };
}
