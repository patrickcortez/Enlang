namespace Enlang.Components
{
    internal enum Types // Declaring our Token types: Print, Variable and Input
    {
        Print, //Outputing text
        Variable, //Storing data
        Input, // Acquiring user input
        If, // if block
        Elif, // else if block
        Else, //else block
        Error,
        End
    }

    internal enum Condition
    {
        Or, // ||
        And, // &&
        Xor, // !|
        None
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
        public List<string> BlockBuffer; // Functions, Loops and Control Flow Container
        public readonly Condition condition; // if the current token is a condition if(<condition>)

        public Token(Types tokentype, string ln, string Error = "", List<string>? TmpBlockBuffer = null) // Token type: Print, Input or Variable. 
        {


            if(tokentype == Types.Variable) // If the token is a variable we finally store the var parameter in the variable member.
            {
                type = tokentype;
                line = ln;
            }else if(tokentype == Types.Error) // If its an unrecognized instruction, we store it as an Error to log later.
            {
                type = tokentype;
                line = Error;
            }else if(tokentype == Types.If || tokentype == Types.Elif || tokentype == Types.Else)
            {
                type = tokentype;
                line = ln;
                BlockBuffer = new List<string>(TmpBlockBuffer);
            }
            else // Otherwise we leave it variable uninitialized and just store the tokentype.
            {
                type = tokentype;
                line = ln;
            }
        }
    }
}
