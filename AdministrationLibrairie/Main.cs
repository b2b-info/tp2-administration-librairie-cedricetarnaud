namespace BookStore;

using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Database.SeedDemoData();

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

        await RunMenuLoop();
    }

    private static async Task RunMenuLoop()
    {
        bool exit = false;

        while (!exit)
        {
            ShowMainMenu();

            uint operation = ReadUInt("Enter your operation: ");

            switch (operation)
            {
                case 1:
                    Console.WriteLine("\nAdd Book:");
                    await AddBookFlow();
                    break;

                case 2:
                    DeleteBookMenu();
                    break;

                case 3:
                    BookInformationMenu();
                    break;

                case 4:
                    Console.WriteLine("\nUpdate book:");
                    UpdateBookById();
                    break;

                case 5:
                    Console.Clear();
                    break;

                case 6:
                    Console.WriteLine("Exiting program....");
                    exit = true;
                    break;

                default:
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

    private static async Task AddBookFlow()
    {
        uint id = ReadUInt("Id: ");

        if (Database.CheckPkExists(id))
        {
            Console.WriteLine("A book with this Id already exists");
            return;
        }

        string title = ReadNonEmpty("Title: ");
        string author = ReadNonEmpty("Author: ");
        double price = ReadDouble("Price: ");
        int quantity = ReadInt("Quantity: ");

        var book = new Book(id, title, author, price, quantity);
        await Database.AddBook(book);

        Console.WriteLine("Book added successfully");
    }

    private static void DeleteBookMenu()
    {
        bool back = false;

        while (!back)
        {
            Console.WriteLine("1. Delete Book by Id");
            Console.WriteLine("2. Delete Book by Title");
            Console.WriteLine("3. Back to Main Menu");

            uint operation = ReadUInt("Enter your operation: ");

            switch (operation)
            {
                case 1:
                    DeleteBookById();
                    break;

                case 2:
                    DeleteBookByTitle();
                    break;

                case 3:
                    back = true;
                    break;

                default:
                    Console.WriteLine("Invalid operation, try again");
                    break;
            }
        }
    }

    private static void DeleteBookById()
    {
        uint id = ReadUInt("Id: ");

        if (Database.RemoveBook(id))
        {
            Console.WriteLine("Book deleted");
        }
        else
        {
            Console.WriteLine("Book not found");
        }
    }

    private static void DeleteBookByTitle()
    {
        string title = ReadNonEmpty("Title: ");

        var books = Database.GetAllBooks().FindAll(b =>
                string.Equals(b.title, title, StringComparison.OrdinalIgnoreCase)
            );

        if (books.Count == 0)
        {
            Console.WriteLine("No book found with that title");
            return;
        }

        foreach (var book in books)
        {
            Database.RemoveBook(book.Id);
            Console.WriteLine($"Book {book.title} deleted");
        }
    }

    private static void BookInformationMenu()
    {
        bool back = false;

        while (!back)
        {
            Console.WriteLine("1. Show Book Details by Id");
            Console.WriteLine("2. Show Book Count");
            Console.WriteLine("3. Show All Books");
            Console.WriteLine("4. Back to Main Menu");

            uint operation = ReadUInt("Enter your operation: ");

            switch (operation)
            {
                case 1:
                    ShowBookDetailById();
                    break;

                case 2:
                    ShowBookCount();
                    break;

                case 3:
                    ShowAllBooks();
                    break;

                case 4:
                    back = true;
                    break;

                default:
                    Console.WriteLine("Invalid operation, try again");
                    break;
            }
        }
    }

    private static void ShowBookDetailById()
    {
        uint id = ReadUInt("Id: ");
        var book = Database.GetBookById(id);

        if (book == null)
        {
            Console.WriteLine("Book not found");
            return;
        }

        Console.WriteLine($"Id: {book.Id}");
        Console.WriteLine($"Title: {book.title}");
        Console.WriteLine($"Author: {book.Author}");
        Console.WriteLine($"Price: {book.Price}");
        Console.WriteLine($"Quantity: {book.Quantity}");
    }

    private static void ShowBookCount()
    {
        Console.WriteLine($"Total books: {Database.CountRecords()}");
    }

    private static void ShowAllBooks()
    {
        var books = Database.GetAllBooks();

        if (books.Count == 0)
        {
            Console.WriteLine("No books in the store");
            return;
        }

        foreach (var b in books)
        {
            Console.WriteLine($"[{b.Id}] {b.title} by {b.Author} - {b.Price} ({b.Quantity} in stock)");
        }
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
        Console.WriteLine($"Current Title: {book.title}");
        string newTitle = ReadOptional("New Title: ");

        Console.WriteLine($"Current Author: {book.Author}");
        string newAuthor = ReadOptional("New Author: ");

        Console.WriteLine($"Current Price: {book.Price}");
        string priceInput = ReadOptional("New Price: ");

        Console.WriteLine($"Current Quantity: {book.Quantity}");
        string qtyInput = ReadOptional("New Quantity: ");

        string finalTitle = string.IsNullOrWhiteSpace(newTitle) ? book.title : newTitle;
        string finalAuthor = string.IsNullOrWhiteSpace(newAuthor) ? book.Author : newAuthor;

        double finalPrice = book.Price;
        if (!string.IsNullOrWhiteSpace(priceInput) && double.TryParse(priceInput, out var parsedPrice))
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
            if (uint.TryParse(Console.ReadLine(), out var value)) return value;

            Console.WriteLine("Invalid number, try again");
        }
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value)) return value;

            Console.WriteLine("Invalid number, try again");
        }
    }

    private static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out var value)) return value;

            Console.WriteLine("Invalid number, try again");
        }
    }

    private static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) return input;

            Console.WriteLine("Value cannot be empty");
        }
    }

    private static string ReadOptional(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }
}
