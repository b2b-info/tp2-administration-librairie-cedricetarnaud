using BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BookInformations : Operations
{
    private readonly Dictionary<int,Action> Operations = new Dictionary<int, Action> { {1, ShowBookDetailsById},{ 2,ShowBookCount}, {3,ShowAllBooks },{4,BackToMainMenu } };
    public override void ExecuteState()
    {
        if (operationsStates == OperationsStates.Waiting)
        {
            bool choiceInvalidValid = true;
            while (choiceInvalidValid)
            {
                Console.WriteLine("1. Show Book Details by Id");
                Console.WriteLine("2. Show Book Count");
                Console.WriteLine("3. Show All Books");
                Console.WriteLine("4. Back to Main Menu");
                int choice = ToolBox.ReadInt("Enter you operation: ");
                if (!Operations.ContainsKey(choice))
                {
                    Console.WriteLine("Invalid choice \n");
                    continue;
                }
                choiceInvalidValid = false;
                Operations[choice]?.Invoke();
            }
        }
      
    }

    private  static readonly Action ShowBookDetailsById = () => 
    {
        uint id = ToolBox.ReadUInt("Book id : ");

       Book? book = Database.GetBookById(id);
        if (book == null) 
        {
            Console.WriteLine("No book with that id");
            return;
        }

        Console.WriteLine(book.ShowDetailsCollum());
    };
    private  static readonly Action ShowBookCount = () => 
    {
        Console.WriteLine($"Total books : {Database.CountRecords()}");
    };
    private  static readonly Action ShowAllBooks = () => 
    {
        foreach ( Book book in Database.GetAllBooks() ) 
        {
            Console.WriteLine(book.ShowDetailsRow()); 
        }
    };
    private static readonly Action BackToMainMenu = () => 
    {

    };
}

