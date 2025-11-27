using BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class AddBook : IOperations
{
    public async void PerformAction()
    {
        uint id = (uint) Database.CountRecords() + 1;
        string title = ToolBox.ReadNonEmpty("Book title : ");
        string author = ToolBox.ReadNonEmpty("Book author : ");
        double price = ToolBox.ReadDoublePositive("Book price : ");
        int quantity = ToolBox.ReadIntPositive("Book quantity : ");
        Book book = new(id,title,author,price,quantity);
        await Database.AddBook(book);
    }
}

