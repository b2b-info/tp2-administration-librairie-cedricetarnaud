namespace BookStore;

using System;
using System.Threading.Tasks;

class Program
{
    private static readonly Dictionary<uint, Operations> PossibleOperations = new Dictionary<uint, Operations> { { 1, new AddBook() }, { 2, new DeleteBook() },{3,new BookInformations() },{ 4,new UpdateBookById()},{5,new ClearScreen() },{6,new Exit() } };
    public static readonly Queue<TaskInstruction> TasksQueue = new ();
    public static int IdTasks = 0;
    private static readonly object _lockIdTasks = new();

    static async Task Main(string[] args)
    {
        Database.SeedDemoData();

        RunLoginLoop();

        await RunMenuLoop();
    }

    private static void RunLoginLoop()
    {
        uint count = 0;
        Console.WriteLine("Please Login");

        while (count <= 2)

        {
            Login.GetLoginInfo();

            if (Login.IsLoggedIn())
            {
                Console.Clear();
                break;
            }
            else
            {
                if (count == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Too many failed attempts....");
                    Environment.Exit(0);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid credentials. Try again....");
                    Console.ResetColor();
                    count++;
                }
            }
        }
    }
    private static async Task RunMenuLoop()
    {
        while (true)
        {
            ShowMainMenu();
            uint operation = ToolBox.ReadUInt("Enter your operation: ");
            PossibleOperations[operation]?.PerformAction();
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine("====================================");
        Console.WriteLine("1. Add Book");
        Console.WriteLine("2. Delete Book");
        Console.WriteLine("3. Book Information");
        Console.WriteLine("4. Update Book by Id");
        Console.WriteLine("5. Clear Screen");
        Console.WriteLine("6. Exit");
        Console.WriteLine("====================================");
    }

    public static void IncrementId()
    {
        lock (_lockIdTasks)
        {
            IdTasks++;
        }
    }
}
