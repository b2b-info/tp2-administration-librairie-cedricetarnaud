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
            //mettre dans database
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
        Console.WriteLine($"Title: {book.Title}");
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
            Console.WriteLine($"[{b.Id}] {b.Title} by {b.Author} - {b.Price} ({b.Quantity} in stock)");
        }
    }

    private static void UpdateBookById()
    {
       
    }


   
}
