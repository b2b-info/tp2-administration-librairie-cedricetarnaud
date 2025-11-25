using BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    internal class UpdateBookById : IActions
    {
      
    public void PerformAction()
    {
        uint id = ToolBox.ReadUInt("Id: ");
        var book = Database.GetBookById(id);

        if (book == null)
        {
            Console.WriteLine("Book not found");
            return;
        }
            Console.WriteLine("Leave empty to keep the current value");
        Console.WriteLine($"Current Title: {book.Title}");
        string newTitle = ToolBox.ReadOptional("New Title: ");

        Console.WriteLine($"Current Author: {book.Author}");
        string newAuthor = ToolBox.ReadOptional("New Author: ");

        Console.WriteLine($"Current Price: {book.Price}");
        string priceInput = ToolBox.ReadOptional("New Price: ");

        Console.WriteLine($"Current Quantity: {book.Quantity}");
        string qtyInput = ToolBox.ReadOptional("New Quantity: ");

        string finalTitle = string.IsNullOrWhiteSpace(newTitle) ? book.Title : newTitle;
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
}

   
    

