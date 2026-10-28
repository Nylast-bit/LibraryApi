using LibraryApi.Application.DTOs.Authors;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;

namespace LibraryApi.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IReadOnlyList<AuthorResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var authors = await _authorRepository.GetAllAsync(cancellationToken);

        var booksCountByName = authors
            .GroupBy(a => new { a.FirstName, a.LastName })
            .ToDictionary(
                g => g.Key,
                g => g.Select(a => a.BookId).Distinct().Count());

        return authors
            .Select(a => a.ToResponse(booksCountByName[new { a.FirstName, a.LastName }]))
            .ToList();
    }

    public async Task<AuthorResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken);
        if (author is null) return null;

        return await ToResponseWithCountAsync(author, cancellationToken);
    }

    public async Task<AuthorResponse> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken = default)
    {
        var created = await _authorRepository.AddAsync(request.ToEntity(), cancellationToken);

        return await ToResponseWithCountAsync(created, cancellationToken);
    }

    public async Task<AuthorResponse?> UpdateAsync(int id, UpdateAuthorRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _authorRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null) return null;

        var updated = await _authorRepository.UpdateAsync(request.ToEntity(id), cancellationToken);

        return await ToResponseWithCountAsync(updated, cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken);
        if (author is null) return false;

        await _authorRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    private async Task<AuthorResponse> ToResponseWithCountAsync(Author author, CancellationToken cancellationToken)
    {
        var allAuthors = await _authorRepository.GetAllAsync(cancellationToken);

        var numberOfBooks = allAuthors
            .Where(a => a.FirstName == author.FirstName && a.LastName == author.LastName)
            .Select(a => a.BookId)
            .Append(author.BookId)
            .Distinct()
            .Count();

        return author.ToResponse(numberOfBooks);
    }
}