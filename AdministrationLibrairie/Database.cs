using System.Security.Cryptography.X509Certificates;

namespace BookStore;

public class Database
{
    private readonly Dictionary<int, BookCrud> Cruds = new Dictionary<int, BookCrud> { { 1, new BookCrudId() }, { 2, new BookCrudTitle() } };

    private static readonly List<Book> books = new();
    private static readonly object _lockDatabase = new();
    private static uint nextId = 1;

    public static void HandleRequest(UInt32 actionRequested)
    {

    }
    public static int CountRecords()
    {
        lock (_lockDatabase)
        {
            return books.Count;
        }
    }

    public static bool CheckPkExists(uint pk)
    {
        lock (_lockDatabase)
        {
            return books.Any(book => book.Id == pk);
        }

    }

    public static List<Book> GetAllBooks()
    {
        lock (_lockDatabase)
        {
            return books.ToList();
        }

    }
    public static void HandleBookDelete()
    {
        Console.WriteLine("1. Delete Book by Id");
        Console.WriteLine("2. Delete Book by Title");
        Console.WriteLine("3. Back to Main Menu");

        string operation = ReadUInt("Enter your operation: ");
       
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
    private static void DeleteBookById()
    {
       
    }
    private static void DeleteBookByTitle()
    {
        string title = ReadNonEmpty("Title: ");

        

        if (books.Count == 0)
        {
            Console.WriteLine("No book found with that Title");
            return;
        }

        foreach (var book in books)
        {
            Database.RemoveBook(book.Id);
            Console.WriteLine($"Book {book.Title} deleted");
        }
    }
    public static Book? GetBookById(uint id)
    {
        lock (_lockDatabase)
        {
            return books.FirstOrDefault(book => book.Id == id);
        }

    }

    public static async Task AddBook(Book book)
    {
        await Task.Delay(500);

        lock (_lockDatabase)
        {
            books.Add(book);
        }

    }

    public static bool UpdateBook(Book updated)
    {
        lock (_lockDatabase)
        {
            var index = books.FindIndex(book => book.Id == updated.Id);
            if (index == -1) return false;

            books[index] = updated;
            return true;
        }

    }

    public static bool RemoveBook(uint id)
    {
        lock (_lockDatabase)
        {
            var book = books.FirstOrDefault(book => book.Id == id);
            if (book == null) return false;

            books.Remove(book);
            return true;
        }
    }

    public static void SeedDemoData()
    {
        lock (_lockDatabase)
        {
            if (books.Count > 0) return;

            books.Add(
                new Book(
                    id: nextId++,
                    title: "The Pragmatic Programmer",
                    author: "Andrew Hunt, David Thomas",
                    price: 49.99,
                    quantity: 10
                )
            );
            books.Add(
                new Book(
                    id: nextId++,
                    title: "Clean Code",
                    author: "Robert C. Martin",
                    price: 39.99,
                    quantity: 5
                )
            );
            books.Add(
                new Book(
                    id: nextId++,
                    title: "Design Patterns: Elements of Reusable Object-Oriented Software",
                    author: "Erich Gamma",
                    price: 54.99,
                    quantity: 7
                )
            );
            books.Add(
                new Book(
                    id: nextId++,
                    title: "Refactoring: Improving the Design of Existing Code",
                    author: "Martin Fowler",
                    price: 47.50,
                    quantity: 4
                )
            );
            books.Add(
                new Book(
                    id: nextId++,
                    title: "Head First Design Patterns",
                    author: "Eric Freeman",
                    price: 44.99,
                    quantity: 12
                )
            );
        }
    }
}