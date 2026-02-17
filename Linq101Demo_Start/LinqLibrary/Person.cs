using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqLibrary
{
  public class Person
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int YearsExperience { get; set; }
    public DateTime Birthday { get; set; }
    public string FullName {
      get
      {
        return $"{ FirstName } { LastName }";
      } 
    }
  }
}
