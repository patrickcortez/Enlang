namespace Enlang.Components
{
    internal class Interpreter
    {
        List<Token> Instructions;
        int index = 0;

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


        // Controls
        private Token current()
        {
            return Instructions[index];
        }

        private void AdvanceTo(int range= 1)
        {
            int tmp = index + range;
            if(tmp < Instructions.Count)
            {
                index+=range;
            }
        }

        private void RetreatTo(int range= 1)
        {
            int tmp = index - range;
            if(index >= 0)
            {
                index -= range;
            }
        }

        // Start Exectuting Instructions
        private void ReadInstructions()
        {
            while (true) // Replaced Foreach with While for better control over the flow
            {

            }
        }
    }
}
