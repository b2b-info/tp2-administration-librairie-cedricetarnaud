using BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


    internal class UpdateBookById : Operations
    {

    private Book _updateBook;

    public override int Id { get; set; }


    public override void PerformAction()
    {
        uint id = ToolBox.ReadUInt("Id: ");
        var book = Database.GetBookById(id);

        if (book == null)
        {
            Console.WriteLine("Book not found");
            return;
        }

        string finalTitle = book.Title;
        string finalAuthor = book.Author; 
        double finalPrice = book.Price;
        int finalQuantity = book.Quantity;
        Console.WriteLine("Leave empty to keep the current value");

        Console.WriteLine($"Current Title: {book.Title}");
        string newTitle = ToolBox.ReadOptional("New Title: ");

        Console.WriteLine($"Current Author: {book.Author}");
        string newAuthor = ToolBox.ReadOptional("New Author: ");

        Console.WriteLine($"Current Price: {book.Price}");
        string newPrice = ToolBox.ReadOptional("New Price: ");

        Console.WriteLine($"Current Quantity: {book.Quantity}");
        string newQuantity = ToolBox.ReadOptional("New Quantity: ");

        AssignEntries(newTitle, newAuthor, newPrice, newQuantity, ref finalTitle, ref finalAuthor, ref finalPrice, ref finalQuantity);

        Book _updateBook = new (id, finalTitle, finalAuthor, finalPrice, finalQuantity);

        Program.IncrementId();
        
    }

    public override void Product () {
        if (Database.UpdateBook(_updateBook))
        {
            Console.WriteLine("Book updated");
        }
        else
        {
            Console.WriteLine("Book could not be updated");
        }
    }

    private void AssignEntries(string newTitle,string newAuthor,string newPrice, string newQuantity,ref string finalTitle, ref string finalAuthor, ref double finalPrice, ref int finalQuantity)
    {
        if(string.IsNullOrEmpty(newTitle)) finalTitle = newTitle;

        if(string.IsNullOrEmpty(newAuthor)) finalAuthor = newAuthor;

         
        if (!string.IsNullOrWhiteSpace(newPrice) && double.TryParse(newPrice, out var parsedPrice))
        {
            finalPrice = parsedPrice;
        }

        if (!string.IsNullOrWhiteSpace(newQuantity) && int.TryParse(newQuantity, out var parsedQty))
        {
            finalQuantity = parsedQty;
        }
    }
    private void blabla()
    {
     
    }
    
}

   
    

