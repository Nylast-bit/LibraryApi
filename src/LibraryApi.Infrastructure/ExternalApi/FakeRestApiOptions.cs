namespace LibraryApi.Infrastructure.ExternalApi;

public sealed class FakeRestApiOptions
{
    public const string SectionName = "FakeRestApi";

    public required string BaseUrl { get; init; }
}