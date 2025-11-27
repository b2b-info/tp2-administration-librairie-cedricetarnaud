namespace BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;


public class DeleteBook : Operations
{
    private string _deletionInformation = "default";

    public override void PerformAction()
    {
        Console.WriteLine("1. Delete book by Id");
        Console.WriteLine("2. Delete book by Title");
        Console.WriteLine("3. Back to Main Menu");
        int choice = ToolBox.ReadInt("Enter operation : ");
        if (choice == 1)
        {
            SetDeleteInfo("Id");
        }
        else if (choice == 2)
        {
            SetDeleteInfo("Title");
        }
        else
        {
            //We gtfo mate
        }
    }
    
    private void SetDeleteInfo(string DemandedDelete)
    {
        _deletionInformation = ToolBox.ReadNonEmpty($"{DemandedDelete} : ");

        //int removedBooks = Database.GetAllBooks().RemoveAll(book => book.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase));

        //if (removedBooks == 0)
        //{
        //    Console.WriteLine("No book found with that Title");
        //    return;
        //}

        //Console.WriteLine($"{removedBooks}books removed from the library");
    }
    public override void Product()
    {
        if (uint.TryParse(_deletionInformation, out uint result))
        {
            Book? book = Database.GetBookById(result);
            if (book == null)
            {
                Console.WriteLine("No book found with that id");
                return;
            }
            Database.RemoveBook(book.Id);
            Console.WriteLine($"Book {book.Title} deleted");
        }
        else
        {
            int removedBooks = Database.GetAllBooks().RemoveAll(book => book.Title.Equals(_deletionInformation, StringComparison.CurrentCultureIgnoreCase));
            if (removedBooks == 0)
            {
                Console.WriteLine("No book found with that Title");
                return;
            }
            Console.WriteLine($"{removedBooks}books removed from the library");
        }
    }
   
}
