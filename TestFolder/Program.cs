bool Duplicates(List<int> numbers)
{
    var sort = numbers.OrderBy(x => x).ToList();
    for (int i = 0; i < numbers.Count - 1; i++)
    {
        if (numbers[i] == numbers[i + 1])
        {
            return true;
        }
    }
    return false;
}