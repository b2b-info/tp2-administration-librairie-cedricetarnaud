using BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class TaskInstruction
    {
        public int OperationKey { get; private set; }
        public int? ActionKey { get; private set; }

    public TaskInstruction(int operationKey, int actionKey) 
    {
        OperationKey = operationKey;
        ActionKey = actionKey;
    }
    }

