using System.Net;
using System.Net.Http.Json;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Infrastructure.ExternalApi.Models;

namespace LibraryApi.Infrastructure.Repositories;

public class AuthorRepository(HttpClient httpClient) : IAuthorRepository
{
    private const string AuthorsEndpoint = "api/v1/Authors";

    public async Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var models = await httpClient.GetFromJsonAsync<List<AuthorApiModel>>(AuthorsEndpoint, cancellationToken);

        return (models ?? []).Select(m => m.ToDomain()).ToList();
    }

    public async Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync($"{AuthorsEndpoint}/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<AuthorApiModel>(cancellationToken);
        return model?.ToDomain();
    }

    public async Task<IReadOnlyList<Author>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync($"{AuthorsEndpoint}/authors/books/{bookId}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound) return [];

        response.EnsureSuccessStatusCode();

        var models = await response.Content.ReadFromJsonAsync<List<AuthorApiModel>>(cancellationToken);
        return (models ?? []).Select(m => m.ToDomain()).ToList();
    }

    public async Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default)
    {
        var model = author.ToApiModel();

        using var response = await httpClient.PostAsJsonAsync(AuthorsEndpoint, model, cancellationToken);
        response.EnsureSuccessStatusCode();

        var createdModel = await response.Content.ReadFromJsonAsync<AuthorApiModel>(cancellationToken)
            ?? throw new InvalidOperationException("The external API returned an empty response when creating a Author.");

        return createdModel.ToDomain();
    }

    public async Task<Author> UpdateAsync(Author Author, CancellationToken cancellationToken = default)
    {
        var model = Author.ToApiModel();

        using var response = await httpClient.PutAsJsonAsync($"{AuthorsEndpoint}/{Author.Id}", model, cancellationToken);
        response.EnsureSuccessStatusCode();

        var updatedModel = await response.Content.ReadFromJsonAsync<AuthorApiModel>(cancellationToken);
        return updatedModel!.ToDomain();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.DeleteAsync($"{AuthorsEndpoint}/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}