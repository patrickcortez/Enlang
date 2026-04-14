namespace Enlang.Components
{
    internal enum Types // Declaring our Token types: Print, Variable and Input
    {
        Print, //Outputing text
        Variable, //Storing data
        Input, // Acquiring user input
        Error,
        End
    }

    internal enum MathTypes // For Aritthmetic
    {
        Number, // 1-9
        Plus, // +
        Minus, // -
        Multiply, // *
        Divide, // /
        Power, // ^
        Lpar, // (
        Rpar, // )
        End
    }

    internal struct MathToken // Math Tokens
    {
        public MathTypes type; // token type
        public float value; // float by default so we have decimal places during division.


        public MathToken(MathTypes mathtype,float val = 0.0F)
        {
            type = mathtype;
            value = val;
        }
    }



    internal struct Token // Token Definition
    {
        public readonly Types type ;
        public string line;

        public Token(Types tokentype,string ln,string Error = "") // Token type: Print, Input or Variable. 
        {


            if(tokentype == Types.Variable) // If the token is a variable we finally store the var parameter in the variable member.
            {
                type = tokentype;
                line = ln;
            }else if(tokentype == Types.Error) // If its an unrecognized instruction, we store it as an Error to log later.
            {
                type = tokentype;
                line = Error;
            }
            else // Otherwise we leave it variable uninitialized and just store the tokentype.
            {
                type = tokentype;
                line = ln;
            }
        }
    }
}
