using LibraryApi.Application.DTOs.Books;
using LibraryApi.Application.Services;
using LibraryApi.Application.Tests.Fakes;

namespace LibraryApi.Application.Tests.Services;

public class BookServiceTests
{
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task GetAllAsync_ReturnsAllBooksMappedToResponses()
    {
        var books = new InMemoryBookRepository(TestData.Book(1, "Uno"), TestData.Book(2, "Dos"));
        var service = new BookService(books, new InMemoryAuthorRepository());

        var result = await service.GetAllAsync(Ct);

        Assert.Equal(2, result.Count);
        Assert.Equal("Uno", result[0].Title);
        Assert.Equal("Dos", result[1].Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenBookDoesNotExist()
    {
        var service = new BookService(new InMemoryBookRepository(), new InMemoryAuthorRepository());

        var result = await service.GetByIdAsync(99, Ct);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_IncludesOnlyTheAuthorsOfThatBook()
    {
        var books = new InMemoryBookRepository(TestData.Book(1));
        var authors = new InMemoryAuthorRepository(
            TestData.Author(1, bookId: 1, "Ana", "Pérez"),
            TestData.Author(2, bookId: 1, "Luis", "Gómez"),
            TestData.Author(3, bookId: 2, "Marta", "Díaz"));
        var service = new BookService(books, authors);

        var result = await service.GetByIdAsync(1, Ct);

        Assert.NotNull(result);
        Assert.Equal(2, result.Authors.Count);
        Assert.Contains(result.Authors, a => a.FullName == "Ana Pérez");
        Assert.Contains(result.Authors, a => a.FullName == "Luis Gómez");
    }

    [Fact]
    public async Task CreateAsync_ReturnsTheCreatedBook()
    {
        var service = new BookService(new InMemoryBookRepository(), new InMemoryAuthorRepository());
        var request = new CreateBookRequest("Nuevo libro", "Desc", 250, null, DateTimeOffset.UnixEpoch);

        var result = await service.CreateAsync(request, Ct);

        Assert.Equal("Nuevo libro", result.Title);
        Assert.Equal(250, result.PageCount);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNullAndDoesNotUpdate_WhenBookDoesNotExist()
    {
        var books = new InMemoryBookRepository();
        var service = new BookService(books, new InMemoryAuthorRepository());
        var request = new UpdateBookRequest("Título", null, 10, null, DateTimeOffset.UnixEpoch);

        var result = await service.UpdateAsync(99, request, Ct);

        Assert.Null(result);
        Assert.Empty(books.UpdatedBooks);
    }

    [Fact]
    public async Task UpdateAsync_UsesTheIdFromTheRoute()
    {
        var books = new InMemoryBookRepository(TestData.Book(1));
        var service = new BookService(books, new InMemoryAuthorRepository());
        var request = new UpdateBookRequest("Título editado", null, 10, null, DateTimeOffset.UnixEpoch);

        var result = await service.UpdateAsync(1, request, Ct);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Título editado", result.Title);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenBookDoesNotExist()
    {
        var books = new InMemoryBookRepository();
        var service = new BookService(books, new InMemoryAuthorRepository());

        var deleted = await service.DeleteAsync(99, Ct);

        Assert.False(deleted);
        Assert.Empty(books.DeletedIds);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletes_WhenBookExists()
    {
        var books = new InMemoryBookRepository(TestData.Book(1));
        var service = new BookService(books, new InMemoryAuthorRepository());

        var deleted = await service.DeleteAsync(1, Ct);

        Assert.True(deleted);
        Assert.Equal([1], books.DeletedIds);
    }
}
