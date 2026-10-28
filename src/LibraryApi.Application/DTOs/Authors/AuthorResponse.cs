namespace LibraryApi.Application.DTOs.Authors;

public record AuthorResponse(
    int Id,
    int BookId,
    string FirstName,
    string LastName,
    int NumberOfBooks
)
{
    public string FullName => $"{FirstName} {LastName}";
};