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

    public override void ExecuteState()
    {
        switch (operationsStates)
        {
            case OperationsStates.Waiting:
                ExecuteWaitingState();
            break;
            case OperationsStates.Queued:
                ExecuteQueuedState();
            break;
        }
      
    }
    
    private async void ExecuteWaitingState()
    {
        Console.WriteLine("1. Delete book by Id");
        Console.WriteLine("2. Delete book by Title");
        Console.WriteLine("3. Back to Main Menu");
        int choice = ToolBox.ReadInt("Enter operation : ");
        if (choice == 1)
        {
            _deletionInformation =  ToolBox.ReadUInt("Id : ").ToString();
            await Program.Produce(this);
            operationsStates = OperationsStates.Queued;
        }
        else if (choice == 2)
        {
            await Program.Produce(this);
            operationsStates = OperationsStates.Queued;

        }
        else if(choice == 3)
        {
            //We gtfo mate
        }
        else
        {
            //C pas fcking bon man
        }

    }
    private  void ExecuteQueuedState()
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
