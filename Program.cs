using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MathGame
{
    internal class Program
    {
        public static List<GameRecord> gameRecords = new();
        
        public enum Difficulties
        {
            Easy,
            Medium,
            Hard
        }

        public static Difficulties selectedDifficulty;


        static void Main(string[] args)
        {
            string? expressionOperator;
            bool gameFinished = false;
            

            do
            {
                Console.WriteLine(@"Welcome to the math game!
    Select your option:
    + for the addition game.
    - for the subtraction game.
    / for the division game.
    * for the multiplication game.
    ? for a randomized game.
    R for a record of past games.

Or press Enter to quit." + "\n");
                
                expressionOperator = Console.ReadLine();

                switch(expressionOperator.ToLower())
                {
                    case "":
                        gameFinished = true;
                        break;
                    case "+":
                    case "-":
                    case "/":
                    case "*":
                    case "?":
                        try
                        {
                            SelectDifficulty();
                            DoOperation(expressionOperator);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "r":
                        DisplayRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }

            } while (!gameFinished);                                                                

        }




        static void DoOperation(string expressionOperator)
        {
            bool wrongAnswer = false;
            int score = 0;

            Random rand = new();
            string[] operatorsForRandomized = { "+", "-", "/", "*"};

            Stopwatch stopWatch = new();
            stopWatch.Start();
            TimeSpan elapsedTime;

            string? gameType = "";

            switch (expressionOperator)
            {
                case "+":
                    gameType = "Addition";
                    break;
                case "-":
                    gameType = "Subtraction";
                    break;
                case "/":
                    gameType = "Division";
                    break;
                case "*":
                    gameType = "Multiplication";
                    break;
                case "?":
                    gameType = "Randomized";
                    break;

            }
            while (!wrongAnswer)
            {
                Console.Clear();

                if (gameType == "Randomized")
                    expressionOperator = operatorsForRandomized[rand.Next(0,3)];

                int result = 0;
                switch (expressionOperator)
                {
                    case "+":
                        int number1 = GenerateNumber();
                        int number2 = GenerateNumber();
                        result = number1 + number2;

                        Console.WriteLine($"{gameType}\n{number1} + {number2} = ?");
                        break;
                    case "-":
                        number1 = GenerateNumber();
                        number2 = GenerateNumber();
                        result = number1 - number2;
                        Console.WriteLine($"{gameType}\n{number1} - {number2} = ?");
                        break;
                    case "/":
                        number1 = 0;
                        number2 = 0;

                        bool validNumbers = false;

                        while (!validNumbers)
                        {
                            number1 = GenerateNumber(true);
                            number2 = GenerateNumber(secondDivisionNumber: true);

                            validNumbers = ValidateDivisionNumbers(number1, number2);
                        }
                        result = number1 / number2;

                        Console.WriteLine($"{gameType}\n{number1} / {number2} = ?");
                        break;
                    case "*":
                        number1 = GenerateNumber();
                        number2 = GenerateNumber(multiplication: true);
                        result = number1 * number2;

                        Console.WriteLine($"{gameType}\n{number1} * {number2} = ?");
                        break;
                }
                int userAnswer = ParseInput();

                if (userAnswer != result)
                {
                    stopWatch.Stop();
                    Console.WriteLine($"Incorrect, the answer was {result}. Your score was {score}");
                    Console.WriteLine("Press enter to quit the game or R to try again.");
                    string? userInput = Console.ReadLine();
                    if (userInput.ToLower() == "r")
                    {
                        elapsedTime = stopWatch.Elapsed;
                        gameRecords.Add(new GameRecord(String.Format("{0:c}:{1:c}:{2:c}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds), gameType, score));
                        stopWatch.Restart();
                        score = 0;
                    }
                    else
                        wrongAnswer = true;
                }
                else
                {
                    Console.WriteLine("Correct, press enter to go to the next question");
                    Console.ReadLine();   
                    score++;
                }
            }
            elapsedTime = stopWatch.Elapsed; 
            gameRecords.Add(new GameRecord(elapsedTime.ToString("hh\\:mm\\:ss"), gameType, score));
            
            Console.Clear();
        }

        static int GenerateNumber(bool firstDivisionNumber = false, bool secondDivisionNumber = false, bool multiplication = false)
        {
            Random rand = new();
            if (!firstDivisionNumber && !secondDivisionNumber && !multiplication)
                switch (selectedDifficulty)
                {
                    case Difficulties.Easy:
                        return rand.Next(1, 101);
                    case Difficulties.Medium:
                        return rand.Next(10, 1001);
                    case Difficulties.Hard:
                        return rand.Next(100, 10001);
                    default:
                        throw new ArgumentException("ArgumentException: No difficulty chosen!");
                }
            else if (firstDivisionNumber && !multiplication && !secondDivisionNumber)
                switch (selectedDifficulty)
                {
                    case Difficulties.Easy:
                    case Difficulties.Medium:
                    case Difficulties.Hard:
                        return rand.Next(0, 101);
                    default:
                        throw new ArgumentException("ArgumentException: No difficulty chosen!");
                }
            else if (secondDivisionNumber && !multiplication && !firstDivisionNumber)
                switch (selectedDifficulty)
                {
                    case Difficulties.Easy:
                        return rand.Next(2, 101);
                    case Difficulties.Medium:
                        return rand.Next(10, 1001);
                    case Difficulties.Hard:
                        return rand.Next(20, 10001);
                    default:
                        throw new ArgumentException("ArgumentException: No difficulty chosen!");
                }
            else if (multiplication && !secondDivisionNumber && !firstDivisionNumber)
                switch (selectedDifficulty)
                {
                    case Difficulties.Easy:
                        return rand.Next(2, 20);
                    case Difficulties.Medium:
                        return rand.Next(5, 50);
                    case Difficulties.Hard:
                        return rand.Next(10, 100);
                    default:
                        throw new ArgumentException("ArgumentException: No difficulty chosen!");
                }
            else
                return 0;
        }

        static int ParseInput()
        {
            do
            {
                string? userInput = Console.ReadLine();

                if (!int.TryParse(userInput, out int parsedNumber))
                { 
                    Console.WriteLine("Input is not a number, try again.\r");
                    Console.SetCursorPosition(0, 2);
                    Console.Write("                                                    ");
                    Console.SetCursorPosition(0, 2);
                }
                else
                    return parsedNumber;
            } while (true);
        }


        static void DisplayRecord()
        {
            Console.Clear();
            Console.WriteLine("Time\t\tGame Type\t\tScore");
            for (int i = 0; i < gameRecords.Count; i++)
            {
                Console.WriteLine($"{gameRecords[i].time}\t{gameRecords[i].typeOfGame}\t\t{gameRecords[i].score}");
            }
            Console.WriteLine("");
        }

        static bool ValidateDivisionNumbers(int number1, int number2)
        {
            bool validDividend;
            bool integerDivision;

            if (number1 > 100 || number1 < 0)
                validDividend = false;
            else
                validDividend = true;

            if (number1 % number2 != 0)
                integerDivision = false;
            else
                integerDivision = true;

            if (validDividend && integerDivision)
                return true;
            else
                return false;
        }

        static void SelectDifficulty()
        {
            Console.Clear();
            do
            {

                Console.WriteLine("Select your difficulty:\n 'E' for easy difficulty. \n 'M' for medium difficulty. \n 'H' for hard difficulty.");
                string? userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "e":
                        selectedDifficulty = Difficulties.Easy;
                        return;
                    case "m":
                        selectedDifficulty = Difficulties.Medium;
                        return;
                    case "h":
                        selectedDifficulty = Difficulties.Hard;
                        return;
                    default:
                        Console.WriteLine("Invalid input, try again.");
                        break;
                }
            } while (true);
            
        }
    }
  
}
