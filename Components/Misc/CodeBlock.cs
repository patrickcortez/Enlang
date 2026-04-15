using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Enlang.Components.Misc
{
    internal class Block // code block for Functions, if else and loops
    {
        Dictionary<string, object> _Variables;
        string[] CodeBlock;

        public Block(Dictionary<string ,object> variables,string[] block) //pass over variables.
        {
            _Variables = new Dictionary<string, object>(variables);
            CodeBlock = block;
        }






    }
}
