namespace BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;


public class DeleteBook : IActions
{
    private readonly Dictionary<int,Action> choices = new Dictionary<int, Action> { { 1, DeleteBookById},{2,DeleteBookByTitle} };
    public void PerformAction()
    {
        Console.WriteLine("1. Delete book by Id");
        Console.WriteLine("2. Delete book by Title");
        Console.WriteLine("3. Back to Main Menu");
        int choice = ToolBox.ReadInt("Enter operation : ");
        choices[choice].Invoke();
    }
    private static readonly Action DeleteBookByTitle = () => 
    {
        string title = ToolBox.ReadNonEmpty("Title: ");

        int removedBooks = Database.GetAllBooks().RemoveAll(book => book.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase));

        if (removedBooks == 0)
        {
            Console.WriteLine("No book found with that Title");
            return;
        }

        Console.WriteLine($"{removedBooks}books removed from the library");
    };
    private static readonly Action DeleteBookById = () => 
    {
        uint id = ToolBox.ReadUInt("Id: ");
        Book? book =  Database.GetBookById(id);
        if (book == null)
        {
            Console.WriteLine("No book found with that id");
            return;
        }
        Database.RemoveBook(book.Id);
        Console.WriteLine($"Book {book.Title} deleted");
    };
}
