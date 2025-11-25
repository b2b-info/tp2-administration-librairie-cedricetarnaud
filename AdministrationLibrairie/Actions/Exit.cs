namespace BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Exit : IOperations
{
    public void PerformAction()
    {
        Environment.Exit(0);
    }
}

