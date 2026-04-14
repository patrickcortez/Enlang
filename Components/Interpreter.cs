using Variable = (string key, object value); // for single variable
using Variables = (string key, object[] values); //for arrays

namespace Enlang.Components
{
    internal class Interpreter
    {
        List<Token> Instructions;
        List<Variable> VarMap;
        int index = 0;

        public Interpreter(List<Token> instructions)
        {
            Instructions = new List<Token>(instructions);
            VarMap = new List<Variable>();
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


        private void Execute(Types type,string line) // Execute instructions
        {
            if(type == Types.Print)
            {
                print(line);
            }if(type == Types.Input)
            {
                //PlaceHolder
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

        private void RetreatTo(int range= 1) //for loops, functions and etc...
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
            while (index < Instructions.Count) // Replaced Foreach with While for better control over the flow
            {
                Token instruction = current();

                Execute(instruction.type, instruction.line);
                AdvanceTo();
            }
        }
    }
}
