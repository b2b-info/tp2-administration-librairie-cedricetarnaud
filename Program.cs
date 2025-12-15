namespace BookStore;

using System;

using System.Diagnostics;
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

    public static bool IsRunning { get; set; } = true;
    public static CancellationTokenSource CancellationTokenSourceMain { get; set; }
    static async Task Main(string[] args)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
    public static bool IsRunning = true;
    public static CancellationTokenSource CancellationToken;
    static async Task Main(string[] args)
    {
        CancellationToken = new CancellationTokenSource();
        logger.LogInformation("Application started.");
        Database.SeedDemoData();
        logger.LogInformation("Database seed with demo data.");
        RunLoginLoop();
        CancellationTokenSourceMain = new CancellationTokenSource();
        var worker = Task.Run(() => Consume(CancellationTokenSourceMain.Token));
        RunMenuLoop();
        logger.LogInformation("Application exiting");
        await worker;
        stopwatch.Stop();
        logger.LogInformation($"Running for {stopwatch.ElapsedMilliseconds} milliseconds");
        var worker = Task.Run(() => Consume(CancellationToken.Token));
        RunMenuLoop();
        await worker;
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
        while (IsRunning)
        {
            ShowMainMenu();
            uint operation = ToolBox.ReadUInt("Enter your operation: ");
            PossibleOperations[operation]?.ExecuteState();
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

    public static ValueTask Produce(Operations operation,string actionQueud) 
    {
        logger.LogInformation(actionQueud);
        return TasksQueue.Writer.WriteAsync(operation);
    }
    static async Task Consume(CancellationToken cancellationToken)
    {
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
