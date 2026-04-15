using static Enlang.Utils.Utility;
using System;

namespace Enlang.Components.Calculate
{
    internal class Arithmetic
    {
        private string line;
        List<MathToken> tokens;
        int position = 0;
        public Arithmetic(string expr)
        {
            line = expr;
            tokens = new List<MathToken>();

            if (expr != string.Empty)
            {
                Evaluate();
            }
        }

        private void Debug(string msg, bool isError = false)
        {

            if (isError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Arithmetic Error> {msg}");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($">{msg}");
            Console.ResetColor();
        }

        // Control flow
        private char Current()
        {
            if (position >= line.Length) return '\0';
            return line[position];
        }

        private void Advance()
        {
            if (position < line.Length)
            {
                position++;
            }
        }


        private float ReadNumbers() // Read all numbers in the line until we reach a math operator
        {
            int start = position;

            while (char.IsDigit(Current())) // stop if we reach a mathematical operator
            {
                Advance();
            }

            return Strinf(line.Substring(start, position - start)); // return the said number.
        }

        private void Evaluate()
        {
            try
            {
                while (position < line.Length)
                {
                    if (char.IsDigit(Current()))
                    {
                        tokens.Add(new MathToken(MathTypes.Number, ReadNumbers()));
                    }
                    else if (Current() == '+')
                    {
                        tokens.Add(new MathToken(MathTypes.Plus));
                        Advance();
                    }
                    else if (Current() == '-')
                    {
                        tokens.Add(new MathToken(MathTypes.Minus));
                        Advance();
                    }
                    else if (Current() == '*')
                    {
                        tokens.Add(new MathToken(MathTypes.Multiply));
                        Advance();
                    }
                    else if (Current() == '/')
                    {
                        tokens.Add(new MathToken(MathTypes.Divide));
                        Advance();
                    }
                    else if (Current() == '(')
                    {
                        tokens.Add(new MathToken(MathTypes.Lpar));
                        Advance();
                    }
                    else if (Current() == ')')
                    {
                        tokens.Add(new MathToken(MathTypes.Rpar));
                        Advance();
                    }
                    else if (Current() == '^')
                    {
                        tokens.Add(new MathToken(MathTypes.Power));
                        Advance();
                    }
                }

                position = 0;
                tokens.Add(new MathToken(MathTypes.End));
            }catch(Exception ex)
            {
                Debug(ex.Message, true);
            }
        }


        public static bool isArithmetic(string expression)
        {
            bool isArith = false;
            char[] operators = { '-', '+', '*', '/' };

            foreach (char c in expression)
            {

                if (char.IsLetter(c))
                {
                    break;
                }

                if (operators.Contains(c) && expression.IndexOf(c) > 0) // incase the expression is just a negative number.
                {
                    isArith = true;
                    break;
                }

            }

            return isArith;
        }

        private MathToken currentToken()
        {
            return tokens[position];
        }

        private float Parse() // starts processing user expression and recursively parse
        {

            return ParseAdd();
        }

        private float ParseAdd() // Add/Sub handler, basically a ladder to parseBase() aka our recursing parser
        {
            float valueL = ParseMultiply();

            while (currentToken().type == MathTypes.Minus || currentToken().type == MathTypes.Plus) // we recursively add/sub here until we dont have any + - tokens
            {
                MathTypes ops = currentToken().type;
                Advance();
                float valueR = ParseMultiply();

                if (ops == MathTypes.Plus)
                {
                    valueL = valueL + valueR;
                }
                else
                {
                    valueL = valueL - valueR;
                }
            }

            return valueL;
        }

        private float ParseMultiply() // Multiplication handler based in PEMDAS, before passing onto Add, check if there are any * operators
        {
            float valueL = ParsePower();

            while (currentToken().type == MathTypes.Multiply || currentToken().type == MathTypes.Divide) //While its * or /, solve then advance until the 
            {
                MathTypes ops = currentToken().type;
                Advance();
                float valueR = ParsePower();

                if (ops == MathTypes.Multiply)
                {
                    valueL = valueL * valueR;
                }
                else
                {
                    if (valueR == 0)
                    {
                        throw new Exception("Cannot be divided to 0!");
                    }
                    valueL = valueL / valueR;
                }
            }

            return valueL;
        }

        private float ParsePower() // power handler
        {
            float valueL = ParseBase();

            while (currentToken().type == MathTypes.Power)
            {
                MathTypes ops = currentToken().type;
                Advance();
                float valueR = ParsePower(); //exponent

                if (ops == MathTypes.Power)
                {
                    valueL = (float)Math.Pow(valueL, valueR);
                }
            }

            return valueL;
        }

        //Evaluator

        private float ParseBase() // the determiner if a number is under a parenthesis or its a negative or a non negative number by  analyzing each current token type
        {
            if (currentToken().type == MathTypes.Number)
            {
                float value = currentToken().value;
                Advance();
                return value;
            }

            if (currentToken().type == MathTypes.Lpar) // if the current token is a left parenthises, advance then we recurse
            {
                Advance();
                float value = Parse();

                if (currentToken().type != MathTypes.Rpar)
                {
                    throw new Exception("Expression is missing a )");
                }

                Advance(); //skip the right parenthesis then returb the value
                return value;
            }

            if (currentToken().type == MathTypes.Minus) // if its a minus token just return it as negative
            {
                Advance();
                return -ParseBase();
            }

            throw new Exception("Unknown token in expression!");
        }

        public float Begin()
        {
            float tmp = Parse();

            if(currentToken().type != MathTypes.End)
            {
                throw new Exception($"Invalid token in end of Expression: {currentToken().value}");
            }

            return tmp;
        }

    }
}
