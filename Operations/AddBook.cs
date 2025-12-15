using BookStore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class AddBook : Operations
{
    private uint _id = 0;
    private string _title = "Default";
    private string _author = "Default";
    private int _quantity = 0;
    private double _price = 0;
    public override  void ExecuteState()
    {
        switch (operationsStates)
        {
            case OperationsStates.Waiting:
                ExecuteWaitingState();
                break;
            case OperationsStates.Queued:
                ExecuteQueuedState();
                break;
        }
      
    }
    
    private async void ExecuteWaitingState()
    {
        _id = (uint)Database.CountRecords() + 1;
        _title = ToolBox.ReadNonEmpty("Book title : ");
        _author = ToolBox.ReadNonEmpty("Book author : ");
        _price = ToolBox.ReadDoublePositive("Book price : ");
        _quantity = ToolBox.ReadIntPositive("Book quantity : ");

        await Program.Produce(this, "Adding book in queue");
        operationsStates = OperationsStates.Queued;
    }
    private async void ExecuteQueuedState()
    {
        Program.logger.LogInformation("Adding book...");
        Stopwatch stopwatch = Stopwatch.StartNew();
        Book book = new(_id, _title, _author, _price, _quantity);
        await Database.AddBook(book);
        stopwatch.Stop();
        Program.logger.LogInformation($"Book added in {stopwatch.ElapsedMilliseconds} milliseconds");
    }
}

