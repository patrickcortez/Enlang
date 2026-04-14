using Enlang.Components.Misc;
using System.Text;
using static Enlang.Utils.Utility;
using static Enlang.Components.Misc.TypeCaster;
using System.ComponentModel.DataAnnotations;

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
        List<Token> Instructions;
        string[] syntax = {"output","input"};

#nullable disable

        public Core(string src)
        {
            if (!File.Exists(src))
            {
                Console.Error.WriteLine($"File {src} does not exist!");
                return;
            }

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

        

        private void Lex() // Lexing File contents line by line.
        {

            using (StreamReader sr = new StreamReader(filepath)) 
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {

                    if (string.IsNullOrWhiteSpace(line)) // skip every newlines
                    {
                        continue;
                    }

                    string current = string.Empty;
                    string[] words = Tokenize(line,'='); // We tokenize the entire line before evaluating
                    

                    if (words.Length < 2) // if the length of words is less than 2, its always an instruction
                    { 

                        

                        if (isSyntax(words[0],ref current, '('))
                        {
                            string instruction = words[0].Remove(words[0].IndexOf('('), words.Length - current.Length); // print("Hello") : print -> 5, ("hello") - 9 over all its 14 - 5
                            string data = words[0].Remove(0, current.Length);
                            
                            if (instruction == "print") // if its a print token
                            {
                                Instructions.Add(new Token(Types.Print,(current,data))); // (print,message)
                            }else if(instruction == "ïnput") // otherise if its an input
                            {
                                Instructions.Add(new Token(Types.Input,(current,data))); // (input,variable_name)
                            }

                        }
                        else // if its not a syntax/instruction we store it as an error.
                        {

                            Instructions.Add(new Token(Types.Error, null, words[0]));

                        }
                    }
                    else
                    {
                        // key and value for variable
                        string key = words[0],
                               type = DetermineDataType(words[1]);

                        object value;
                        
                        //manual Data type determiner
                        if(type == "Integer")
                        {
                            value = CastObject<int>(words[1]);
                        }else if(type == "float")
                        {
                            value = CastObject<float>(words[1]);
                        }else if(type == "Boolean")
                        {
                            value = CastObject<bool>(words[1]);
                        }
                        else
                        {
                            value = words[1];
                        }
                        

                        Instructions.Add(new Token(Types.Variable, (key, value))); // (Variable_Name,Value)
                    }
                }
            
            }
        }

        internal void BeginInterpret()
        {
            Interpreter interpret = new Interpreter(Instructions);
        }
    }
}
