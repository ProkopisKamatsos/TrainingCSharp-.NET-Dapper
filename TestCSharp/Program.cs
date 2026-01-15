public interface IBorrowable
{
    bool IsAvailable { get; }
    void Borrow();
}
public class BorrowableBook : IBorrowable
{
    public string Title { get; set; }
    public bool IsAvailable { get; private set; } = true;

    public BorrowableBook(string title)
    {
        Title = title;
    }

    public void Borrow()
    {
        if (IsAvailable)
        {
            IsAvailable = false;
            Console.WriteLine($"You have borrowed \"{Title}\".");
        }
        else
        {
            Console.WriteLine($"\"{Title}\" is already borrowed.");
        }
    }
}
public class Library
{
    private IBorrowable _item;

    public Library(IBorrowable item)
    {
        _item = item;
    }

    public void BorrowItem()
    {
        if (_item.IsAvailable)
        {
            _item.Borrow();
        }
        else
        {
            Console.WriteLine("The item is not available.");
        }
    }
}
public class BorrowableDVD : IBorrowable
{
    public string Title { get; set; }
    public bool IsAvailable { get; private set; } = true;

    public BorrowableDVD(string title)
    {
        Title = title;
    }

    public void Borrow()
    {
        if (IsAvailable)
        {
            IsAvailable = false;
            Console.WriteLine($"You have borrowed the DVD \"{Title}\".");
        }
        else
        {
            Console.WriteLine($"The DVD \"{Title}\" is already borrowed.");
        }
    }
}

class Program
{
    static void Main()
    {
        // Create borrowable items
        IBorrowable book = new BorrowableBook("Adventure Works Cycles");
        IBorrowable dvd = new BorrowableDVD("Graphic Design Institute");

        // Create libraries
        Library bookLibrary = new Library(book);
        Library dvdLibrary = new Library(dvd);

        // Borrow items
        bookLibrary.BorrowItem();
        bookLibrary.BorrowItem(); // Try borrowing again

        Console.WriteLine();

        dvdLibrary.BorrowItem();
        dvdLibrary.BorrowItem(); // Try borrowing again
    }
}
