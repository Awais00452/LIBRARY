/*
Name       : Rana Muhammad Awais
StudentID  : 40742404
Module code: EN8107
*/

using System;
using System.Collections.Generic;
using System.Linq;

// User class for library members
public class User
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public List<Book> BorrowedBooks { get; private set; } = new();

    public User(string userId, string name)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("User ID cannot be empty.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("User name cannot be empty.");
        UserId = userId;
        Name = name;
    }

    // Borrow a book if not already borrowed
    public bool BorrowBook(Book book)
    {
        if (book == null) throw new ArgumentNullException();
        if (BorrowedBooks.Any(b => b.ISBN == book.ISBN)) return false;
        BorrowedBooks.Add(book);
        return true;
    }

    // Return a book if it was borrowed
    public bool ReturnBook(Book book)
    {
        if (book == null) throw new ArgumentNullException();
        var toRemove = BorrowedBooks.FirstOrDefault(b => b.ISBN == book.ISBN);
        if (toRemove == null) return false;
        BorrowedBooks.Remove(toRemove);
        return true;
    }
}
