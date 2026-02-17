using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqLibrary
{
  public class ListManager
  {
    public static List<Person> LoadSampleData()
    {
      var output = new List<Person>();

      output.Add(new Person { FirstName = "Tim", LastName = "Corey", Birthday = Convert.ToDateTime("2/25/1970"), YearsExperience = 20 });
      output.Add(new Person { FirstName = "Joe", LastName = "Smith", Birthday = Convert.ToDateTime("3/31/1970"), YearsExperience = 12 });
      output.Add(new Person { FirstName = "Sue", LastName = "Storm", Birthday = Convert.ToDateTime("1/3/1970"), YearsExperience = 1 });
      output.Add(new Person { FirstName = "Sara", LastName = "Jones", Birthday = Convert.ToDateTime("3/6/1970"), YearsExperience = 8 });
      output.Add(new Person { FirstName = "Jamie", LastName = "Doe", Birthday = Convert.ToDateTime("2/18/1970"), YearsExperience = 7 });
      output.Add(new Person { FirstName = "Mary", LastName = "Smith", Birthday = Convert.ToDateTime("1/23/1970"), YearsExperience = 16 });

      return output;
    }




  }
}
