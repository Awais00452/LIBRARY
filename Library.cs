using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Library
{
    public Dictionary<string, Book> Books { get; private set; } = new();
    public Dictionary<string, User> Users { get; private set; } = new();
    private Dictionary<string, int> borrowCounts = new();

    public bool AddBook(Book book)
    {
        if (book == null || Books.ContainsKey(book.ISBN)) return false;
        Books[book.ISBN] = book;
        return true;
    }

    public bool RemoveBook(string isbn)
    {
        return Books.Remove(isbn);
    }

    public List<Book> SearchByTitle(string title)
    {
        return Books.Values.Where(b => b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }

    public List<Book> SearchByAuthor(string author)
    {
        return Books.Values.Where(b => b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }

    public List<Book> SearchByAvailability(bool isAvailable)
    {
        return Books.Values.Where(b => b.IsAvailable == isAvailable).ToList();
    }

    public bool AddUser(User user)
    {
        if (user == null || Users.ContainsKey(user.UserId)) return false;
        Users[user.UserId] = user;
        return true;
    }

    public bool RemoveUser(string userId)
    {
        return Users.Remove(userId);
    }

    public string? BorrowBook(string userId, string isbn)
    {
        if (!Users.ContainsKey(userId)) return "User not found.";
        if (!Books.ContainsKey(isbn)) return "Book not found.";
        var book = Books[isbn];
        if (!book.IsAvailable) return $"Book '{book.Title}' is not available.";
        var user = Users[userId];
        if (!user.BorrowBook(book)) return "User already borrowed this book.";
        book.IsAvailable = false;
        if (!borrowCounts.ContainsKey(isbn)) borrowCounts[isbn] = 0;
        borrowCounts[isbn]++;
        return null;
    }

    public string? ReturnBook(string userId, string isbn)
    {
        if (!Users.ContainsKey(userId)) return "User not found.";
        if (!Books.ContainsKey(isbn)) return "Book not found.";
        var user = Users[userId];
        var book = Books[isbn];
        if (!user.ReturnBook(book)) return "User did not borrow this book.";
        book.IsAvailable = true;
        return null;
    }

    public void LoadBooks(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split('|');
            if (parts.Length == 4)
            {
                var book = new Book(parts[0], parts[1], parts[2], parts[3].Trim() == "true");
                AddBook(book);
            }
        }
    }

    public void LoadUsers(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split('|');
            if (parts.Length >= 2)
            {
                var user = new User(parts[0], parts[1]);
                if (parts.Length == 3 && !string.IsNullOrWhiteSpace(parts[2]))
                {
                    var borrowed = parts[2].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var isbn in borrowed)
                    {
                        if (Books.TryGetValue(isbn.Trim(), out var book) && book != null)
                        {
                            user.BorrowBook(book);
                            book.IsAvailable = false;
                        }
                    }
                }
                AddUser(user);
            }
        }
    }

    public void SaveBooks(string filePath)
    {
        var lines = Books.Values.Select(b => $"{b.Title}|{b.Author}|{b.ISBN}|{b.IsAvailable.ToString().ToLower()}");
        File.WriteAllLines(filePath, lines);
    }

    public void SaveUsers(string filePath)
    {
        var lines = Users.Values.Select(u => $"{u.UserId}|{u.Name}|{string.Join(",", u.BorrowedBooks.Select(b => b.ISBN))}");
        File.WriteAllLines(filePath, lines);
    }

    public List<(Book book, int count)> MostBorrowed(int topN = 5)
    {
        return borrowCounts.OrderByDescending(kv => kv.Value)
            .Take(topN)
            .Select(kv => (Books[kv.Key], kv.Value))
            .ToList();
    }
}
