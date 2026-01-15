namespace ImplementInterfaces
{
    public class Teacher : IPerson
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; } = 0;

        public void DisplayInfo()
        {
            Console.WriteLine($"Teacher Name: {Name}, Age: {Age}");
        }
    }
}