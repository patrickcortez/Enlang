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

        private bool isSyntax(string word)
        {
            return false; //place holder will implement tomorrow
        }

        private void Lex()
        {

            using (StreamReader sr = new StreamReader(filepath)) 
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    string[] words = Tokenize(line,'=');
                    
                    //Evaluator PlaceHolder
                }
            
            }
        }
    }
}
