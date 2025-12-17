using BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public abstract class Operations
    {
        protected OperationsStates OperationsStates;
        public abstract void ExecuteState();
    }

