namespace BookStore;

using System;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

class Program
{
    public static ILogger logger = LoggerFactory
        .Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug).AddConsole();
        })
        .CreateLogger<Program>();
    private static readonly Dictionary<uint, Operations> PossibleOperations = new Dictionary<uint, Operations> { { 1, new AddBook() }, { 2, new DeleteBook() },{3,new BookInformations() },{ 4,new UpdateBookById()},{5,new ClearScreen() },{6,new Exit() } };
    public static readonly Channel<Operations> TasksQueue = Channel.CreateUnbounded<Operations>();
    public static int IdTasks = 0;
    private static readonly object _lockIdTasks = new();
    
    static async Task Main(string[] args)
    {
        logger.LogInformation("Application started.");

        Database.SeedDemoData();
        logger.LogInformation("Database seed with demo data.");

        RunLoginLoop();
        using var cancellationToken = new CancellationTokenSource();
        var worker = Task.Run(() => Consume(cancellationToken.Token));
        RunMenuLoop();
    }

    private async static void RunLoginLoop()
    {
        uint count = 0;
        Console.WriteLine("Please Login");

        while (count <= 2)
        {
            Login.GetLoginInfo();

            if (Login.IsLoggedIn())
            {
                logger.LogInformation($"User successfully logged in.");
                Console.Clear();
                break;
            }
            else
            {
                
                if (count == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Too many failed attempts....");
                    logger.LogError("Application exiting due to too many failed login attempts.");
                    Environment.Exit(0);
                }
                else
                {
                    count++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid credentials. Try again....");
                    logger.LogWarning($"Failed login attempt: {count}");
                    Console.ResetColor();
                }
            }
        }
        logger.LogInformation("Starting main menu loop.");
    }
    private static void RunMenuLoop()
    {
        while (true)
        {
            ShowMainMenu();
            uint operation = ToolBox.ReadUInt("Enter your operation: ");
            PossibleOperations[operation]?.ExecuteState();
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        logger.LogInformation("Application exiting");
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

    public static ValueTask Produce(Operations operation) 
    {
          return TasksQueue.Writer.WriteAsync(operation);
    }
    static async Task Consume(CancellationToken cancellationToken)
    {
        Console.WriteLine("allo");
        await foreach (var operation in TasksQueue.Reader.ReadAllAsync(cancellationToken))
        {
            operation.ExecuteState();
            await Task.Delay(100, cancellationToken);
        }
    }
    public static void IncrementId()
    {
        lock (_lockIdTasks)
        {
            IdTasks++;
        }
    }
}
