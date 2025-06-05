/*
Name       : Rana Muhammad Awais
StudentID  : 40742404
Module code: EN8107
*/

// Base class for all library items (Book, DVD, CD, etc.)
public class Item
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Id { get; set; } // ISBN for books, or unique ID for other items
    public bool IsAvailable { get; set; }

    public Item(string title, string author, string id, bool isAvailable)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author cannot be empty.");
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("ID cannot be empty.");
        Title = title;
        Author = author;
        Id = id;
        IsAvailable = isAvailable;
    }
}
