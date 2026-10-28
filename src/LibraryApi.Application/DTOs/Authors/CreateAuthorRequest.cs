namespace LibraryApi.Application.DTOs.Authors;

public record CreateAuthorRequest(
    int BookId,
    string FirstName,
    string LastName
);
