public interface IToolUser
{
    void SetHammer(Hammer hammer);
    void SetSaw(Saw saw);

}
public class Hammer
{
    public void Use()
    {
        System.Console.WriteLine("Hammering Nails");
    }
}
public class Saw
{
    public void Use()
    {
        System.Console.WriteLine("Sawing Wood");
    }
}
public class Builder : IToolUser
{


    private Hammer _hammer;
    private Saw _saw;

    public void BuilderHouse()
    {
        _hammer.Use();
        _saw.Use();
        System.Console.WriteLine("House Build");
    }

    public void SetHammer(Hammer hammer)
    {
        _hammer = hammer;

    }

    public void SetSaw(Saw saw)
    {
        _saw = saw;
    }
}
internal class Program
{
    private static void Main(string[] args)
    {
        Hammer hammer = new Hammer();
        Saw saw = new Saw();
        Builder builder = new Builder();
        builder.SetHammer(hammer);
        builder.SetSaw(saw);



    }
}