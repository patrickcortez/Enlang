namespace Enlang.Components.Arithmetic
{
    internal class Arithmetic
    {

        public Arithmetic(string expr)
        {
            isArithmetic(expr);
        }

        private static bool isArithmetic(string expression)
        {
            bool isArith = true;
            char[] operators = { '-', '+','*','/' };
            
            foreach(char c in expression)
            {

                if (char.IsLetter(c))
                {
                    isArith = false;
                    break;
                }

                if (!operators.Contains(c) && expression.IndexOf(c) > 0) // incase the expression is just a negative number.
                {
                    isArith = false;
                    break;
                }

            }

            return isArith;
        }
    }
}
