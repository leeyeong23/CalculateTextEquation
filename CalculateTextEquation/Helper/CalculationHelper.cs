using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateTextEquation.Helper
{
    public static class CalculationHelper
    {
        public static readonly char[] _operators = { '+', '-', '*', '/' };
        public static double Sum(double firstNumber, double secondNumber)
        {
            return firstNumber + secondNumber;
        }

        public static double Minus(double firstNumber, double secondNumber)
        {
            return firstNumber - secondNumber;
        }

        public static double Multiply(double firstNumber, double secondNumber)
        {
            return firstNumber * secondNumber;
        }

        public static double Division(double firstNumber, double secondNumber)
        {
            return firstNumber / secondNumber;
        }

        public static double CalculateBasedOnOperator(string op, double firstNumber, double secondNumber)
        {
            switch (op)
            {
                case "+":
                    return Sum(firstNumber, secondNumber);
                case "-":
                    return Minus(firstNumber, secondNumber);
                case "*":
                    return Multiply(firstNumber, secondNumber);
                case "/":
                    return Division(firstNumber, secondNumber);
                default:
                    throw new Exception("Invalid Operator");
            }
        }

        public static string CombineOperator(string equation)
        {
            equation = equation.Replace("+-", "-");
            equation = equation.Replace("-+", "-");
            equation = equation.Replace("++", "+");

            return equation;
        }

        public static string FindFirstPriorityOperator(string input)
        {
            //check if contain * or /
            if (input.Contains("*") || input.Contains("/"))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '*' || input[i] == '/')
                    {
                        return input[i].ToString();
                    }
                }
            }
            else
            {
                //find operator + or -, +- at index 0 is excluded, consider positive or negative value
                for (int i = 1; i < input.Length; i++)
                {
                    if (!Char.IsDigit(input[i]) && input[i] != '.')
                    {
                        return input[i].ToString();
                    }
                }
            }

            //return empty string if no more operator
            return string.Empty;
        }
    }
}
