using System;
using System.ComponentModel.DataAnnotations;
using ConsoleCalculator;

while (true)
{
    Calculator calculator = new Calculator();

    Console.WriteLine("=== Calculator ===");
    Console.WriteLine("1) Add");
    Console.WriteLine("2) Subtract");
    Console.WriteLine("3) Multiply");
    Console.WriteLine("4) Divide");
    Console.WriteLine("0) Exit");
    Console.Write("Choose an option (0-4): ");

    // Read menu choice safely
    string? choiceInput = Console.ReadLine();
    if (!int.TryParse(choiceInput, out int choice) || choice < 0 || choice > 4)
    {
        Console.WriteLine("Invalid menu option. Try again.\n");
        continue;
    }

    if (choice == 0)
    {
        Console.WriteLine("Calculator closed.");
        break;
    }

    // Read numbers safely
    double num1 = ReadDouble("Enter first number: ");
    double num2 = ReadDouble("Enter second number: ");

    double result;

    try
    {
        switch (choice)
        {
            case 1:
                result = calculator.Add(num1, num2);
                break;
            case 2:
                result = calculator.Subtract(num1, num2);
                break;
            case 3:
                result = calculator.Multiply(num1, num2);
                break;
            case 4:
                result = calculator.Divide(num1, num2);
                break;
            default:
                Console.WriteLine("Invalid option.");
                continue;
        }

        Console.WriteLine($"Result: {result}\n");
    }
    catch (DivideByZeroException ex)
    {
        Console.WriteLine(ex.Message + "\n");
    }

 
}

static double ReadDouble(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (double.TryParse(input, out double value))
            return value;

        Console.WriteLine("Invalid number. Try again.");
    }
}
