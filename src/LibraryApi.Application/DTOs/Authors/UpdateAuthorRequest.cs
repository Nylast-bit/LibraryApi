namespace LibraryApi.Application.DTOs.Authors;

public record UpdateAuthorRequest(
    int BookId,
    string FirstName,
    string LastName
);