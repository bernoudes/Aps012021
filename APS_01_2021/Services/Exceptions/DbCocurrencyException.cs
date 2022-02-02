using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services.Exceptions
{
    public class DbCocurrencyException : ApplicationException
    {
        public DbCocurrencyException (string message) : base(message)
        {
        }
    }
}
