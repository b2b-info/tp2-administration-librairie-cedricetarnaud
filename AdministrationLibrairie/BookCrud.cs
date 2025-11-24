using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore;

public abstract class BookCrud
{
    
   
    public abstract Action Delete { get; }
    public abstract Action Insert { get; }
    public abstract Action Update { get; }
    //public static void Get(string operation, out BookCrud currentCrudUsed)
    //{

    //    if (UInt32.TryParse(operation, out var result))
    //    {
    //        var books = Database.GetAllBooks().FindAll(b =>
    //            uint.Equals(b.Id,UInt32.Parse(operation))
    //        );
    //    }
    //    else
    //    {
    //        Book books = Database.GetAllBooks().FindAll(b =>
    //                 string.Equals(b.Title, title, StringComparison.OrdinalIgnoreCase)
    //             );
    //    }
            
    //}

}

