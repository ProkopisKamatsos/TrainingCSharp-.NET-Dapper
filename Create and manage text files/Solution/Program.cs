using Files_M1;

using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        /*
        
        string directoryPath = @"C:\TempDir\SampleDirectory";

        string subDirectoryPath1 = Path.Combine(directoryPath, "SubDirectory1");
        string subDirectoryPath2 = Path.Combine(directoryPath, "SubDirectory2");

        string filePath = Path.Combine(directoryPath, "sample.txt");
        string appendFilePath = Path.Combine(directoryPath, "append.txt");
        string copyFilePath = Path.Combine(directoryPath, "copy.txt");
        string moveFilePath = Path.Combine(directoryPath, "moved.txt");

        Console.WriteLine($"Directory path: {directoryPath}");
        Console.WriteLine($"Text file paths ... Sample file path: {filePath}, Append file path: {appendFilePath}, Copy file path: {copyFilePath}, Move file path: {moveFilePath}");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Console.WriteLine($"Created directory: {directoryPath}");
        }

        if (!Directory.Exists(subDirectoryPath1))
        {
            Directory.CreateDirectory(subDirectoryPath1);
            Console.WriteLine($"Created subdirectory: {subDirectoryPath1}");
        }

        if (!Directory.Exists(subDirectoryPath2))
        {
            Directory.CreateDirectory(subDirectoryPath2);
            Console.WriteLine($"Created subdirectory: {subDirectoryPath2}");
        }

        // Use the File class to create a sample file in the root directory
        File.WriteAllText(filePath, "This is a sample file.");

        // Use the File class to create sample files in the subdirectories
        File.WriteAllText(Path.Combine(subDirectoryPath1, "file1.txt"), "Content of file1 in SubDirectory1");
        File.WriteAllText(Path.Combine(subDirectoryPath2, "file2.txt"), "Content of file2 in SubDirectory2");

        Console.WriteLine("\nEnumerating directories and files ...\n");

        // Enumerate the files within a specified root directory
        foreach (var file in Directory.GetFiles(directoryPath))
        {
            Console.WriteLine($"File: {file}");
        }

        // Enumerate the directories within a specified root directory
        foreach (var dir in Directory.GetDirectories(directoryPath))
        {
            Console.WriteLine($"Directory: {dir}");
        }

        // Enumerate the files within each subdirectory of the specified root directory
        foreach (var subDir in Directory.GetDirectories(directoryPath))
        {
            foreach (var file in Directory.GetFiles(subDir))
            {
                Console.WriteLine($"File: {file}");
            }
        }

        // foreach (var entry in Directory.EnumerateFileSystemEntries(directoryPath, "*", SearchOption.AllDirectories))
        // {
        //     Console.WriteLine($"Entry: {entry}");
        // }


        Console.WriteLine("\nUse the File class to write and read CSV-formatted text files.");

        string label = "deposits";
        double[,] depositValues =
        {
            { 100.50, 200.75, 300.25 },
            { 150.00, 250.50, 350.75 },
            { 175.25, 275.00, 375.50 }
        };

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < depositValues.GetLength(0); i++)
        {
            sb.AppendLine($"{label}: {depositValues[i, 0]}, {depositValues[i, 1]}, {depositValues[i, 2]}");
        }

        // Split the string representation of the StringBuilder object into an array of strings 
        //  based on the environment's newline character, removing any empty entries.
        string csvString = sb.ToString();
        string[] csvLines = csvString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine("\nCSV formatted string array:");
        foreach (var line in csvLines)
        {
            Console.WriteLine(line);
        }

        // Write the CSV formatted string array to a text file. The file is created if it doesn't exist, or overwritten if it does. 
        File.WriteAllText(filePath, csvString);

        // Read the contents of the text file into a string array and display the file contents
        string[] readLines = File.ReadAllLines(filePath);
        Console.WriteLine($"\nContent read from the {filePath} file:");
        foreach (var line in readLines)
        {
            Console.WriteLine(line);
        }

        // Append a new line to the text file
        File.AppendAllText(filePath, "deposits: 215.25, 417, 111.5\r\n");

        // Read and display the updated file contents
        string[] readUpdatedLines = File.ReadAllLines(filePath);
        Console.WriteLine($"\nContent read from updated the {filePath} file:");
        foreach (var line in readUpdatedLines)
        {
            Console.WriteLine(line);
        }

        // Extract the label and value components from the CSV formatted string
        string readLabel = readUpdatedLines[0].Split(':')[0];
        double[,] readDepositValues = new double[readUpdatedLines.Length, 3];
        for (int i = 0; i < readUpdatedLines.Length; i++)
        {
            string[] parts = readUpdatedLines[i].Split(':');
            string[] values = parts[1].Split(',');
            for (int j = 0; j < values.Length; j++)
            {
                readDepositValues[i, j] = double.Parse(values[j]);
            }
        }

        Console.WriteLine($"\nLabel: {readLabel}");
        Console.WriteLine("Deposit values:");
        for (int i = 0; i < readDepositValues.GetLength(0); i++)
        {
            Console.WriteLine($"{readDepositValues[i, 0]:C}, {readDepositValues[i, 1]:C}, {readDepositValues[i, 2]:C}");
        }

        Console.WriteLine("\nUse the File class to perform file management operations.\n");

        // Check whether the append.txt file exists
        if (File.Exists(appendFilePath))
        {
            Console.WriteLine($"The {appendFilePath} file exists.");
        }
        else
        {
            Console.WriteLine($"The {appendFilePath} file does not exist.");
        }

        // Copy the sample.txt file to the file location defined by the copyFilePath variable
        File.Copy(filePath, copyFilePath, true);
        Console.WriteLine($"Copied {filePath} to {copyFilePath}.");

        // Move the copy.txt file to the file location defined by the moveFilePath variable
        File.Move(copyFilePath, moveFilePath, true);
        Console.WriteLine($"Moved {copyFilePath} to {moveFilePath}");

        // Delete the move.txt file
        if (File.Exists(moveFilePath))
        {
            File.Delete(moveFilePath);
            Console.WriteLine($"Deleted file: {moveFilePath}");
        }
        
        */

        Console.WriteLine("\nUse the StreamWriter and StreamReader classes.\n");

        // Get the current directory of the executable program
        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine($"Current directory: {currentDirectory}");

        // Use currentDirectory to create a directory path named TransactionLogs
        string transactionsDirectoryPath = Path.Combine(currentDirectory, "TransactionLogs");
        if (!Directory.Exists(transactionsDirectoryPath))
        {
            Directory.CreateDirectory(transactionsDirectoryPath);
            Console.WriteLine($"Created directory: {transactionsDirectoryPath}");
        }

        // Create a filepath in the TransactionLogs directory for a file named transactions.csv
        string csvFilePath = Path.Combine(transactionsDirectoryPath, "transactions.csv");

        // Simulate one month of transactions for customer Niki Demetriou
        string firstName = "Niki";
        string lastName = "Demetriou";
        BankCustomer customer = new BankCustomer(firstName, lastName);

        // Add CheckingAccount, SavingsAccount, and MoneyMarketAccount to the customer object using the customer.CustomerId
        customer.AddAccount(new CheckingAccount(customer, customer.CustomerId, 5000));
        customer.AddAccount(new SavingsAccount(customer, customer.CustomerId, 15000));
        customer.AddAccount(new MoneyMarketAccount(customer, customer.CustomerId, 90000));

        DateOnly startDate = new DateOnly(2025, 3, 1);
        DateOnly endDate = new DateOnly(2025, 3, 31);
        customer = SimulateDepositsWithdrawalsTransfers.SimulateActivityDateRange(startDate, endDate, customer);

        using (StreamWriter sw = new StreamWriter(csvFilePath))
        {
            sw.WriteLine("TransactionId,TransactionType,TransactionDate,TransactionTime,PriorBalance,TransactionAmount,SourceAccountNumber,TargetAccountNumber,Description");

            Console.WriteLine("\nSimulated transactions:\n");
            foreach (var account in customer.Accounts)
            {
                foreach (var transaction in account.Transactions)
                {
                    Console.WriteLine($"{transaction.TransactionId},{transaction.TransactionType},{transaction.TransactionDate},{transaction.TransactionTime},{transaction.PriorBalance:F2},{transaction.TransactionAmount:F2},{transaction.SourceAccountNumber},{transaction.TargetAccountNumber},{transaction.Description}");
                    sw.WriteLine($"{transaction.TransactionId},{transaction.TransactionType},{transaction.TransactionDate},{transaction.TransactionTime},{transaction.PriorBalance:F2},{transaction.TransactionAmount:F2},{transaction.SourceAccountNumber},{transaction.TargetAccountNumber},{transaction.Description}");
                }
            }
        }

        // Read the transaction data from the transactions.csv file
        using (StreamReader sr = new StreamReader(csvFilePath))
        {
            string headerLine = sr.ReadLine(); // Read the header line
            Console.WriteLine("\nTransaction data read from the CSV file:\n");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        // Use the FileStream class to perform low-level file I/O operations

        // Create a filepath for the filestream.txt file
        string fileStreamPath = Path.Combine(currentDirectory, "filestream.txt");

        // Prepare transaction data from customer accounts
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("TransactionId,TransactionType,TransactionDate,TransactionTime,PriorBalance,TransactionAmount,SourceAccountNumber,TargetAccountNumber,Description");

        foreach (var account in customer.Accounts)
        {
            foreach (var transaction in account.Transactions)
            {
                // Append transaction data to the StringBuilder object
                sb.AppendLine($"{transaction.TransactionId},{transaction.TransactionType},{transaction.TransactionDate},{transaction.TransactionTime},{transaction.PriorBalance:F2},{transaction.TransactionAmount:F2},{transaction.SourceAccountNumber},{transaction.TargetAccountNumber},{transaction.Description}");
            }
        }

        Console.WriteLine($"\n\nUse the FileStream class to perform file I/O operations.");

        // Write transaction data to file using FileStream
        using (FileStream fileStream = new FileStream(fileStreamPath, FileMode.Create, FileAccess.Write))
        {
            // Convert the StringBuilder object to a byte array and write the byte array to the file
            byte[] transactionDataBytes = new UTF8Encoding(true).GetBytes(sb.ToString());

            // Use the Write method to write the byte array to the file
            fileStream.Write(transactionDataBytes, 0, transactionDataBytes.Length);
            Console.WriteLine($"\nFile length after write: {fileStream.Length} bytes");

            // Use the Flush method to ensure all data is written to the file
            fileStream.Flush();
        }

        Console.WriteLine($"\nTransaction data written using FileStream. File: {fileStreamPath}");

        // Read transaction data from file using FileStream
        using (FileStream fileStream = new FileStream(fileStreamPath, FileMode.Open, FileAccess.Read))
        {
            byte[] readBuffer = new byte[1024];
            UTF8Encoding utf8Decoder = new UTF8Encoding(true);
            int bytesRead;

            Console.WriteLine("\nUsing FileStream to read/display transaction data.\n");

            while ((bytesRead = fileStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                Console.WriteLine($"bytes read: {utf8Decoder.GetString(readBuffer, 0, bytesRead)}\n");
            }

            Console.WriteLine($"File length: {fileStream.Length} bytes");
            Console.WriteLine($"Current position: {fileStream.Position} bytes");

            // Use the Seek method to move to the beginning of the file
            fileStream.Seek(0, SeekOrigin.Begin);
            Console.WriteLine($"Position after seek: {fileStream.Position} bytes");

            // Use the FileStream.Read method to read the first 100 bytes
            bytesRead = fileStream.Read(readBuffer, 0, 100);
            Console.WriteLine($"Read first 100 bytes: {utf8Decoder.GetString(readBuffer, 0, bytesRead)}");

        }

        // Create a filepath for a binary file named binary.dat
        string binaryFilePath = Path.Combine(currentDirectory, "binary.dat");

        // Create a BinaryWriter object and write binary data to the binary.dat file
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(binaryFilePath, FileMode.Create)))
        {
            binaryWriter.Write(1.25);
            binaryWriter.Write("Hello");
            binaryWriter.Write(true);
        }

        Console.WriteLine($"\n\nBinary data written to: {binaryFilePath}");

        // Create a BinaryReader object and read binary data from the binary.dat file
        using (BinaryReader binaryReader = new BinaryReader(File.Open(binaryFilePath, FileMode.Open)))
        {
            double a = binaryReader.ReadDouble();
            string b = binaryReader.ReadString();
            bool c = binaryReader.ReadBoolean();
            Console.WriteLine($"Binary data read from {binaryFilePath}: {a}, {b}, {c}");
        }

    }
}