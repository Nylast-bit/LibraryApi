using LibraryApi.Application.DTOs.Books;
using LibraryApi.Application.Interfaces;

namespace LibraryApi.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task<IReadOnlyList<BookResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var books = await _bookRepository.GetAllAsync(cancellationToken);

        return books.Select(b => b.ToResponse()).ToList();
    }

    public async Task<BookDetailResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
        if (book is null) return null;

        var authors = await _authorRepository.GetByBookIdAsync(id, cancellationToken);

        return book.ToDetailResponse(authors);
    }

    public async Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken = default)
    {

        var createdBook = await _bookRepository.AddAsync(request.ToEntity(), cancellationToken);
        return createdBook.ToResponse();
    }

    public async Task<BookResponse?> UpdateAsync(int id, UpdateBookRequest request, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
        if (book is null) return null;

        var updatedBook = await _bookRepository.UpdateAsync(request.ToEntity(id), cancellationToken);
        return updatedBook.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
        if (book is null) return false;

        await _bookRepository.DeleteAsync(id, cancellationToken);
        return true;
    }
}