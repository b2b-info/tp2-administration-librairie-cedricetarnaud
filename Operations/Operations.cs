using BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public abstract class Operations
    {
        protected OperationsStates operationsStates;
        public abstract void ExecuteState();
    }

