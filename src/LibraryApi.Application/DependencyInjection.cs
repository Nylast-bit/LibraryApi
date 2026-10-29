using LibraryApi.Application.Interfaces;
using LibraryApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();

        return services;
    }
}