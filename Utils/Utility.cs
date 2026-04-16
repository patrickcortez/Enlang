using System.Text;

namespace Enlang.Utils
{
    internal static class Utility
    {


        public static void Debug(string msg, bool isError = false)
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

        public static string[] Tokenize(string data,params char[] seperators)
        {
            List<string> tmp = new List<string>();
            StringBuilder nstr = new StringBuilder();
            bool inQoutes = false;

            foreach(char c in data)
            {

                if(c == '"')
                {
                    inQoutes = !inQoutes;
                }


                if (inQoutes)
                {
                    nstr.Append(c);
                    continue;
                }

                if (seperators.Contains(c)) // '=',',', etc...
                {
                    tmp.Add(nstr.ToString()); // store to String List once we encounter any Seperator.
                    nstr.Clear();
                    continue;
                }

                nstr.Append(c);
            }

            if(nstr.Length > 0)
            {
                tmp.Add(nstr.ToString());
            }

            return tmp.ToArray();

        }

        public static string TrimTo(string data,int start,int range)
        {
            StringBuilder Nstr = new StringBuilder();

            for(int x = start;x < range; x++)
            {
                Nstr.Append(data[x]);
            }

            return Nstr.ToString();
        }

        public static float Strinf(string data)
        {
            StringBuilder nStr = new StringBuilder();

            foreach(char c in data)
            {
                if(char.IsDigit(c) || c == '.')
                {
                    nStr.Append(c);
                }

                continue;

            }


            return float.Parse(nStr.ToString());
        }

        public static string ReplaceWords(string data, string[] oldWords,Dictionary<string,object> Variables,char prefix)
        {
            List<string> NoldWords = new List<string>(); // $(VariableName)

            foreach(string word in oldWords)
            {
                NoldWords.Add(string.Concat(prefix, word));
            }

            StringBuilder nStr = new StringBuilder(data);

            foreach(string word in NoldWords)
            {
                nStr.Replace(word, Variables[word.Remove(0, 1)].ToString());
            }


            return nStr.ToString();

        }

    }
}
