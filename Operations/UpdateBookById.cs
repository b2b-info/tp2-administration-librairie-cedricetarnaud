using BookStore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


    public class UpdateBookById : Operations
    {
    private uint _updatedBookId = 0;
    private string  _updatedTitle = "default";
    private string _updatedAuthor = "default";
    private uint _updatedQuantity = 0;
    private double _updatedPrice = 0;

    public override void ExecuteState()
    {
        switch(OperationsStates)
        {
            case OperationsStates.Waiting: 
                ExecuteWaitingState();
            break;
            
            case OperationsStates.Queued:
                ExecuteQueuedState(); 
            break;

        }
    } 
     
        
    private void ExecuteQueuedState()
    {
        Program.logger.LogInformation("Adding book...");
        Stopwatch stopwatch = Stopwatch.StartNew();
        if (Database.UpdateBook(new Book(_updatedBookId, _updatedTitle, _updatedAuthor, _updatedPrice, _updatedQuantity)))
        {
            Console.WriteLine("Book updated");
        }
        else
        {
            Console.WriteLine("Book could not be updated");
        }
        stopwatch.Stop();
        Program.logger.LogInformation($"Task executed in {stopwatch.ElapsedMilliseconds} milliseconds");
    }
    
    private async void ExecuteWaitingState()
    {
        _updatedBookId = ToolBox.ReadUInt("Id: ");
        var book = Database.GetBookById(_updatedBookId);

        if (book == null)
        {
            Console.WriteLine("Book not found");
            return;
        }

        _updatedTitle = book.Title;
        _updatedAuthor = book.Author;
        _updatedPrice = book.Price;
        _updatedQuantity = book.Quantity;
        Console.WriteLine("Leave empty to keep the current value");

        Console.WriteLine($"Current Title: {book.Title}");
        string newTitle = ToolBox.ReadOptional("New Title: ");

        Console.WriteLine($"Current Author: {book.Author}");
        string newAuthor = ToolBox.ReadOptional("New Author: ");

        Console.WriteLine($"Current Price: {book.Price}");
        string newPrice = ToolBox.ReadOptional("New Price: ");

        Console.WriteLine($"Current Quantity: {book.Quantity}");
        string newQuantity = ToolBox.ReadOptional("New Quantity: ");

        AssignEntries(newTitle, newAuthor, newPrice, newQuantity);

        await ProducerConsumerPatternHandler.Produce(this,"Adding book in queue");
        OperationsStates = OperationsStates.Queued;
    }


    private void AssignEntries(string newTitle,string newAuthor,string newPrice, string newQuantity)
    {
        if(string.IsNullOrEmpty(newTitle)) _updatedTitle = newTitle;

        if(string.IsNullOrEmpty(newAuthor)) _updatedTitle = newAuthor;

         
        if (!string.IsNullOrWhiteSpace(newPrice) && double.TryParse(newPrice, out var parsedPrice))
        {
            _updatedPrice = parsedPrice;
        }

        if (!string.IsNullOrWhiteSpace(newQuantity) && uint.TryParse(newQuantity, out var parsedQty))
        {
            _updatedQuantity = parsedQty;
        }
    }    
}

   
    

