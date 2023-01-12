using System;
using System.Collections.Generic;
using System.Text;

namespace HashTable
{
    public class DuplicateKeyException : ArgumentException
    {
        public DuplicateKeyException(string message, string paramName) 
            : base(message, paramName)
        {

        }
    }
}
