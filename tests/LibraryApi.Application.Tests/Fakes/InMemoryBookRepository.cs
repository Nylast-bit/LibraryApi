using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;

namespace LibraryApi.Application.Tests.Fakes;

/// <summary>
/// Repositorio falso en memoria. Imita a FakeRestAPI: las escrituras responden con
/// la entidad recibida pero no la guardan. Registra las llamadas para poder verificarlas.
/// </summary>
public sealed class InMemoryBookRepository(params Book[] books) : IBookRepository
{
    private readonly List<Book> _books = [.. books];

    public List<int> DeletedIds { get; } = [];
    public List<Book> UpdatedBooks { get; } = [];

    public Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Book>>(_books);

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

    public Task<Book> AddAsync(Book entity, CancellationToken cancellationToken = default) =>
        Task.FromResult(entity);

    public Task<Book> UpdateAsync(Book entity, CancellationToken cancellationToken = default)
    {
        UpdatedBooks.Add(entity);
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        DeletedIds.Add(id);
        return Task.CompletedTask;
    }
}
