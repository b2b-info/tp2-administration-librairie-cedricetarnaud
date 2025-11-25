using BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ClearScreen : IActions
{
    public void PerformAction()
    {
        Console.Clear();
    }
}

