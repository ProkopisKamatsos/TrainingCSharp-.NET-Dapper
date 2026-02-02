
using Files_M1;

using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
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

    }
}
