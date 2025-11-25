namespace BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;


internal class DeleteBook : IActions
{
    public void PerformAction()
    {
        
    }
    private Action DeleteBookByTitle = () => 
    {
        string title = ToolBox.ReadNonEmpty("Title: ");



        if (books.Count == 0)
        {
            Console.WriteLine("No book found with that Title");
            return;
        }

        foreach (var book in books)
        {
            Database.RemoveBook(book.Id);
            Console.WriteLine($"Book {book.Title} deleted");
        }
    };
    private Action DeleteBookById = () => 
    {
        string title = ToolBox.ReadNonEmpty("Title: ");



        if (books.Count == 0)
        {
            Console.WriteLine("No book found with that Title");
            return;
        }

        foreach (var book in books)
        {
            Database.RemoveBook(book.Id);
            Console.WriteLine($"Book {book.Title} deleted");
        }
    };
}
