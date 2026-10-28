namespace LibraryApi.Domain.Entities;

public class Author
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}