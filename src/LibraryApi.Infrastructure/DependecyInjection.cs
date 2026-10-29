using LibraryApi.Application.Interfaces;
using LibraryApi.Infrastructure.ExternalApi;
using LibraryApi.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(FakeRestApiOptions.SectionName).Get<FakeRestApiOptions>()
            ?? throw new InvalidOperationException($"Missing '{FakeRestApiOptions.SectionName}' configuration section.");

        var baseAddress = new Uri(options.BaseUrl);

        services.AddHttpClient<IBookRepository, BookRepository>(client => client.BaseAddress = baseAddress);
        services.AddHttpClient<IAuthorRepository, AuthorRepository>(client => client.BaseAddress = baseAddress);

        return services;
    }
}