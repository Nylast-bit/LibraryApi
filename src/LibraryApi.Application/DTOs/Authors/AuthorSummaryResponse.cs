namespace LibraryApi.Application.DTOs.Authors;

public record AuthorSummaryResponse(int Id, string FirstName, string LastName)
{
    public string FullName => $"{FirstName} {LastName}";
}