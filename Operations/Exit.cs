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
        if (OperationsStates == OperationsStates.Waiting)
        {
            Program.IsRunning = false;
            Program.CancellationTokenSourceMain.Cancel();
        }
    }


}

