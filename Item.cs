namespace LibraryBackend;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public bool IsAvailable { get; set; }

    public Book(string title, string author, string isbn, bool isAvailable)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author cannot be empty.");
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN cannot be empty.");
        Title = title;
        Author = author;
        ISBN = isbn;
        IsAvailable = isAvailable;
    }
}
