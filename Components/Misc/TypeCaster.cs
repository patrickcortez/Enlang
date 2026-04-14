using System;
using System.Collections.Generic;
using System.Text;

namespace Enlang.Components.Misc
{
    internal static class TypeCaster
    {
        public static T CastObject<T>(object Data) // Automatic typecaster
        {
            if(Data == null)
            {
                throw new ArgumentNullException("Value cannot be null!");
            }

            try
            {
                return (T)Convert.ChangeType(Data, typeof(T));
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Cannot cast: {Data} to {typeof(T).Name}");
            }
        }

        public static string DetermineDataType(string data)
        {
            string type = string.Empty;
            foreach(char c in data)
            {
                if (char.IsDigit(c))
                {
                    type = "Integer";
                }

                if(c == '.' && type == "Integer")
                {
                    type = "float";
                }
            }

            if(data == "true" || data == "false")
            {
                type = "Boolean";
            }
            else if(type != "Integer" || type != "float" || type != "Boolean")
            {
                type = "String";
            }


            return type;
        }

    }
}
