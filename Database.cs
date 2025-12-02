using System.Security.Cryptography.X509Certificates;

namespace BookStore;

using Microsoft.Extensions.Logging;


public class Database
{

    private static readonly List<Book> _books = new();
    private static readonly object _lockDatabase = new();
    private static uint _nextId = 1;

  
    public static int CountRecords()
    {
        lock (_lockDatabase)
        {
            int count = _books.Count;
            return count;
        }
    }

    public static bool CheckPkExists(uint pk)
    {
        Program.logger.LogInformation($"Checking if {pk} exist in the database.");
        lock (_lockDatabase)
        {
            bool exists = _books.Any(book => book.Id == pk);

            if (exists)
            {
                Program.logger.LogInformation($"Primary key {pk} found in the database.");
            }
            else
            {
                Program.logger.LogWarning($"Primary key {pk} does not exist in the database.");
            }

            return exists;
        }
    }

    public static List<Book> GetAllBooks()
    {
        Program.logger.LogInformation($"Retrieving all _books from the database.");
        lock (_lockDatabase)
        {
            var listBooks = _books.ToList();
            int count = listBooks.Count;

            Program.logger.LogDebug($"Retrieving {count} _books from the database.");

            return listBooks;
        }
    }

    public static Book? GetBookById(uint id)
    {
        Program.logger.LogInformation($"Looking up for the book with ID: {id}.");
        lock (_lockDatabase)
        {
            var book = _books.FirstOrDefault(book => book.Id == id);

            if(book != null)
            {
                Program.logger.LogInformation($"Book with ID {id} found.");
            }
            else
            {
                Program.logger.LogWarning($"Book with ID {id} not found.");
            }

            return book;
        }
    }

    public static async Task AddBook(Book book)
    {
        Program.logger.LogInformation($"Adding book with ID: {book.Id} started.");
        await Task.Delay(500);
      
        lock (_lockDatabase)
        {
            _books.Add(book);
        }

        Program.logger.LogInformation($"Book successfully added with ID: {book.Id} and Title: {book.Title}.");
    }

    public static bool UpdateBook(Book updated)
    {
        Program.logger.LogInformation($"Updating book with ID: {updated.Id} started.");

        lock (_lockDatabase)
        {
            var index = _books.FindIndex(book => book.Id == updated.Id);
            if (index == -1)
            {
                Program.logger.LogWarning($"Updating failed. Book with ID: {updated.Id} was not found.");
                return false;
            }
                

            _books[index] = updated;

            Program.logger.LogInformation($"Book successfully updated with ID: {updated.Id} and Title: {updated.Title}");
            return true;
        }
    }

    public static bool RemoveBook(uint id)
    {
        Program.logger.LogInformation($"Removing book with ID: {id}.");
        lock (_lockDatabase)
        {
            var book = _books.FirstOrDefault(book => book.Id == id);
            if (book == null)
            {
                Program.logger.LogWarning($"Removing failed. Book with ID: {id} was not found.");
                return false;
            }
                

            _books.Remove(book);

            Program.logger.LogInformation($"Book successfully removed with ID: {id}");
            return true;
        }
    }

    public static void SeedDemoData()
    {
        lock (_lockDatabase)
        {
            if (_books.Count > 0)
            {
                Program.logger.LogDebug("SeedDemoData skipped. Database already contains _books.");
                return;
            }
                

            _books.Add(
                new Book(
                    id: _nextId++,
                    title: "The Pragmatic Programmer",
                    author: "Andrew Hunt, David Thomas",
                    price: 49.99,
                    quantity: 10
                )
            );
            _books.Add(
                new Book(
                    id: _nextId++,
                    title: "Clean Code",
                    author: "Robert C. Martin",
                    price: 39.99,
                    quantity: 5
                )
            );
            _books.Add(
                new Book(
                    id: _nextId++,
                    title: "Design Patterns: Elements of Reusable Object-Oriented Software",
                    author: "Erich Gamma",
                    price: 54.99,
                    quantity: 7
                )
            );
            _books.Add(
                new Book(
                    id: _nextId++,
                    title: "Refactoring: Improving the Design of Existing Code",
                    author: "Martin Fowler",
                    price: 47.50,
                    quantity: 4
                )
            );
            _books.Add(
                new Book(
                    id: _nextId++,
                    title: "Head First Design Patterns",
                    author: "Eric Freeman",
                    price: 44.99,
                    quantity: 12
                )
            );
            Program.logger.LogInformation($"SeedDemoData completed. {_books.Count} Demo _books added to the database.");
        }
    }
}
