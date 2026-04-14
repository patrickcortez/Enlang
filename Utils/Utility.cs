using System;
using System.Collections.Generic;
using System.Text;

namespace Enlang.Utils
{
    internal static class Utility
    {

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
                }

                nstr.Append(c);
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

    }
}
