using System.Linq.Expressions;

namespace MathGame
{
    internal class Program
    {
        public static List<string> operationsRecord = new();

        static void Main(string[] args)
        {
            string? expressionOperator;
            int number1 = 0;
            int number2 = 0;
            bool gameFinished = false;

            do
            {
                Console.WriteLine(@"Welcome to the math game!
    Select your option:
    + for sums.
    - for subtraction.
    / for divisions.
    * for multiplication.
    R for a record of operations.

Or press Enter to quit." + "\n");
                
                expressionOperator = Console.ReadLine();

                switch(expressionOperator)
                {
                    case "":
                        gameFinished = true;
                        break;
                    case "+":
                    case "-":
                    case "/":
                    case "*":
                        number1 = 0;
                        number2 = 0;
                        GetNumbers(ref number1, ref number2);
                        try
                        {
                            DoOperation(number1, number2, expressionOperator);
                        }
                        catch
                        {

                        }
                        break;
                    case "R":
                        DisplayRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }

            } while (!gameFinished);                                                                

        }




        static void DoOperation(int number1, int number2, string expressionOperator)
        {
            int result = 0;
            switch (expressionOperator)
            {
                case "+":
                    result = number1 + number2;
                    break;
                case "-":
                    result = number1 - number2;
                    break;
                case "/":
                    if (ManageDivisionNumbers(ref number1, ref number2))
                    {
                        result = number1 / number2;
                        break;
                    }
                    else
                        throw new InvalidOperationException("InvalidOperationException: Could not divide the numbers.");             
                case "*":
                    result = number1 * number2;
                    break;
                default:
                    throw new ArgumentException("ArgumentException: Invalid operator.");
            }

            string operation = String.Format("{0} {1} {2} = {3}", number1, expressionOperator, number2, result);        
            Console.WriteLine($"\n Result: {operation} \n");

            operationsRecord.Add(operation);
        }

        static int ParseInput()
        {
            string? userInput = Console.ReadLine();

            if (!int.TryParse(userInput, out int parsedNumber))
                throw new InvalidDataException("InvalidDataException: Input is not a number.");

            return parsedNumber;
        }

        static void GetNumbers(ref int number1, ref int number2)
        {
            do
            {
                Console.Write("First number: ");
                try
                {
                    number1 = ParseInput();
                    break;
                }
                catch (InvalidDataException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);

            do
            {
                Console.Write("Second number: ");
                try
                {
                    number2 = ParseInput();
                    break;
                }
                catch (InvalidDataException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);
        }

        static void DisplayRecord()
        {
            Console.WriteLine("\n Record of past operations: ");
            for (int i = 0; i < operationsRecord.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {operationsRecord[i]}");
            }
            Console.WriteLine("");
        }

        static bool ManageDivisionNumbers(ref int number1, ref int number2)
        {

            bool validDividend = false;
            bool integerDivision = false;
            bool validDivisor = false;

            while (!validDividend && !validDivisor && !integerDivision)
            {
                validDividend = false;
                integerDivision = false;
                validDivisor = false;

                while (!validDividend)
                {
                    if (number1 > 100 || number1 < 0)
                    {
                        Console.Write("Dividend is out of the 0 - 100 range, input a new number for the dividend: ");
                        number1 = ParseInput();
                    }
                    else
                    {
                        validDividend = true;

                    }

                }
                while (!validDivisor)
                {
                    if (number2 == 0)
                    {
                        Console.Write("Divisor cannot be 0, input a new number for the divisor: ");
                        number2 = ParseInput();
                    }
                    else
                    {
                        validDivisor = true;
                        break;
                    }

                }

                while (!integerDivision)
                {
                    if (number1 % number2 != 0)
                    {
                        Console.WriteLine("Division has remainder, try another pair of numbers.");
                        GetNumbers(ref number1, ref number2);
                    }
                    else
                    {
                        integerDivision = true;
                    }
                }
            }
            if (validDividend && validDivisor && integerDivision)
                return true;
            else
                return false;
        }
    }
        
}
