List<int> DuplicateNumbers(List<int> numbers)
{
    var duplicates = numbers.GroupBy(x => x)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key)
                            .ToList();
    return duplicates;
}