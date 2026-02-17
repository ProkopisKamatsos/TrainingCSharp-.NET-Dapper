
    Console.WriteLine("Hello, World!");

    var p1 = new Person("Scott", " Hanselman", new DateOnly(1970, 01, 01));

    var p2 = new Person("David", " Fowler", new DateOnly(1986, 01, 01));

    p1.Pets.Add(new Dog("Rex"));
    p1.Pets.Add(new Cat("Felix"));

    p2.Pets.Add(new Cat("Mittens"));

    List<Person> people = [p1, p2];

    foreach (var person in people)
    {
      System.Console.WriteLine(person);
      foreach (var pet in person.Pets)
      {
      System.Console.WriteLine($"    {pet}");
        
      }
    }


public class Person(string firstname, string lastname, DateOnly birthDate)
{
  public string FirstName { get; } = firstname;
  public string LastName { get; } = lastname;
  public DateOnly BirthDate { get; } = birthDate;

  public List<Pet> Pets { get; } = new();

  public override string ToString()
  {
    return $"{FirstName} {LastName} (born {BirthDate})";
  }
}

public abstract class Pet(string firstname)
{
  public string FirstName { get; } = firstname;

  public abstract string MakeNoise();

  public override string ToString()
  {
    return $"{FirstName} says {MakeNoise()} and is a {GetType().Name}";
  }
  
}

public class Dog(string firstname) : Pet(firstname)
{
  public override string MakeNoise()
  {
    return "bark";
  }
  public override string ToString()
  {
    return $"{FirstName} says {MakeNoise()}";
  }
}

public class Cat(string firstname) : Pet(firstname)
{
  public override string MakeNoise()
  {
    return "meow";
  }
  public override string ToString()
  {
    return $"{FirstName} says {MakeNoise()}";
  } 
}




