
using static Enlang.Components.Misc.TypeCaster;
using static Enlang.Utils.Utility;
using Enlang.Components.Calculate;
using System.Text;

namespace Enlang.Components
{
    internal class Interpreter
    {
        Dictionary<string,object> Variables;
        List<Token> Instructions;
        int index = 0;
        bool debug;

        private void Debug(string msg)
        {
            Console.WriteLine(msg);
        }

        public Interpreter(List<Token> instructions,bool isdebug = false)
        {
            debug = isdebug;
            Instructions = new List<Token>(instructions);
            Variables = new Dictionary<string, object>();
            ReadInstructions();
        }

        private void print(string msg,bool isError = false) //handle output
        {

            msg = ReplaceWords(msg, Variables.Keys.ToArray(),Variables,'$');

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


            variable = Console.ReadLine();    
               
            return variable;

        }

        private object ConvertValue(object value, string dtype, string val) // Convert Value to its data type
        {

            if (dtype == "Integer") // int
            {
                value = int.Parse(val);
            }
            else if (dtype == "float") // float
            {
                value = float.Parse(val);
            }
            else if (dtype == "Boolean") // boolean
            {
                value = bool.Parse(val);
            }
            else // string is our default
            {
                val = val.TrimStart('"').TrimEnd('"'); // if its a string we automatically remove the qoutes, so its clean.
                value = val;

            }

            return value;
        }


        private void HandleVariable(string line)
        {
           
            string[] words = Tokenize(line, '=');
            string key = words[0];
            object value = new object();
            string dtype = DetermineDataType(words[1]);

            if (!Arithmetic.isArithmetic(words[1])) // if the line is not Arithmetic then we simply store the variable name and its value
            {
                if (!Variables.ContainsKey(key))
                {

                    if (!Variables.ContainsKey(words[1]))
                    {
                        value = ConvertValue(value, dtype, words[1]);

                        Variables.Add(key, value); // add the new variable in the
                    }
                    else
                    {
                        value = Variables[words[1]];

                        Variables.Add(key, value);
                    }
                }
                else //if Variable already exists
                {

                    if (!Variables.ContainsKey(words[1]))
                    {
                        value = ConvertValue(value, dtype, words[1]);

                        Variables[key] = value; // change the current variables value to the new one.
                    }
                    else
                    {
                        value = Variables[words[1]];
                        Variables[key] = value;
                    }
                }
            }
            else
            {
                string tmp = words[1].Trim();
                if (Variables.Count > 0) // if variable count is greater than 0 then we start replacing variables in the expression.
                {
                    tmp = ReplaceWords(words[1], Variables.Keys.ToArray(), Variables, '$').Trim();
                }

                if (!Variables.ContainsKey(key)) // if the variable already exists replace their value.
                {
                    Arithmetic arith = new Arithmetic(words[1]);
                    Variables.Add(key, arith.Begin());
                }
                else
                {
                    Arithmetic arith = new Arithmetic(words[1]);
                    Variables[key] = arith.Begin();
                }
            }
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

            if(type == Types.Error && debug)
            {
                if (line.Length < 1)
                {
                    return;
                }
                Debug($"Error: {line}");
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

                if (debug)
                {
                    Debug($"Current Instruction: {instruction.type.ToString()}  , Instruction Line: {instruction.line}");
                }

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
