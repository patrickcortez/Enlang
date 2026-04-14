using System;
using System.Collections.Generic;
using System.Text;

using Variable = (string name,object value); //variable definition

namespace Enlang.Components
{
    internal enum Types // Declaring our Token types: Print, Variable and Input
    {
        Print, //Outputing text
        Variable, //Storing data
        Input, // Acquiring user input
        Error
    }

#nullable enable

    internal struct Token // Token Definition
    {
        Types type ;
        Variable? variable;
        string ErrorValue;

        public Token(Types tokentype,Variable? var = null,string Error = "") // Token type: Print, Input or Variable. 
        {


            if(tokentype == Types.Variable && var != null) // If the token is a variable we finally store the var parameter in the variable member.
            {
                type = tokentype;
                variable = var;
            }else if(tokentype == Types.Error) // If its an unrecognized instruction, we store it as an Error to log later.
            {
                ErrorValue = Error;
            }
            else // Otherwise we leave it variable uninitialized and just store the tokentype.
            {
                type = tokentype;
            }
        }
    }
}
