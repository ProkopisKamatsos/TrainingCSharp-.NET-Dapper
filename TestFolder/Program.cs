public static partial class Program
{
    public static void Main()
    {
        HandleThree();
    }

    public static void HandleThree()
    {
        var task = Task.Run(
            () => throw new CustomException("This exception is expected!"));

        try
        {
            task.Wait();
        }
        catch (AggregateException ae)
        {
            foreach (var ex in ae.InnerExceptions)
            {
                // Χειρισμός της προσαρμοσμένης εξαίρεσης.
                if (ex is CustomException)
                {
                    Console.WriteLine(ex.Message);
                }
                // Επανεκτόξευση οποιασδήποτε άλλης εξαίρεσης.
                else
                {
                    throw ex;
                }
            }
        }
    }
}

// Ορισμός της κλάσης CustomException
public class CustomException : Exception
{
    public CustomException(string message) : base(message) { }
}
// Το παράδειγμα εμφανίζει την ακόλουθη έξοδο:
//        This exception is expected!
