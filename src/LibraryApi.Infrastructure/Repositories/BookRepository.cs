using System.Net;
using System.Net.Http.Json;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Infrastructure.ExternalApi.Models;

namespace LibraryApi.Infrastructure.Repositories;

public class BookRepository(HttpClient httpClient) : IBookRepository
{
    private const string BooksEndpoint = "api/v1/Books";

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var models = await httpClient.GetFromJsonAsync<List<BookApiModel>>(BooksEndpoint, cancellationToken);

        return (models ?? []).Select(m => m.ToDomain()).ToList();
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync($"{BooksEndpoint}/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<BookApiModel>(cancellationToken);
        return model?.ToDomain();
    }

    public async Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        var model = book.ToApiModel();

        using var response = await httpClient.PostAsJsonAsync(BooksEndpoint, model, cancellationToken);
        response.EnsureSuccessStatusCode();

        var createdModel = await response.Content.ReadFromJsonAsync<BookApiModel>(cancellationToken)
            ?? throw new InvalidOperationException("The external API returned an empty response when creating a book.");

        return createdModel.ToDomain();
    }

    public async Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        var model = book.ToApiModel();

        using var response = await httpClient.PutAsJsonAsync($"{BooksEndpoint}/{book.Id}", model, cancellationToken);
        response.EnsureSuccessStatusCode();

        var updatedModel = await response.Content.ReadFromJsonAsync<BookApiModel>(cancellationToken);
        return updatedModel!.ToDomain();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.DeleteAsync($"{BooksEndpoint}/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}