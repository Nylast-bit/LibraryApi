using LibraryApi.Application.DTOs.Authors;
using LibraryApi.Application.Services;
using LibraryApi.Application.Tests.Fakes;

namespace LibraryApi.Application.Tests.Services;

public class AuthorServiceTests
{
    private static readonly CancellationToken Ct = CancellationToken.None;

    // Ana Pérez aparece en dos registros con libros distintos; Luis Gómez en uno.
    private static InMemoryAuthorRepository SeededRepository() =>
        new(
            TestData.Author(1, bookId: 10, "Ana", "Pérez"),
            TestData.Author(2, bookId: 20, "Ana", "Pérez"),
            TestData.Author(3, bookId: 10, "Luis", "Gómez"));

    [Fact]
    public async Task GetAllAsync_CountsDistinctBooksPerAuthorName()
    {
        var service = new AuthorService(SeededRepository());

        var result = await service.GetAllAsync(Ct);

        Assert.All(result.Where(a => a.FullName == "Ana Pérez"), a => Assert.Equal(2, a.NumberOfBooks));
        Assert.Equal(1, result.Single(a => a.FullName == "Luis Gómez").NumberOfBooks);
    }

    [Fact]
    public async Task GetAllAsync_DoesNotCountTheSameBookTwice()
    {
        var repository = new InMemoryAuthorRepository(
            TestData.Author(1, bookId: 10, "Ana", "Pérez"),
            TestData.Author(2, bookId: 10, "Ana", "Pérez"));
        var service = new AuthorService(repository);

        var result = await service.GetAllAsync(Ct);

        Assert.All(result, a => Assert.Equal(1, a.NumberOfBooks));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenAuthorDoesNotExist()
    {
        var service = new AuthorService(SeededRepository());

        var result = await service.GetByIdAsync(99, Ct);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTheAuthorWithItsBooksCount()
    {
        var service = new AuthorService(SeededRepository());

        var result = await service.GetByIdAsync(1, Ct);

        Assert.NotNull(result);
        Assert.Equal("Ana Pérez", result.FullName);
        Assert.Equal(2, result.NumberOfBooks);
    }

    [Fact]
    public async Task CreateAsync_CountsTheNewBook_EvenThoughTheApiDoesNotPersistIt()
    {
        var service = new AuthorService(SeededRepository());
        var request = new CreateAuthorRequest(30, "Ana", "Pérez");

        var result = await service.CreateAsync(request, Ct);

        Assert.Equal(3, result.NumberOfBooks);
    }

    [Fact]
    public async Task CreateAsync_NewAuthorHasOneBook()
    {
        var service = new AuthorService(SeededRepository());
        var request = new CreateAuthorRequest(50, "Nuevo", "Autor");

        var result = await service.CreateAsync(request, Ct);

        Assert.Equal(1, result.NumberOfBooks);
        Assert.Equal("Nuevo Autor", result.FullName);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenAuthorDoesNotExist()
    {
        var repository = SeededRepository();
        var service = new AuthorService(repository);

        var deleted = await service.DeleteAsync(99, Ct);

        Assert.False(deleted);
        Assert.Empty(repository.DeletedIds);
    }
}
