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

                if (seperators.Contains(c))
                {
                    tmp.Add(nstr.ToString());
                    nstr.Clear();
                }

                nstr.Append(c);
            }

            return tmp.ToArray();

        }

    }
}
