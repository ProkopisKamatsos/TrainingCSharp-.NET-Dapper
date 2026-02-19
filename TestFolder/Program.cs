List<int> evenNumbers = [45, 85, 54, 78, 99];
PrintEvenNumbers(evenNumbers);
void PrintEvenNumbers(List<int> numbers)
{
    foreach (var n in numbers)
    {
        if (n % 2 == 0)
        {
            Console.WriteLine(n);
        }
    }
}
