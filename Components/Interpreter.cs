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

        private void print(string msg,bool isError = false)
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

        private void input(object? variable)
        {
           variable = Console.ReadLine();
        }


        private void Execute(string instruction,params object[] value)
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
                if(instruction.type == Types.Error)
                {
                    continue;
                }
                else if(instruction.type == Types.Variable)
                {
                    continue;
                }

                Execute(instruction.variable.name, instruction.variable.value);
                
            }
        }
    }
}
