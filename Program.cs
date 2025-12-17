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
    public static bool IsRunning { get; set; } = true;
    public static CancellationTokenSource CancellationTokenSourceMain { get; set; }
    static async Task Main(string[] args)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        CancellationTokenSourceMain = new CancellationTokenSource();
        logger.LogInformation("Application started.");
        logger.LogInformation("Database seed with demo data.");
        Database.SeedDemoData();
        logger.LogInformation("Running Login loop");
        Login.RunLoginLoop();
        logger.LogInformation("Running Worker");
        var worker = Task.Run(() => ProducerConsumerPatternHandler.Consume(CancellationTokenSourceMain.Token));
        logger.LogInformation("Running Menu Loop");
        RunMenuLoop();
        logger.LogInformation("Application exiting");
        await worker;
        stopwatch.Stop();
        logger.LogInformation($"Running for {stopwatch.ElapsedMilliseconds} milliseconds");
        await worker;
    }
    private static void RunMenuLoop()
    {
        while (IsRunning)
        {
            Console.WriteLine("====================================");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Delete Book");
            Console.WriteLine("3. Book Information");
            Console.WriteLine("4. Update Book by Id");
            Console.WriteLine("5. Clear Screen");
            Console.WriteLine("6. Exit");
            Console.WriteLine("====================================");
            uint operation = ToolBox.ReadUInt("Enter your operation: ");
            PossibleOperations[operation]?.ExecuteState();
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }

}
