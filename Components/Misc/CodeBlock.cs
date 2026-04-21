using Enlang.Components.Calculate;
using System.Diagnostics;
using System.Text;
using static Enlang.Components.Calculate.Arithmetic;
using static Enlang.Components.Misc.TypeCaster;
using static Enlang.Utils.Utility;
/*
 * if(<Condition>)
 * { # Open
 * 
 *  print("Hello World")
 *  varname="NewVar"
 *  
 * } # Close
 */

// Code blocks begin where the open curly brace is, and ends where the closing curly brace is!
// If a Code Block is false based on its condition, then we go to its elif or else
// Note that only elif's has conditions, and the last code block 'else' will always run if 
// the main if and elifs are all false.

/*
 * Dev Notes:
 * You know the more I work on this, the more stupid my interpreter looks and feels -_-
 * FML... But Alas! I have to finish what I started. Next Project I am implementing an AST tree 
 * that way its less long & complex, more readable and scalable.
 */

namespace Enlang.Components.Misc
{
    internal class Block // code block for Functions, if else and loops
    {
        Dictionary<string, object> _Variables; // main variable map from the interpreter class passed down to this block
        Dictionary<string, object> LocalVariables; // local variables for the block.
        private List<Token> Instructions; // The blocks own list of Instruction Defined between its braces
        bool _Condition; // if blocks condition has been set to true, also an indicator if the block has successfully executed
        string[] CodeBlock; // Storage of the strings before its parsed as Tokens.
        bool debug,IFSuccess;
        string[] operators = { "==", ">", "<", ">=", "<=" };

        private bool isSyntax(string word, ref string current, char terminator) // Syntax Checker
        {
            StringBuilder nstr = new StringBuilder(); // Initialize our string builder.

            foreach (char c in word) // iterate through each character through the entire line
            {

                if (c == terminator) // guard clause to stop loop if we have reached a terminator.
                {
                    break;
                }

                nstr.Append(c); // append every character until we reach our terminator
            }

            if (syntax.Contains(nstr.ToString()))
            {
                current = nstr.ToString();
                return true;
            }


            return false; 
        }

        private bool HasVariables(string data)
        {
            if (debug)
            {
                Debug($"Current Value in inspection: {data}");
            }

            foreach (string key in _Variables.Keys)
            {
                if (data.Contains(key))
                {
                    return true;
                }
            }

            return false;
        }

        private void print(string msg, bool isError = false) //handle output
        {

            msg = ReplaceWords(msg, _Variables.Keys.ToArray(), _Variables, '$');

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

        private object input() // handle input... Duhhh i mean its self explenatory
        {
            object variable;


            variable = Console.ReadLine();

            return variable;

        }

        public Block(Dictionary<string ,object> variables,string[] block,bool isTrue = false,bool isDebug = false) //pass over variables.
        {
            _Variables = new Dictionary<string, object>(variables); // Variable table from original;
            CodeBlock = block;
            _Condition = isTrue; // _Condition is always fault by default.
            debug = isDebug;
            IFSuccess = false;
            if (isTrue) // if the current if block is true then we read and execute. Simple yet its gonna be a long operation, not to mention this is Just a Control Block, we havent reached Function and Loop Blocks T_T...
            {
                ReadBlock();
                ExecuteBlock();
            }
            else
            {
                return;
            }
        }

        public bool BlockExecuted() // An indicator for other classes if the codeblock did get executed.
        {
            return _Condition;
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

        private void HandleCondition(string condition)
        {
            Condition[] conds = TokenizeCondition(condition).ToArray(); // && , ||

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
                        if (_Variables.ContainsKey(evals[0]))
                        {
                            val1 = _Variables[evals[0]];
                        }
                        else
                        {
                            val1 = ConvertValue(dtype1, evals[0]);
                        }

                        if (_Variables.ContainsKey(evals[1]))
                        {
                            val2 = _Variables[evals[1]];
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

        private void ExecuteBlock()
        {
            try
            {
                foreach (Token tok in Instructions)
                {
                    if (tok.type == Types.Print)
                    {
                        print(tok.line);
                    }
                    else if (tok.type == Types.Input)
                    {
                        if (_Variables.ContainsKey(tok.line))
                        {
                            _Variables[tok.line] = input();
                        }
                        else
                        {
                            Debug($"Variable: {tok.line} does not exist!", true);
                            continue;
                        }
                    }
                    else if (tok.type == Types.Variable) // if current token is variable
                    {
                        string[] var = Tokenize(tok.line, '=');
                        string key = var[0].Trim(), value = var[1];
                        string dtype = DetermineDataType(value);

                        if (_Variables.ContainsKey(key))
                        {
                            if (_Variables.ContainsKey(value))
                            {
                                _Variables[key] = _Variables[value];
                                continue;
                            }
                            else
                            {
                                _Variables[key] = ConvertValue(dtype, value);
                                continue;
                            }
                        }
                        else
                        {

                            if (LocalVariables.ContainsKey(value))
                            {
                                LocalVariables.Add(key, LocalVariables[value]);
                                continue;
                            }

                            if (_Variables.ContainsKey(value))
                            {
                                LocalVariables.Add(key, _Variables[value]);
                                continue;
                            }
                            else
                            {
                                LocalVariables.Add(key, value);
                                continue;
                            }
                        }
                    }
                    else if (tok.type == Types.If)
                    {
                        bool isCond = false;

                        if (IFSuccess)
                        {
                            IFSuccess = !IFSuccess;
                        }

                        HandleCondition(tok.line);

                        if (IFSuccess)
                        {
                            _Variables = HandleBlock();
                            continue;
                        }
                    }
                    else if (tok.type == Types.Elif)
                    {
                        HandleCondition(tok.line);

                        if (IFSuccess)
                        {
                            _Variables = HandleBlock();
                            continue;
                        }
                    }
                    else if (tok.type == Types.Else)
                    {
                        HandleCondition(tok.line);

                        if (IFSuccess)
                        {
                            _Variables = HandleBlock();
                            continue;
                        }
                    }

                }
            }catch(Exception ex)
            {
                Debug(ex.Message, true);
            }
        }

        private Dictionary<string,object> HandleBlock()
        {
            Block nblock = new Block(_Variables, CodeBlock, IFSuccess);
            return nblock.GetVariables();
        }

        private void ReadBlock() // Tokenizing and Parsing of the entire code block.
        {
            try
            {
                char[] sep = { '=' };
                string current = string.Empty, previousCondition = string.Empty;
                object? data = null;
                List<string> BlockBuffer = new List<string>();
                Types previous = new Types();
                bool IfBlock = false;
                foreach (string line in CodeBlock)
                {

                    if (string.IsNullOrWhiteSpace(line)) // Ignore Newlines
                    {
                        continue;
                    }

                    if (line.StartsWith('#')) // Ignore Comments
                    {
                        continue;
                    }

                    if ((line.StartsWith('{') && previous == Types.If || previous == Types.Elif || previous == Types.Else) && !IfBlock)
                    {
                        IfBlock = true;
                        continue;
                    }

                    if ((line.StartsWith('{') && previous == Types.If || previous == Types.Elif || previous == Types.Else) && IfBlock) // after the Reader has read all the instructions in the if block we will add the BlockBuffer to the List of Instructions
                    {
                        IfBlock = false;

                        Instructions.Add(new Token(previous, previousCondition, "", BlockBuffer));
                        continue;
                    }



                    string[] words = Tokenize(line, sep);

                    if (words.Length < 2)
                    {
                        if (isSyntax(line, ref current, '('))
                        {
                            string key = words[0].Remove(words[0].IndexOf('('), words[0].Length - current.Length).Trim();
                            string value = words[0].Remove(0, key.Length);
                            value = value.TrimStart('(').TrimEnd(')'); //remove parenthesis
                            value = value.Replace("\"", ""); // remove quotes

                            if (key == "print")
                            {
                                if (IfBlock)
                                {
                                    BlockBuffer.Add(line);
                                    continue;
                                }

                                Instructions.Add(new Token(Types.Print, value));
                                continue;
                            }
                            else if (key == "input")
                            {
                                if (IfBlock)
                                {
                                    BlockBuffer.Add(line);
                                    continue;
                                }

                                Instructions.Add(new Token(Types.Input, value));
                                continue;
                            }
                            else if (key == "if") // Assuming there is no nesting (Just Yet)
                            {
                                previous = Types.If;
                                previousCondition = value.Trim();
                                continue;
                            }
                        }
                        else
                        {
                            Instructions.Add(new Token(Types.Error, line));
                        }

                    }
                    else // Variables
                    {
                        if (IfBlock)
                        {
                            BlockBuffer.Add(line);
                            continue;
                        }

                        Instructions.Add(new Token(Types.Variable, line));
                        continue;

                    }


                }
            }catch(Exception ex)
            {
                Debug(ex.Message, true);
            }
        }

        public Dictionary<string, object> GetVariables() // after execution is done we return back the
        {
            return _Variables;
        }


        // So far i have added the tokenizing of if elif and else in the Core.cs, and its code block, now for the main part, its code block handling, condition parsing and Block execution





    }
}
