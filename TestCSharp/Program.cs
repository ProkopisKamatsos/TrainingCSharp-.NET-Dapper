// Define an interface for sound
public interface ISound
{
    void MakeSound();
}

// Define an interface for movement
public interface IMovable
{
    void Move();
}

// Implement the ISound and IMovable interfaces in different classes
public class Dog : ISound, IMovable
{
    public void MakeSound()
    {
        Console.WriteLine("The dog barks very loud.");
    }

    public void Move()
    {
        Console.WriteLine("The dog runs.");
    }
}

public class Cat : ISound, IMovable
{
    public void MakeSound()
    {
        Console.WriteLine("The cat meows.");
    }

    public void Move()
    {
        Console.WriteLine("The cat jumps.");
    }
}

public class Cow : ISound, IMovable
{
    public void MakeSound()
    {
        Console.WriteLine("The cow moos.");
    }

    public void Move()
    {
        Console.WriteLine("The cow walks.");
    }
}

public class Doorbell : ISound
{
    public void MakeSound()
    {
        Console.WriteLine("The doorbell rings.");
    }
}

public class CarHorn : ISound
{
    public void MakeSound()
    {
        Console.WriteLine("The car horn honks.");
    }
}

class Program
{
    static void Main()
    {
        // Create an array of ISound objects
        ISound[] soundObjects = new ISound[5];

        ISound object1 = new Dog();
        ISound object2 = new Cat();
        ISound object3 = new Cow();
        ISound object4 = new Doorbell();
        ISound object5 = new CarHorn();

        soundObjects[0] = object1;
        soundObjects[1] = object2;
        soundObjects[2] = object3;
        soundObjects[3] = object4;
        soundObjects[4] = object5;

        // Demonstrate polymorphism with ISound
        Console.WriteLine("Demonstrating ISound polymorphism:");
        foreach (ISound currentObject in soundObjects)
        {
            currentObject.MakeSound();
        }

        // Create an array of IMovable objects
        IMovable[] movableObjects = new IMovable[3];

        IMovable movableObject1 = new Dog();
        IMovable movableObject2 = new Cat();
        IMovable movableObject3 = new Cow();

        movableObjects[0] = movableObject1;
        movableObjects[1] = movableObject2;
        movableObjects[2] = movableObject3;

        // Demonstrate polymorphism with IMovable
        Console.WriteLine("\nDemonstrating IMovable polymorphism:");
        foreach (IMovable currentObject in movableObjects)
        {
            currentObject.Move();
        }

        // Demonstrate objects with single and multiple interfaces
        Console.WriteLine("\nDemonstrating objects with single and multiple interfaces:");
        Dog dog = new Dog();
        Cat cat = new Cat();
        Cow cow = new Cow();
        Doorbell doorbell = new Doorbell();
        CarHorn carHorn = new CarHorn();

        dog.MakeSound();
        dog.Move();

        cat.MakeSound();
        cat.Move();

        cow.MakeSound();
        cow.Move();

        doorbell.MakeSound();

        carHorn.MakeSound();
    }
}
