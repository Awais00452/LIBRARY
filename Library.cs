/*
Name       : Rana Muhammad Awais
StudentID  : 40742404
Module code: EN8107
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Library class manages books and users
public class Library
{
    public Dictionary<string, Book> Books { get; private set; } = new();
    public Dictionary<string, User> Users { get; private set; } = new();
    private Dictionary<string, int> borrowCounts = new();

    // Add a new book to the library
    public bool AddBook(Book book)
    {
        if (book == null || Books.ContainsKey(book.ISBN)) return false;
        Books[book.ISBN] = book;
        return true;
    }

    // Remove a book by ISBN
    public bool RemoveBook(string isbn)
    {
        return Books.Remove(isbn);
    }

    // Search for books by title
    public List<Book> SearchByTitle(string title)
    {
        return Books.Values.Where(b => b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }

    // Search for books by author
    public List<Book> SearchByAuthor(string author)
    {
        return Books.Values.Where(b => b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }

    // Search for books by availability
    public List<Book> SearchByAvailability(bool isAvailable)
    {
        return Books.Values.Where(b => b.IsAvailable == isAvailable).ToList();
    }

    // Add a new user
    public bool AddUser(User user)
    {
        if (user == null || Users.ContainsKey(user.UserId)) return false;
        Users[user.UserId] = user;
        return true;
    }

    // Remove a user by ID
    public bool RemoveUser(string userId)
    {
        return Users.Remove(userId);
    }

    // Borrow a book for a user
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

    // Return a book for a user
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

    // Load books from a file
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

    // Load users from a file
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

    // Save books to a file
    public void SaveBooks(string filePath)
    {
        var lines = Books.Values.Select(b => $"{b.Title}|{b.Author}|{b.ISBN}|{b.IsAvailable.ToString().ToLower()}");
        File.WriteAllLines(filePath, lines);
    }

    // Save users to a file
    public void SaveUsers(string filePath)
    {
        var lines = Users.Values.Select(u => $"{u.UserId}|{u.Name}|{string.Join(",", u.BorrowedBooks.Select(b => b.ISBN))}");
        File.WriteAllLines(filePath, lines);
    }

    // Get most borrowed books
    public List<(Book book, int count)> MostBorrowed(int topN = 5)
    {
        return borrowCounts.OrderByDescending(kv => kv.Value)
            .Take(topN)
            .Select(kv => (Books[kv.Key], kv.Value))
            .ToList();
    }
}
