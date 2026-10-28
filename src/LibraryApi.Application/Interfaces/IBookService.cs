using LibraryApi.Application.DTOs.Books;

namespace LibraryApi.Application.Interfaces;

public interface IBookService
{
    Task<IReadOnlyList<BookResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookDetailResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken = default);
    Task<BookResponse?> UpdateAsync(int id, UpdateBookRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}