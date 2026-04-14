
using static Enlang.Components.Misc.TypeCaster;
using static Enlang.Utils.Utility;
using Enlang.Utils;

namespace Enlang.Components
{
    internal class Interpreter
    {
        Dictionary<string,object> Variables;
        List<Token> Instructions;
        int index = 0;

        public Interpreter(List<Token> instructions)
        {
            Instructions = new List<Token>(instructions);
            Variables = new Dictionary<string, object>();
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

        private object input() // handle input
        {
           object variable;


            while ((variable = Console.ReadLine()) != null)
            {
                if((variable = Console.ReadLine()) == null) 
                {
                    Console.Error.WriteLine("Input Cannot be empty!");

                    Console.Clear();
                    continue;
                }
            }
               
                return variable;

        }

        private void HandleVariable(string line)
        {
            string[] words = Tokenize(line, '=');
            string key = words[0];
            object value;
            string dtype = DetermineDataType(words[1]);


            if (dtype == "Integer")
            {
                value = int.Parse(words[1]);
            }
            else if (dtype == "float")
            {
                value = float.Parse(words[1]);
            }
            else if (dtype == "Boolean")
            {
                value = bool.Parse(words[1]);
            }
            else
            {
                value = words[1];
            }

            Variables.Add(key,value);
        }


        private void Execute(Types type,string line) // Execute instructions
        {
            if(type == Types.Print)
            {
                print(line);
            }
            
            if(type == Types.Input)
            {
               Variables[line] = input();
            }

            if(type == Types.Variable)
            {
                HandleVariable(line);
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
            while (true) // Replaced Foreach with While for better control over the flow
            {
                Token instruction = current();

                if (instruction.type == Types.End)
                {
                    break;
                }

                Execute(instruction.type, instruction.line);
                AdvanceTo();


            }
        }
    }
}
