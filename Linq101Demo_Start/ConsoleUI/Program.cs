using LinqLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
  class Program
  {
    static void Main(string[] args)
    {
      List<Person> people = ListManager.LoadSampleData();
            // people= people.OrderByDescending(p => p.YearsExperience).ThenBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
            //people = people.Where(x => x.YearsExperience > 5).ToList();
            //int yearsTotal= people.Sum(p => p.YearsExperience);
            foreach (var person in people)
      {
        Console.WriteLine($"{ person.FirstName } { person.LastName } ({ person.Birthday.ToShortDateString() }): Experience { person.YearsExperience }");
      }

      Console.ReadLine();
    }
  }
}
