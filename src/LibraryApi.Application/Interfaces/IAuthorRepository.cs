using LibraryApi.Domain.Entities;

namespace LibraryApi.Application.Interfaces;

public interface IAuthorRepository : IRepository<Author>
{
    Task<IReadOnlyList<Author>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken = default);

}