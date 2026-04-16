using System.Text;
using static Enlang.Utils.Utility;
using static Enlang.Components.Calculate.Arithmetic;
using static Enlang.Components.Misc.TypeCaster;
using Enlang.Components.Calculate;
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

namespace Enlang.Components.Misc
{
    internal class Block // code block for Functions, if else and loops
    {
        Dictionary<string, object> _Variables; // main variable map from the interpreter class passed down to this block
        private List<Token> Instructions; // The blocks own list of Instruction Defined between its braces
        bool _Condition; // if blocks condition has been set to true, also an indicator if the block has successfully executed
        string[] CodeBlock; // Storage of the strings before its parsed as Tokens.

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


            return false; //place holder will implement tomorrow
        }

        public Block(Dictionary<string ,object> variables,string[] block,bool isTrue = false) //pass over variables.
        {
            _Variables = new Dictionary<string, object>(variables); // Variable table from original;
            CodeBlock = block;
            _Condition = isTrue; // _Condition is always fault by default.
            if (isTrue)
            {
                ReadBlock();
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

        private void ReadBlock() // Tokenizing and Parsing of the entire code block.
        {
            char[] sep = { '=' };
            string current = string.Empty;
            object? data = null;
            foreach(string line in CodeBlock)
            {

                if (string.IsNullOrWhiteSpace(line)) // Ignore Newlines
                {
                    continue;
                }

                if (line.StartsWith('#')) // Ignore Comments
                {
                    continue;
                }

                string[] words = Tokenize(line,sep);

                if(words.Length < 2)
                {
                    if(isSyntax(line,ref current, '('))
                    {
                        string key = words[0].Remove(words[0].IndexOf('('), words[0].Length - current.Length).Trim();
                        string value = words[0].Remove(0, key.Length);
                        value = value.TrimStart('(').TrimEnd(')'); //remove parenthesis
                        value = value.Replace("\"",""); // remove quotes

                        if(key == "print")
                        {
                            Instructions.Add(new Token(Types.Print,value));
                            continue;
                        }else if(key == "input")
                        {
                            Instructions.Add(new Token(Types.Input, value));
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
                    string key = words[0].Trim();


                    if (_Variables.ContainsKey(key)) // if the variable already exists
                    {
                        string value = ReplaceWords(words[1], _Variables.Keys.ToArray(), _Variables, '$');
                        if (isArithmetic(value)) // if the expression is an arithmetic
                        {
                            Arithmetic arith = new Arithmetic(value);
                            _Variables[key] = arith.Begin();
                            continue;
                        }
                        else // if not we store the value directly
                        {
                            string dtype = DetermineDataType(value);
                            data = ConvertValue(data, dtype, value);


                            if (_Variables.ContainsKey(value)) // if the expression is a fellow variable just incase
                            {
                                _Variables[key] = _Variables[value];
                                continue;
                            }
                            else // if not store the data normally
                            {
                                _Variables[key] = data;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        string value = ReplaceWords(words[1], _Variables.Keys.ToArray(), _Variables, '$');
                        if (isArithmetic(value)) // if the expression is an arithmetic
                        {
                            Arithmetic arith = new Arithmetic(value);
                            _Variables.Add(key,arith.Begin());
                            continue;
                        }
                        else
                        {
                            string dtype = DetermineDataType(value);
                            data = ConvertValue(data, dtype, value);


                            if (_Variables.ContainsKey(value)) // if the expression is a fellow variable just incase
                            {
                                _Variables.Add(key, _Variables[value]);
                                continue;
                            }
                            else // if not store the data normally
                            {
                                _Variables.Add(key,data);
                                continue;
                            }
                        }
                    }


                }

                
            }
        }

        public Dictionary<string, object> GetVariables() // after execution is done we return back the
        {
            return _Variables;
        }

        // Stuff I have yet to implement: Block Execution
        // If-elif-else Execution
        // Condition Handling

        // I'll Save for now and continue later, Time stamp 4:58  PM





    }
}
