/*
Name       : Rana Muhammad Awais
StudentID  : 40742404
Module code: EN8107
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

public class LibraryTests
{
    [Fact]
    public void AddBook_ValidBook_AddsBook()
    {
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", true);
        library.AddBook(book);
        var user = new User("u1", "Alice");
        library.AddUser(user);
        var result = library.BorrowBook("u1", "1234567890");
        Assert.Null(result);
    }

    [Fact]
    public void AddBook_NullBook_DoesNotThrow()
    {
        var library = new Library();
        library.AddBook(null);
    }

    [Fact]
    public void AddUser_ValidUser_AddsUser()
    {
        var library = new Library();
        var user = new User("u1", "Alice");
        library.AddUser(user);
        var book = new Book("Test Title", "Test Author", "1234567890", true);
        library.AddBook(book);
        var result = library.BorrowBook("u1", "1234567890");
        Assert.Null(result);
    }

    [Fact]
    public void AddUser_NullUser_DoesNotThrow()
    {
        var library = new Library();
        library.AddUser(null);
    }

    [Fact]
    public void BorrowBook_Valid_Succeeds()
    {
        var library = new Library();
        var user = new User("u1", "Alice");
        var book = new Book("Test Title", "Test Author", "1234567890", true);
        library.AddUser(user);
        library.AddBook(book);
        var result = library.BorrowBook("u1", "1234567890");
        Assert.Null(result);
    }

    [Fact]
    public void BorrowBook_AlreadyBorrowed_Fails()
    {
        var library = new Library();
        var user = new User("u1", "Alice");
        var book = new Book("Test Title", "Test Author", "1234567890", true);
        library.AddUser(user);
        library.AddBook(book);
        library.BorrowBook("u1", "1234567890");
        var result = library.BorrowBook("u1", "1234567890");
        Assert.Equal("Book 'Test Title' is not available.", result);
    }

    [Fact]
    public void BorrowBook_InvalidUser_Fails()
    {
        var library = new Library();
        var book = new Book("Test Title", "Test Author", "1234567890", true);
        library.AddBook(book);
        var result = library.BorrowBook("invalid", "1234567890");
        Assert.Equal("User not found.", result);
    }

    [Fact]
    public void ReturnBook_Valid_Succeeds()
    {
        var library = new Library();
        var user = new User("u1", "Alice");
        var book = new Book("Test Title", "Test Author", "1234567890", true);
        library.AddUser(user);
        library.AddBook(book);
        library.BorrowBook("u1", "1234567890");
        var result = library.ReturnBook("u1", "1234567890");
        Assert.Null(result);
    }

    [Fact]
    public void ReturnBook_NotBorrowed_Fails()
    {
        var library = new Library();
        var user = new User("u1", "Alice");
        var book = new Book("Test Title", "Test Author", "1234567890", true);
        library.AddUser(user);
        library.AddBook(book);
        var result = library.ReturnBook("u1", "1234567890");
        Assert.Equal("User did not borrow this book.", result);
    }

    [Fact]
    public void LoadBooksFromFile_ValidFile_LoadsBooks()
    {
        var library = new Library();
        var filePath = Path.GetTempFileName();
        File.WriteAllText(filePath, "Book1|Author1|111|true\nBook2|Author2|222|false");
        library.LoadBooks(filePath);
        var user = new User("u1", "Alice");
        library.AddUser(user);
        var result = library.BorrowBook("u1", "111");
        Assert.Null(result);
        File.Delete(filePath);
    }

    [Fact]
    public void LoadBooksFromFile_InvalidFormat_HandlesError()
    {
        var library = new Library();
        var filePath = Path.GetTempFileName();
        File.WriteAllText(filePath, "InvalidLineWithoutEnoughFields");
        library.LoadBooks(filePath);
        File.Delete(filePath);
    }

    [Fact]
    public void SaveAndLoadUsers_FileIO_Works()
    {
        var library = new Library();
        var user = new User("u1", "Alice");
        user.BorrowBook(new Book("Book1", "Author1", "111", true));
        library.AddUser(user);
        var book = new Book("Book1", "Author1", "111", true);
        library.AddBook(book);
        var filePath = Path.GetTempFileName();
        library.SaveUsers(filePath);
        var library2 = new Library();
        library2.AddBook(book);
        library2.LoadUsers(filePath);
        var result = library2.BorrowBook("u1", "111");
        Assert.Equal("User already borrowed this book.", result);
        File.Delete(filePath);
    }

    [Fact]
    public void SaveAndLoadBooks_FileIO_Works()
    {
        var library = new Library();
        var book = new Book("Book1", "Author1", "111", true);
        library.AddBook(book);
        var filePath = Path.GetTempFileName();
        library.SaveBooks(filePath);
        var library2 = new Library();
        library2.LoadBooks(filePath);
        var user = new User("u1", "Alice");
        library2.AddUser(user);
        var result = library2.BorrowBook("u1", "111");
        Assert.Null(result);
        File.Delete(filePath);
    }
}

public class ScenarioTests
{
    [Fact]
    public void AssessmentScenarioTest()
    {
        // Arrange
        var inventoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inventory.txt");
        var usersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");
        var library = new Library();
        library.LoadBooks(inventoryPath);
        library.LoadUsers(usersPath);
        // Debug: Print Emily's borrowed books after loading
        var emilyBorrowed = string.Join(", ", library.Users["u202"].BorrowedBooks.Select(b => b.ISBN));
        Console.WriteLine($"Emily's borrowed books after loading: {emilyBorrowed}");

        // 2. Add the book "Good Omens" by Neil Gaiman and Terry Pratchett with ISBN 978-0060853982
        var goodOmens = new Book("Good Omens", "Neil Gaiman and Terry Pratchett", "978-0060853982", true);
        var added = library.AddBook(goodOmens);
        Assert.False(added); // Already exists in inventory.txt

        // 3. Create a user profile for a user called "Emily"
        var emily = new User("u202", "Emily");
        var userAdded = library.AddUser(emily);
        Assert.False(userAdded); // Emily already exists in users.txt

        // 4. Have Emily borrow "1984"
        var borrowResult = library.BorrowBook("u202", "9780451524935");
        Assert.Null(borrowResult);
        Assert.Contains(library.Books["9780451524935"], library.Users["u202"].BorrowedBooks);
        Assert.False(library.Books["9780451524935"].IsAvailable);

        // 5. Have Emily return "1984"
        var returnResult = library.ReturnBook("u202", "9780451524935");
        Assert.Null(returnResult);
        Assert.DoesNotContain(library.Books["9780451524935"], library.Users["u202"].BorrowedBooks);
        Assert.True(library.Books["9780451524935"].IsAvailable);

        // 6. Search for "Pride and Prejudice" in the library
        var searchResults = library.SearchByTitle("Pride and Prejudice");
        Assert.Contains(searchResults, b => b.Title == "Pride and Prejudice");

        // 7. Have David borrow "The Catcher in the Rye"
        var borrowDavid = library.BorrowBook("u101", "9780316769488");
        Assert.Null(borrowDavid);
        Assert.Contains(library.Books["9780316769488"], library.Users["u101"].BorrowedBooks);
        Assert.False(library.Books["9780316769488"].IsAvailable);

        // 8. Export the current data
        library.SaveBooks("test_inventory_out.txt");
        library.SaveUsers("test_users_out.txt");
        Assert.True(File.Exists("test_inventory_out.txt"));
        Assert.True(File.Exists("test_users_out.txt"));
    }
}
