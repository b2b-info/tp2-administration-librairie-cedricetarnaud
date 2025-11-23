using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public class UserCredential
    {
        public string UserName { get; }
        public string Password { get; }

        public UserCredential(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
