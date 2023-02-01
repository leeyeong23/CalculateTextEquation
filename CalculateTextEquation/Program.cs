using CalculateTextEquation.Helper;
using System.Text;

namespace CalculateTextEquation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var testCases = GetTestCasesAndAnswer();
            foreach (var tc in testCases)
            {
                var calculatedResult = Calculate(tc.Key);

                //do rounding up to compare result
                var roundedExpectedResult = Math.Round(tc.Value, 2);
                var roundedCalculatedResult = Math.Round(calculatedResult, 2);
                var isCorrect = roundedExpectedResult == roundedCalculatedResult ? "Correct" : "Wrong";


                Console.WriteLine($"{tc.Key}: Expected result: {roundedExpectedResult}, Calculated result: {roundedCalculatedResult}; {isCorrect}");
                Console.WriteLine("");
            }
        }

        public static double Calculate(string sum)
        {
            var result = DoCalculation(sum);

            if (double.TryParse(result, out double dbResult))
            {
                return double.Parse(result);
            }
            else
            {
                throw new Exception("Error: Invalid Result:" + result);
            }
        }

        private static string DoCalculation(string equation)
        {
            equation = RemoveAllSpacing(equation);
            equation = CalculationHelper.CombineOperator(equation);

            //solve all the bracket equation first
            while (equation.Contains('('))
            {
                equation = CalculateBracketEquationAndBuildNewString(equation);
            }

            //solve multiply and divide first, from left to right
            var nextOperator = CalculationHelper.FindFirstPriorityOperator(equation);
            while (!string.IsNullOrEmpty(nextOperator))
            {
                equation = CalculateEquationAndBuildNewString(equation, nextOperator);
                nextOperator = CalculationHelper.FindFirstPriorityOperator(equation);
            }

            return equation;
        }
        private static string CalculateBracketEquationAndBuildNewString(string input)
        {
            string replacedEquation = input;
            while (replacedEquation.Contains("("))
            {
                var indexOfOpenBracket = replacedEquation.LastIndexOf('(');
                var indexOfCloseBracket = replacedEquation.IndexOf(')', indexOfOpenBracket);

                var equation = replacedEquation.Substring(indexOfOpenBracket + 1, indexOfCloseBracket - indexOfOpenBracket - 1);
                if (equation.Any(x => CalculationHelper._operators.Any(y => y == x)))
                {
                    var newEquation = DoCalculation(equation);
                    replacedEquation = new StringBuilder(replacedEquation).Remove(indexOfOpenBracket, equation.Length + 2).Insert(indexOfOpenBracket, newEquation).ToString();
                }
                else
                {
                    var newEquation = equation.Replace("(", "").Replace(")", "");
                    replacedEquation = new StringBuilder(replacedEquation).Remove(indexOfOpenBracket, equation.Length + 2).Insert(indexOfOpenBracket, newEquation).ToString();
                }

            }

            return CalculationHelper.CombineOperator(replacedEquation);
        }
        private static string CalculateEquationAndBuildNewString(string input, string op)
        {
            var indexOfOperator = input.LastIndexOf(op);

            //get firstDigit
            var sbFirstDigit = new StringBuilder();
            for (int i = indexOfOperator - 1; i >= 0; i--)
            {
                if (Char.IsDigit(input[i]) || input[i] == '.' || input[i] == '-') // allow decimal and negative value at first digit
                {
                    sbFirstDigit.Insert(0, input[i]);
                }
                else
                {
                    break;
                }
            }
            var firstDigit = double.Parse(sbFirstDigit.ToString());

            //get secondDigit
            var sbSecondDigit = new StringBuilder();
            for (int i = indexOfOperator + 1; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]) || input[i] == '.') // allow decimal value
                {
                    sbSecondDigit.Append(input[i]);
                }
                else
                {
                    break;
                }
            }
            var secondDigit = double.Parse(sbSecondDigit.ToString());

            var stringToReplace = $"{firstDigit}{op}{secondDigit}";
            var result = CalculationHelper.CalculateBasedOnOperator(op, firstDigit, secondDigit);

            var newEquation = input.Replace(stringToReplace.ToString(), result.ToString());
            return CalculationHelper.CombineOperator(newEquation);
        }        
        private static string RemoveAllSpacing(string input)
        {
            return input.Replace(" ", "");
        }
       

        #region Test Cases
        private static Dictionary<string, double> GetTestCasesAndAnswer()
        {
            var cases = new Dictionary<string, double>();
            cases.Add("1 + 1", 2);
            cases.Add("2 * 2", 4);
            cases.Add("1 + 2 + 3", 6);
            cases.Add("6 / 2", 3);
            cases.Add("11 + 23", 34);
            cases.Add("11.1 + 23", 34.1);
            cases.Add("1 + 1 * 3", 4);
            cases.Add("( 11.5 + 15.4 ) + 10.1", 37);
            cases.Add("23 - ( 29.3 - 12.5 )", 6.2);
            cases.Add("( 1 / 2 ) - 1 + 1", 0.5);
            cases.Add("10 - ( 2 + 3 * ( 7 - 5 ) )", 2);
            cases.Add("-1 + 1", 0);
            cases.Add("-1 + (-3)", -4);

            return cases;
        }
        #endregion

    }
}