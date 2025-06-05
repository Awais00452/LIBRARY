/*
Name       : Rana Muhammad Awais
StudentID  : 40742404
Module code: EN8107
*/

// Book class inherits from Item for extensibility
public class Book : Item
{
    public string ISBN => Id; // For clarity in code using ISBN

    public Book(string title, string author, string isbn, bool isAvailable)
        : base(title, author, isbn, isAvailable)
    {
    }
}
