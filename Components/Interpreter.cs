using System;
using System.Collections.Generic;
using System.Text;

namespace Enlang.Components
{
    internal class Interpreter
    {
        List<Token> Instructions;

        public Interpreter(List<Token> instructions)
        {
            Instructions = new List<Token>(instructions);
        }
    }
}
