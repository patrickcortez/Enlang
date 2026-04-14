using System.Text;
using static Enlang.Utils.Utility;

/*
 * Syntaxes:
 *  - output("text")
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

        private bool isSyntax(string word,char terminator) // Syntax Checker
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
                    string[] words = Tokenize(line,'='); // We tokenize the entire line before evaluating

                    if (words.Length < 2) // if the length of words is less than 2, its always an instruction
                    { 

                        

                        if (isSyntax(words[0], '('))
                        {
                            string instruction = words[0].Remove(words[0].IndexOf('('), words[0].IndexOf(')') + 1);
                            if (instruction == "print") // if its a print token
                            {
                                Instructions.Add(new Token(Types.Print));
                            }else if(instruction == "ïnput") // otherise if its an input
                            {
                                Instructions.Add(new Token(Types.Input));
                            }

                        }
                        else // if its not a syntax/instruction we store it as an error.
                        {

                            Instructions.Add(new Token(Types.Error, null, words[0]));

                        }
                    }
                    else
                    {
                        Instructions.Add(new Token(Types.Variable, (words[0], words[1])));
                    }
                }
            
            }
        }
    }
}
