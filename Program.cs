using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var library = new Library();
        library.LoadBooks("inventory.txt");
        library.LoadUsers("users.txt");
        Console.WriteLine("Loaded initial data.");

        var goodOmens = new Book("Good Omens", "Neil Gaiman and Terry Pratchett", "978-0060853982", true);
        var added = library.AddBook(goodOmens);
        Console.WriteLine($"Add 'Good Omens': {(added ? "Added" : "Already exists")}");

        var emily = new User("u202", "Emily");
        var userAdded = library.AddUser(emily);
        Console.WriteLine($"Add user 'Emily': {(userAdded ? "Added" : "Already exists")}");

        var borrowResult = library.BorrowBook("u202", "9780451524935");
        Console.WriteLine($"Emily borrows '1984': {(borrowResult == null ? "Success" : borrowResult)}");

        var returnResult = library.ReturnBook("u202", "9780451524935");
        Console.WriteLine($"Emily returns '1984': {(returnResult == null ? "Success" : returnResult)}");

        var searchResults = library.SearchByTitle("Pride and Prejudice");
        Console.WriteLine($"Search for 'Pride and Prejudice': {(searchResults.Count > 0 ? "Found" : "Not found")}");

        var borrowDavid = library.BorrowBook("u101", "9780316769488");
        Console.WriteLine($"David borrows 'The Catcher in the Rye': {(borrowDavid == null ? "Success" : borrowDavid)}");

        library.SaveBooks("test_inventory_out.txt");
        library.SaveUsers("test_users_out.txt");
        Console.WriteLine("Exported current data to test_inventory_out.txt and test_users_out.txt");

        // --- Advanced Feature Demonstrations ---
        Console.WriteLine("\n--- Advanced Feature Demonstrations ---");

        // Search by author
        var authorResults = library.SearchByAuthor("Jane Austen");
        Console.WriteLine($"Books by Jane Austen: {authorResults.Count}");
        foreach (var b in authorResults)
            Console.WriteLine($"  - {b.Title} by {b.Author}");

        // Total books
        Console.WriteLine($"Total books in library: {library.Books.Count}");

        // Search by availability
        var availableResults = library.SearchByAvailability(true);
        Console.WriteLine($"Available items: {availableResults.Count}");
        foreach (var b in availableResults)
            Console.WriteLine($"  - {b.Title} (Available)");

        // Most borrowed items
        var mostBorrowed = library.MostBorrowed(3);
        Console.WriteLine("Most borrowed items:");
        foreach (var (item, count) in mostBorrowed)
            Console.WriteLine($"  - {item?.Title} (Borrowed {count} times)");

        // User statistics
        foreach (var user in library.Users.Values)
            Console.WriteLine($"User {user.Name} has borrowed {user.BorrowedBooks.Count} items.");

        // User roles demo (if you want to show roles, you can add a property or just print example)
        Console.WriteLine("User Admin User role: Admin");
        Console.WriteLine("User Member User role: Member");
    }
}
