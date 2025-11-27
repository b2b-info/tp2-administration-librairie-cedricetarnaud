namespace BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Exit : Operations
{
    public override void PerformAction()
    {
        Environment.Exit(0);
    }

    public override void Product()
    {
        throw new NotImplementedException();
    }
}

