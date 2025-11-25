namespace BookStore;

using System;
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

    static async Task Main(string[] args)
    {
        logger.LogInformation("Application started.");

        Database.SeedDemoData();
        logger.LogInformation("Database seed with demo data.");

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
        await RunMenuLoop();
    }

    private static async Task RunMenuLoop()
    {
        bool exit = false;

        while (!exit)
        {
            ShowMainMenu();

            uint operation = ReadUInt("Enter your operation: ");
            logger.LogDebug($"User selected option: {operation}.");

            switch (operation)
            {
                case 1:
                    logger.LogInformation("Starting AddBookFlow.");
                    Console.WriteLine("\nAdd Book:");
                    await AddBookFlow();
                    break;

                case 2:
                    logger.LogInformation("Starting DeleteBookMenu.");
                    DeleteBookMenu();
                    break;

                case 3:
                    logger.LogInformation("Starting BookInformationMenu.");
                    BookInformationMenu();
                    break;

                case 4:
                    logger.LogInformation("Starting UpdateBookById.");
                    Console.WriteLine("\nUpdate book:");
                    UpdateBookById();
                    break;

                case 5:
                    logger.LogInformation("Console Cleared.");
                    Console.Clear();
                    break;

                case 6:
                    logger.LogInformation("User requested exit.");
                    Console.WriteLine("Exiting program....");
                    exit = true;
                    break;

                default:
                    logger.LogInformation($"Invalid operation entered: {operation}.");
                    Console.WriteLine("Invalid operation. Try again");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }
        logger.LogInformation("Application exiting");
    }

    private static void ShowMainMenu()
    {
        logger.LogInformation("Displaying main menu.");

        Console.WriteLine("====================================");
        Console.WriteLine("1. Add Book");
        Console.WriteLine("2. Delete Book");
        Console.WriteLine("3. Book Information");
        Console.WriteLine("4. Update Book by Id");
        Console.WriteLine("5. Clear Screen");
        Console.WriteLine("6. Exit");
        Console.WriteLine("====================================");
    }

    private static async Task AddBookFlow()
    {
        uint id = ReadUInt("Id: ");
        logger.LogDebug($"User entered Id: {id}.");

        if (Database.CheckPkExists(id))
        {
            logger.LogWarning($"AddBookFlow aborted: Id {id} already exist.");
            Console.WriteLine("A book with this Id already exists");
            return;
        }

        string title = ReadNonEmpty("Title: ");
        logger.LogDebug($"User entered Title: {title}.");

        string author = ReadNonEmpty("Author: ");
        logger.LogDebug($"User entered Author: {author}.");
        
        double price = ReadDouble("Price: ");
        logger.LogDebug($"User entered Price: {price}.");

        int quantity = ReadInt("Quantity: ");
        logger.LogDebug($"User entered Quantity: {quantity}.");


        var book = new Book(id, title, author, price, quantity);
        await Database.AddBook(book);

        Console.WriteLine("Book added successfully");
    }

    private static void DeleteBookMenu()
    {
        bool back = false;

        while (!back)
        {
            logger.LogInformation("Displaying DeleteBookMenu options.");

            Console.WriteLine("1. Delete Book by Id");
            Console.WriteLine("2. Delete Book by Title");
            Console.WriteLine("3. Back to Main Menu");

            uint operation = ReadUInt("Enter your operation: ");
            logger.LogDebug($"User selected option: {operation} in DeleteBookMenu.");

            switch (operation)
            {
                case 1:
                    logger.LogInformation("User choose to DeleteBookById.");
                    DeleteBookById();
                    break;

                case 2:
                    logger.LogInformation("User choose to DeleteBookByTitle.");
                    DeleteBookByTitle();
                    break;

                case 3:
                    logger.LogInformation("User choose to go back to the Main Menu.");
                    back = true;
                    break;

                default:
                    logger.LogWarning($"Invalid operation entered in DeleteBookMenu: {operation}.");
                    Console.WriteLine("Invalid operation, try again");
                    break;
            }
        }

        logger.LogInformation("Exiting DeleteBookMenu.");
    }

    private static void DeleteBookById()
    {
        logger.LogInformation("Starting DeleteBookById.");

        uint id = ReadUInt("Id: ");

        if (Database.RemoveBook(id))
        {
            Console.WriteLine("Book deleted");
        }
        else
        {
            Console.WriteLine("Book not found");
        }

        logger.LogInformation("Exiting DeleteBookById.");
    }

    private static void DeleteBookByTitle()
    {
        logger.LogInformation("Starting DeleteBookByTitle.");

        string title = ReadNonEmpty("Title: ");
        logger.LogDebug($"User entered Title: {title}");

        var books = Database
            .GetAllBooks()
            .FindAll(b => string.Equals(b.Title, title, StringComparison.OrdinalIgnoreCase));

        if (books.Count == 0)
        {
            logger.LogWarning($"No books with Title: {title}.");
            Console.WriteLine("No book found with that Title");
            return;
        }

        foreach (var book in books)
        {
            Database.RemoveBook(book.Id);
            logger.LogInformation($"Book deleted: {book.Title}.");
            Console.WriteLine($"Book {book.Title} deleted");
        }

        logger.LogInformation("Exiting DeleteBookByTitle.");
    }

    private static void BookInformationMenu()
    {
        logger.LogInformation("Starting BookInformationMenu.");

        bool back = false;

        while (!back)
        {
            logger.LogInformation("Displaying BookInformationMenu options.");

            Console.WriteLine("1. Show Book Details by Id");
            Console.WriteLine("2. Show Book Count");
            Console.WriteLine("3. Show All Books");
            Console.WriteLine("4. Back to Main Menu");

            uint operation = ReadUInt("Enter your operation: ");
            logger.LogDebug($"User selected operation: {operation} in BookInformationMenu.");

            switch (operation)
            {
                case 1:
                    logger.LogInformation("User choose to ShowBookDetailById.");
                    ShowBookDetailById();
                    break;

                case 2:
                    logger.LogInformation("User choose to ShowBookCount.");
                    ShowBookCount();
                    break;

                case 3:
                    logger.LogInformation("User choose to ShowAllBooks.");
                    ShowAllBooks();
                    break;

                case 4:
                    logger.LogInformation("User choose to go back to the Main Menu.");
                    back = true;
                    break;

                default:
                    logger.LogWarning($"Invalid operation entered in DeleteBookMenu: {operation}.");
                    Console.WriteLine("Invalid operation, try again");
                    break;
            }
        }

        logger.LogInformation("Exiting BookInformationMenu.");
    }

    private static void ShowBookDetailById()
    {
        logger.LogInformation("Starting ShowBookDetailById.");

        uint id = ReadUInt("Id: ");
        logger.LogDebug($"User entered Id: {id}.");

        var book = Database.GetBookById(id);
        if (book == null)
        {   
            Console.WriteLine("Book not found");
            return;
        }

        Console.WriteLine($"Id: {book.Id}");
        Console.WriteLine($"Title: {book.Title}");
        Console.WriteLine($"Author: {book.Author}");
        Console.WriteLine($"Price: {book.Price}");
        Console.WriteLine($"Quantity: {book.Quantity}");

        logger.LogInformation("Exiting ShowBookDetailById.");
    }

    private static void ShowBookCount()
    {
        logger.LogInformation("Starting ShowBookCount");
        Console.WriteLine($"Total books: {Database.CountRecords()}");

        logger.LogInformation("Exiting ShowBookCount");
    }

    private static void ShowAllBooks()
    {
         logger.LogInformation("Starting ShowAllBooks.");

        var books = Database.GetAllBooks();

        if (books.Count == 0)
        {
            logger.LogWarning("No book in the store.");
            Console.WriteLine("No books in the store");
            logger.LogInformation("Exiting ShowAllBooks.");
            return;
        }

        foreach (var b in books)
        {
            logger.LogDebug("Displaying book: {@Book}.", b);
            Console.WriteLine(
                $"[{b.Id}] {b.Title} by {b.Author} - {b.Price} ({b.Quantity} in stock)"
            );
        }

        logger.LogInformation("Exiting ShowAllBooks.");
    }

    private static void UpdateBookById()
    {
        uint id = ReadUInt("Id: ");
        var book = Database.GetBookById(id);

        if (book == null)
        {
            Console.WriteLine("Book not found");
            return;
        }

        Console.WriteLine("Leave empty to keep the current value");
        Console.WriteLine($"Current Title: {book.Title}");
        string newTitle = ReadOptional("New Title: ");

        Console.WriteLine($"Current Author: {book.Author}");
        string newAuthor = ReadOptional("New Author: ");

        Console.WriteLine($"Current Price: {book.Price}");
        string priceInput = ReadOptional("New Price: ");

        Console.WriteLine($"Current Quantity: {book.Quantity}");
        string qtyInput = ReadOptional("New Quantity: ");

        string finalTitle = string.IsNullOrWhiteSpace(newTitle) ? book.Title : newTitle;
        string finalAuthor = string.IsNullOrWhiteSpace(newAuthor) ? book.Author : newAuthor;

        double finalPrice = book.Price;
        if (
            !string.IsNullOrWhiteSpace(priceInput)
            && double.TryParse(priceInput, out var parsedPrice)
        )
        {
            finalPrice = parsedPrice;
        }

        int finalQuantity = book.Quantity;
        if (!string.IsNullOrWhiteSpace(qtyInput) && int.TryParse(qtyInput, out var parsedQty))
        {
            finalQuantity = parsedQty;
        }

        var updated = new Book(id, finalTitle, finalAuthor, finalPrice, finalQuantity);
        if (Database.UpdateBook(updated))
        {
            Console.WriteLine("Book updated");
        }
        else
        {
            Console.WriteLine("Book could not be updated");
        }
    }

    private static uint ReadUInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            logger.LogDebug($"User input for prompt '{prompt}' is: {input}.");

            if (uint.TryParse(input, out var value))
            {
                logger.LogInformation($"Parsed uint successfully: {value}.");
                return value;
            }
                
            logger.LogWarning($"Invalid uint entered: {input}.");
            Console.WriteLine("Invalid number, try again");
        }
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            logger.LogDebug($"User input for prompt '{prompt}' is: {input}.");

            if (int.TryParse(input, out var value))
            {
                logger.LogInformation($"Parsed int successfully: {value}.");
                return value;
            }
                
            logger.LogWarning($"Invalid int entered: {input}.");
            Console.WriteLine("Invalid number, try again");
        }
    }

    private static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            logger.LogDebug($"User input for prompt '{prompt}' is: {input}.");

            if (double.TryParse(input, out var value))
            {
                logger.LogInformation($"Parsed double successfully: {value}.");
                return value;
            }
                
            logger.LogWarning($"Invalid double entered: {input}.");
            Console.WriteLine("Invalid number, try again");
        }
    }

    private static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            logger.LogDebug($"User input for prompt '{prompt}' is: {input}.");

            if (!string.IsNullOrWhiteSpace(input))
            {
                logger.LogInformation($"Valid non-empty inpu received: {input}.");
                return input;
            }
                
            logger.LogWarning($"Empty input entered for promt: '{prompt}'.");
            Console.WriteLine("Value cannot be empty");
        }
    }

    private static string ReadOptional(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        string result;

        if (string.IsNullOrWhiteSpace(input))
        {
            result = "";
            logger.LogWarning($"User input for prompt '{prompt}' was empty.");
        }
        else
        {
            logger.LogDebug($"User input for prompt '{prompt}' is: {input}");
            result = input;
        }

        return  result;
    }
}
