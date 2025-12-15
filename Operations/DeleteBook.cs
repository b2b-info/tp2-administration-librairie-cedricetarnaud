    namespace BookStore;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        bool choiceIsValid = true;
        do
        {
            Console.WriteLine("1. Delete book by Id");
            Console.WriteLine("2. Delete book by Title");
            Console.WriteLine("3. Back to Main Menu");
            int choice = ToolBox.ReadInt("Enter operation : ");
            if (choice == 1)
            {
                _deletionInformation = ToolBox.ReadUInt("Id : ").ToString();
                await Program.Produce(this, "Delete book by id in queue");
                operationsStates = OperationsStates.Queued;
            }
            else if (choice == 2)
            {
                await Program.Produce(this, "Delete book by title in queue");
                operationsStates = OperationsStates.Queued;

            }
            else if (choice > 3)
            {
                Console.WriteLine("Please pick a valid option");
                choiceIsValid = false;
            }

        } while (!choiceIsValid);

    }
    private  void ExecuteQueuedState()
    {
        Program.logger.LogInformation("Deleting book...");
        Stopwatch stopwatch = Stopwatch.StartNew();
        if (uint.TryParse(_deletionInformation, out uint result))
        {
            Book? book = Database.GetBookById(result);
            if (book == null)
            {
                Console.WriteLine("No book found with that id");
                return;
            }
            Database.RemoveBook(book.Id);
        }
        else
        {
            int removedBooks = Database.GetAllBooks().RemoveAll(book => book.Title.Equals(_deletionInformation, StringComparison.CurrentCultureIgnoreCase));
            if (removedBooks == 0)
            {
                Console.WriteLine("No book found with that Title");
                return;
            }
            Console.WriteLine($"{removedBooks}_books removed from the library");
        }
        stopwatch.Stop();
        Program.logger.LogInformation($"Book deleted in {stopwatch.ElapsedMilliseconds} milliseconds");
    }
   
}
