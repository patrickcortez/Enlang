using System.Data;
using System.Text;
using static Enlang.Utils.Utility;

/*
 * Syntaxes:
 *  - print("text")
 *  - input(var)
 *  - name=value
 */

namespace Enlang.Components
{
    internal class Core
    {
        readonly string filepath;
        List<string> lines;
        List<string> BlockBuffer;
        List<Token> Instructions;
        bool debug;

#nullable disable

        public Core(string src,bool isDebug = false)
        {
            if (!File.Exists(src)) // if file does not exist 
            {
                Console.Error.WriteLine($"File {src} does not exist!");
                return;
            }

            if (Path.GetExtension(src) != ".enl") // if file isn't valid we halt exeu
            {
                Console.Error.WriteLine($"File: {src} is not a .enl file!");
                return;
            }

            debug = isDebug;
            filepath = src;
            Instructions = new List<Token>();
            Lex();
        }

        private bool isSyntax(string word,ref string current,char terminator) // Syntax Checker
        {
            StringBuilder nstr = new StringBuilder(); // Initialize our string builder.

            foreach(char c in word) // iterate through each character through the entire line
            {
                
                if(c == terminator) // guard clause to stop loop if we have reached a terminator.
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

        private void Debug(string msg,bool isError = false)
        {

            if (isError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Lexer Error> {msg}");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"> {msg}");
            Console.ResetColor();
        }

        private void Lex() // Lexing File contents line by line.
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    string line,prevCondition = string.Empty;
                    Types previous = new Types();
                    bool IFBlock = false;
                    while ((line = sr.ReadLine()) != null)
                    {

                        if (debug)
                        {
                            Debug($"Current Line: {line}");
                        }

                        if (string.IsNullOrWhiteSpace(line)) // skip every newlines
                        {
                            continue;
                        }

                        if (line.Trim().StartsWith('#')) //we ignore comments
                        {
                            continue;
                        }

                        if (line.Trim().StartsWith('{') && (previous == Types.If || previous == Types.Elif || previous == Types.Else))
                        {
                            IFBlock = true;
                            continue;
                        }

                        if(line.Trim().StartsWith('}') && IFBlock == true)
                        {
                            Instructions.Add(new Token(previous,prevCondition,"",BlockBuffer));
                            prevCondition = string.Empty;
                            BlockBuffer.Clear();
                            IFBlock = false;
                            continue;
                        }

                        string current = string.Empty;
                        string[] words = Tokenize(line, '='); // We tokenize the entire line before evaluating


                        if (words.Length < 2) // if the length of words is less than 2, its always an instruction
                        {

                            if (isSyntax(words[0], ref current, '(')) // bug, somewhere in this line
                            {
                                string instruction = words[0].Remove(words[0].IndexOf('('), words[0].Length - current.Length); // print("Hello") : print -> 5, ("hello") - 9 over all its 14 - 5
                                string data = words[0].Remove(0, current.Length);
                                data = data.TrimStart('(').TrimEnd(')');
                                
                                if (data.StartsWith('"'))
                                {
                                    data = data.TrimStart('"').TrimEnd('"');
                                }
                                
                                

                                if (debug)
                                {
                                    Debug($"Current instruction: {instruction} , Current data: {data}");
                                }

                                if (instruction == "print") // if its a print token
                                {
                                    if (IFBlock)
                                    {
                                        BlockBuffer.Add(line);
                                        continue;
                                    }
                                    Instructions.Add(new Token(Types.Print, data)); // (message)
                                   
                                }
                                else if (instruction == "input") // otherise if its an input
                                {
                                    if (IFBlock)
                                    {
                                        BlockBuffer.Add(line);
                                        continue;
                                    }
                                    Instructions.Add(new Token(Types.Input, data)); // (variable_name)
                                    
                                }
                                else if(instruction == "if") // control flow
                                {

                                    if (IFBlock) // well store an nested if for now
                                    {
                                        BlockBuffer.Add(line);
                                        continue;
                                    }

                                    data = data.Trim();
                                    prevCondition = data;
                                    IFBlock = true;
                                    previous = Types.If;
                                    continue;

                                }
                                else if(instruction == "elif") // else if
                                {
                                    if (IFBlock) // well store an nested if for now
                                    {
                                        BlockBuffer.Add(line);
                                        continue;
                                    }

                                    data = data.Trim();
                                    prevCondition = data;
                                    IFBlock = true;
                                    previous = Types.Elif;
                                    continue;
                                }
                                else if(instruction == "else") // else
                                {
                                    if (IFBlock) // well store an nested if for now
                                    {
                                        BlockBuffer.Add(line);
                                        continue;
                                    }
                                    IFBlock = true;
                                    previous = Types.Else;
                                    continue;
                                }
                                
                            }
                            else // if its not a syntax/instruction we store it as an error.
                            {
                                if (debug)
                                {
                                    Debug($"Unknown Line: {line}");
                                }

                                if (IFBlock)
                                {
                                    BlockBuffer.Add(line);
                                    continue;
                                }

                                Instructions.Add(new Token(Types.Error,line));

                            }
                        }
                        else
                        {
                            // key and value for variable
                            if (debug)
                            {
                                Debug($"Current Variable: {line}");
                            }

                            if (IFBlock)
                            {
                                BlockBuffer.Add(line);
                                continue;
                            }

                            Instructions.Add(new Token(Types.Variable, line)); // (Variable_Name,Value)
                        }

                        
                    }

                    Instructions.Add(new Token(Types.End, "null")); // signifies an end of a program

                }

            }catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Environment.Exit(1);
            }
        }

        internal void BeginInterpret()
        {
            if (debug)
            {
                Debug("Interpretting Began!");
            }

            Interpreter interpret = new Interpreter(Instructions,debug);
        }
    }
}
