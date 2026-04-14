namespace Enlang.Components
{
    internal class Interpreter
    {
        List<Token> Instructions;

        public Interpreter(List<Token> instructions)
        {
            Instructions = new List<Token>(instructions);
            ReadInstructions();
        }

        private void print(string msg,bool isError = false) //handle output
        {
            if (isError)
            {
                Console.Error.WriteLine($"Error: {msg}");
            }
            else
            {
                Console.WriteLine(msg);
            }
        }

        private void input(object? variable) // handle input
        {
           variable = Console.ReadLine();
        }


        private void Execute(string instruction,params object[] value) // Execute instructions
        {
            if(instruction == "print")
            {
                print((string)value[0]);
            }if(instruction == "input")
            {
                input(value[0]);
            }
        }

        private void ReadInstructions()
        {
            foreach(Token instruction in Instructions)
            {
                if(instruction.type == Types.Error) // print Errors
                {
                    print(instruction.ErrorValue, true);
                }
                else if(instruction.type == Types.Variable)
                {
                    continue; // variables are already handled in the Tokenizer, so no need to go over them again.
                }

                Execute(instruction.variable.name, instruction.variable.value); // print and input
                
            }
        }
    }
}
