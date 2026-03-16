public delegate void Notify(string message);

public class Program
{
    public static void Main()
    {
        // Create an instance of the delegate using a named method
        Notify notify = new Notify(NotificationService.SendNotification);

        // Invoke the delegate
        notify("Hello, World!");
    }
}

public class NotificationService
{
    // Method that matches the delegate's signature
    public static void SendNotification(string message)
    {
        Console.WriteLine("Notification sent: " + message);
    }
}