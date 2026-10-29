using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;

namespace LibraryApi.Application.Tests.Fakes;

/// <summary>
/// Repositorio falso en memoria. Igual que FakeRestAPI, las escrituras no se guardan,
/// lo que permite probar que el conteo de libros funciona con autores recién creados.
/// </summary>
public sealed class InMemoryAuthorRepository(params Author[] authors) : IAuthorRepository
{
    private readonly List<Author> _authors = [.. authors];

    public List<int> DeletedIds { get; } = [];

    public Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Author>>(_authors);

    public Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_authors.FirstOrDefault(a => a.Id == id));

    public Task<IReadOnlyList<Author>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Author>>(_authors.Where(a => a.BookId == bookId).ToList());

    public Task<Author> AddAsync(Author entity, CancellationToken cancellationToken = default) =>
        Task.FromResult(entity);

    public Task<Author> UpdateAsync(Author entity, CancellationToken cancellationToken = default) =>
        Task.FromResult(entity);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        DeletedIds.Add(id);
        return Task.CompletedTask;
    }
}
