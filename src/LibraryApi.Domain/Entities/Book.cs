namespace LibraryApi.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int PageCount { get; set; }
    public string? Excerpt { get; set; }
    public DateTimeOffset PublishDate { get; set; } //Guarda la hora con a zona horaria
}