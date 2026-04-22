using Enlang.Components;
using System.Text;

namespace Enlang.Utils
{
    internal static class Utility
    {

        public readonly static string[] syntax = { "print", "input" , "if" , "elif" , "else" };
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

        public static Condition[] TokenizeCondition(string data,params string[] seperator)
        {
            StringBuilder nStr = new StringBuilder();
            List<Condition> Conditions = new List<Condition>();

            foreach (char c in data)
            {

                if (seperator.Contains(nStr.ToString()))
                {
                    string Ops = nStr.ToString();
                    if(Ops == "&&")
                    {
                        Conditions.Add(Condition.And);
                        nStr.Clear();
                        continue;
                    }else if(Ops == "||")
                    {
                        Conditions.Add(Condition.Or);
                        nStr.Clear();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }


                if(c == '&' || c == '|')
                {
                    nStr.Append(c);
                }
                else
                {
                    continue;
                }

            }

            return Conditions.ToArray();
        }

        public static string[] TokenizeExpression(string data) // tokenize expression ex: (val==val2)
        {
            List<string> Exp = new List<string>();
            StringBuilder nStr = new StringBuilder();

            foreach(char c in data)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    if (nStr.Length > 1)
                    {
                        Exp.Add(nStr.ToString());
                        nStr.Clear();
                    }
                    continue;
                }
                nStr.Append(c);
               
            }

            if(nStr.Length > 1)
            {
                Exp.Add(nStr.ToString());
            }

            return Exp.ToArray();
        }

        public static string GetOperation(string data)
        {
            StringBuilder nStr = new StringBuilder();

            foreach(char c in data)
            {
                if (char.IsLetterOrDigit(c))
                {
                    continue;
                }

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                nStr.Append(c);
            }

            return nStr.ToString();
        }
        private static int InstanceCount(string data,char[] sep) // Counts the instances of a seperator: The fix to the tokenizing problem in if else conditions
        {
            int instance = 0;
            foreach(char c in data)
            {
                if (sep.Contains(c))
                {
                    instance++;
                }
                else
                {
                    continue;
                }
            }

            return instance;
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

                if (char.IsWhiteSpace(c) && !inQoutes)
                {
                    continue;
                }

                if(c == '#') // halt if we ever encounter a comment
                {
                    break;
                }

                if (inQoutes)
                {
                    nstr.Append(c);
                    continue;
                }

                if (seperators.Contains(c) && InstanceCount(data,seperators) > 2) // '=',',', etc...
                {
                    // If the amount of seperator instances is greater than 1 then we skip adding to string List

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
