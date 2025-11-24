namespace BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BookCrudId : BookCrud
{
    public override Action Delete(string information)
    {
        throw new NotImplementedException();
    }

    public override Action Insert(string information)
    {
        throw new NotImplementedException();
    }


    public override Action Update = () =>
    {

    };
    public Dictionary<int, Action> ActionsPossible()
    {
        return new Dictionary<int, Action> { { 1, Delete }, { 2, Insert }, { 3, Update } };
    }
}

