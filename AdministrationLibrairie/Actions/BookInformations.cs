using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BookInformations : IActions
{
    public void PerformAction()
    {
        Console.WriteLine("1. Show Book Details by Id");
        Console.WriteLine("2. Show Book Count");
        Console.WriteLine("3. Show All Books");
        Console.WriteLine("4. Back to Main Menu");
        int choice = ToolBox.ReadInt("Enter you operation: ");
    }
}

