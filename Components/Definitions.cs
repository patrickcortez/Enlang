using System;
using System.Collections.Generic;
using System.Text;

using Variable = (string name,object value); //variable definition

namespace Enlang.Components
{
    internal enum Types // Declaring our Token types: Print, Variable and Input
    {
        Print,
        Variable,
        Input
    }

    internal struct Token // Token Definition
    {
        Types type ;
        Variable variable;

        public Token(Types tokentype,Variable var)
        {


            if(tokentype == Types.Variable)
            {
                variable = var;
            }
            else
            {
                type = tokentype;
            }
        }
    }
}
