using System;
using System.Collections.Generic;
using System.Text;

namespace Enlang.Utils
{
    internal class NullInputException : Exception
    {
        public NullInputException() : base("Input Cannot be empty!")
        {

        }
    }
}
