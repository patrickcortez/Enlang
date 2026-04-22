
using static Enlang.Components.Misc.TypeCaster;
using static Enlang.Utils.Utility;
using Enlang.Components.Calculate;
using System.Text;
using Enlang.Components.Misc;

namespace Enlang.Components
{
    internal class Interpreter
    {
        Dictionary<string,object> Variables;
        List<Token> Instructions;
        string[] operators = { "==", ">", "<", ">=", "<=" };
        int index = 0;
        bool IFSuccess = false;
        bool debug;

        private void Debug(string msg,bool isError = false)
        {

            if (isError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Interpreter Error> {msg}");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"> {msg}");
            Console.ResetColor();
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

            if (Arithmetic.isArithmetic(msg))
            {
                if (debug)
                {
                    Debug($"Current Arithmetic: {msg}");
                }
                Arithmetic arith = new Arithmetic(msg.Trim());
                msg = arith.Begin().ToString();
            }

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

        private object ConvertValue(string dtype, string val) // Convert Value to its data type
        {
            object value;
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

        private bool HasVariables(string data)
        {
            if (debug)
            {
                Debug($"Current Value in inspection: {data}");
            }

            foreach(string key in Variables.Keys)
            {
                if (data.Contains(key))
                {
                    return true;
                }
            }

            return false;
        }


        private void HandleVariable(string line)
        {
            try
            {
                string[] words = Tokenize(line, '=');
                string key = words[0].Trim();
                object value = new object();
                string dtype = DetermineDataType(words[1]);

                if (debug)
                {
                    Debug($"Current Variable Name: {words[0]} , Value: {words[1]}");
                }

                if (!Arithmetic.isArithmetic(words[1]) && !HasVariables(words[1])) // if the line is not Arithmetic then we simply store the variable name and its value
                {
                    if (!Variables.ContainsKey(key))
                    {

                        if (!Variables.ContainsKey(words[1]))
                        {
                            value = ConvertValue(dtype, words[1]);

                            Variables.Add(key, value); // add the new variable in the

                            if (debug)
                            {
                                Debug($"New Variable name: {key} , Value: {Variables[key]} , Data-type: {dtype}");
                            }
                        }
                        else
                        {
                            value = Variables[words[1]];

                            Variables.Add(key, value);

                            if (debug)
                            {
                                Debug($"Variable name: {key} , Value: {Variables[key]} , Data-type: {dtype}");
                            }
                        }
                    }
                    else //if Variable already exists
                    {

                        if (!Variables.ContainsKey(words[1]))
                        {
                            value = ConvertValue(dtype, words[1]);

                            Variables[key] = value; // change the current variables value to the new one.

                            if (debug)
                            {
                                Debug($"Existing Variable name: {key} , Value: {Variables[key]} , Data-type: {dtype}");
                            }
                        }
                        else
                        {
                            value = Variables[words[1]];
                            Variables[key] = value;

                            if (debug)
                            {
                                Debug($"Existing Variable name: {key} , Value: {Variables[key]} , Data-type: {dtype}");
                            }
                        }
                    }
                }
                else
                {

                    string tmp = words[1].Trim();
                    if (Variables.Count > 0) // if variable count is greater than 0 then we start replacing variables in the expression.
                    {
                        tmp = ReplaceWords(words[1], Variables.Keys.ToArray(), Variables, '$').Replace(" ","");
                    }

                    if (debug)
                    {
                        Debug($"Current Arithmetic: {tmp} , Key: {key}");
                    }

                    
                    if (!Variables.ContainsKey(key)) // if it doesnt exist then we add it to our Variable Table
                    {


                        if (debug)
                        {
                            Debug($"New Variable name:{key}");
                        }

                        Arithmetic arith = new Arithmetic(tmp);
                        Variables.Add(key, arith.Begin());

                        
                    }
                    else  // if the variable already exists replace their value.
                    {
                        if (debug)
                        {
                            Debug($"Variable name: {key} , Value: {tmp} , Data-type: {dtype}");
                        }

                        Arithmetic arith = new Arithmetic(tmp);
                        Variables[key] = arith.Begin();

                    }

                }
            }catch(Exception ex)
            {
                string exceptionMSG = $@"
                    Cause: {ex.StackTrace}

                    Exception Message: {ex.Message}

                ";
                Debug(exceptionMSG, true);
            }
        }

        private Dictionary<string,object> HandleBlock(List<string> BlockBuffer) // Handles Block Execution.
        {

            if (debug)
            {
                Debug($"Block Running with a Buffer size of {BlockBuffer.Count}");
            }

            Block tmp = new Block(Variables, BlockBuffer.ToArray(),IFSuccess);
            return tmp.GetVariables();
        }

        private void HandleCondition(string condition)
        {
            List<Condition> conds = new List<Condition>();

            if (condition.Contains('&') || condition.Contains('|'))
            {
               conds.AddRange(TokenizeCondition(condition, "&&", "||").ToList()); // && , ||
            }


            if (debug)
            {
                Debug($"Current Condition: {condition}");
            }

            if (conds.Count() < 1)
            {
                string expression = condition.TrimStart('(').TrimEnd(')').Replace(" ", "");
                string value1, value2;
                string[] evals = TokenizeExpression(expression);
                string currentOps = GetOperation(condition);



                if (operators.Contains(currentOps))
                {

                    object val1, val2;
                    string dtype1 = DetermineDataType(evals[0]), dtype2 = DetermineDataType(evals[1]);


                    if (HasVariables(condition))
                    {
                        if (Variables.ContainsKey(evals[0]))
                        {
                            val1 = Variables[evals[0]];
                        }
                        else
                        {
                            val1 = ConvertValue(dtype1, evals[0]);
                        }

                        if (Variables.ContainsKey(evals[1]))
                        {
                            val2 = Variables[evals[1]];
                        }
                        else
                        {
                            val2 = ConvertValue(dtype2, evals[1]);
                        }
                    }
                    else
                    {
                        val1 = ConvertValue(dtype1, evals[0]);
                        val2 = ConvertValue(dtype2, evals[1]);
                    }

                    if (currentOps == "==")
                    {
                        if (val1 == val2)
                        {
                            IFSuccess = true;
                        }
                        else
                        {
                            IFSuccess = false;
                        }
                    }
                    else if (currentOps == ">")
                    {
                        if ((float)val1 > (float)val2)
                        {
                            IFSuccess = true;
                        }
                        else
                        {
                            IFSuccess = false;
                        }
                    }
                    else if (currentOps == "<")
                    {
                        if ((float)val1 < (float)val2)
                        {
                            IFSuccess = true;
                        }
                        else
                        {

                        }
                    }


                }
            }
        }


        private void Execute(Types type,string line,List<string>? BlockBuffer = null) // Execute instructions
        {
            try
            {
                if (debug)
                {
                    Debug($"Current Instruction: {type}");
                }

                if (type == Types.Print)
                {
                    print(line);
                }

                if (type == Types.Input)
                {
                    Variables[line] = input();
                }

                if (type == Types.Variable)
                {
                    HandleVariable(line);
                }

                if (type == Types.If)
                {

                    if (debug)
                    {
                        Debug($"Current If: {line}");
                    }

                    if (IFSuccess)
                    {
                        IFSuccess = !IFSuccess;
                    }


                    HandleCondition(line);



                    if (IFSuccess)
                    {

                        if (debug)
                        {
                            Debug("Block Buffer Started");
                        }

                        Variables = HandleBlock(BlockBuffer);
                    }

                }

                if (type == Types.Elif && !IFSuccess)
                {

                    if (debug)
                    {
                        Debug($"Current Elif: {line}");
                    }

                    HandleCondition(line);
                    if (IFSuccess)
                    {
                        Variables = HandleBlock(BlockBuffer);
                    }
                }

                if (type == Types.Else && !IFSuccess)
                {
                    if (debug)
                    {
                        Debug($"Current Else: {line}");
                    }

                    IFSuccess = true;
                    Variables = HandleBlock(BlockBuffer);
                    IFSuccess = false;
                    
                }

                if (type == Types.Error && debug)
                {
                    if (line.Length < 1)
                    {
                        return;
                    }
                    Debug($"Error: {line}");
                }
            }catch(Exception ex)
            {
                string exceptionMSG = $@"
                    Cause: {ex.StackTrace}

                    Exception Message: {ex.Message}

                ";
                Debug(exceptionMSG, true);
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


                if (instruction.type == Types.If || instruction.type == Types.Elif || instruction.type == Types.Else)
                {
                    Execute(instruction.type, instruction.line,instruction.BlockBuffer);
                }
                else
                {
                    Execute(instruction.type, instruction.line);
                }

                AdvanceTo();


            }
        }
    }
}
