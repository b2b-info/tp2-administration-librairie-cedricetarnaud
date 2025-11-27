namespace BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Exit : Operations
{
    public override void ExecuteState()
    {
        if (operationsStates == OperationsStates.Waiting)
        {
            //manque de quoi attention
            Environment.Exit(0);

        }
    }


}

