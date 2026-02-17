using System;
using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

// ----------------------------
// Config
// ----------------------------
const int examAssignments = 5;

// Default students (you can add/remove names here)
string[] studentNames = new[] { "Sophia", "Andrew", "Emma", "Logan" };

// Store scores per student (each array will contain: exams first, then extra credit)
int[][] allStudentScores = new int[studentNames.Length][];

// ----------------------------
// Input
// ----------------------------
Console.Clear();
Console.WriteLine("Student Grading (Interactive)");
Console.WriteLine($"- Exams per student: {examAssignments}");
Console.WriteLine("- Extra credit scores are entered after exams (0 or more).");
Console.WriteLine();

// Collect scores for each student
for (int i = 0; i < studentNames.Length; i++)
{
    string student = studentNames[i];
    Console.WriteLine($"=== {student} ===");

    int[] examScores = ReadIntList(
        prompt: $"Enter {examAssignments} exam scores (0-100), separated by space or comma:\n> ",
        requiredCount: examAssignments,
        min: 0,
        max: 100
    );

    int extraCount = ReadInt(
        prompt: "How many extra credit scores? (0+)\n> ",
        min: 0,
        max: 100
    );

    int[] extraScores = extraCount == 0
        ? Array.Empty<int>()
        : ReadIntList(
            prompt: $"Enter {extraCount} extra credit scores (0-100), separated by space or comma:\n> ",
            requiredCount: extraCount,
            min: 0,
            max: 100
        );

    // Combine: exams first, then extra credit
    int[] combined = new int[examScores.Length + extraScores.Length];
    Array.Copy(examScores, 0, combined, 0, examScores.Length);
    Array.Copy(extraScores, 0, combined, examScores.Length, extraScores.Length);

    allStudentScores[i] = combined;

    Console.WriteLine();
}

// ----------------------------
// Report
// ----------------------------
Console.Clear();
Console.WriteLine("Student\t\tExam Score\tOverall Grade\tExtra Credit\n");

for (int i = 0; i < studentNames.Length; i++)
{
    string student = studentNames[i];
    int[] studentScores = allStudentScores[i];

    // Split sums
    int sumExamScores = 0;
    int sumExtraCreditScores = 0;

    int gradedAssignments = 0;

    foreach (int score in studentScores)
    {
        gradedAssignments++;

        if (gradedAssignments <= examAssignments)
            sumExamScores += score;
        else
            sumExtraCreditScores += score;
    }

    // Calculations (decimal to keep fractions)
    decimal examScore = (decimal)sumExamScores / examAssignments;

    int extraCreditAssignments = studentScores.Length - examAssignments;

    // raw average of extra credit scores (as shown in the report)
    decimal extraCreditScore = extraCreditAssignments > 0
        ? (decimal)sumExtraCreditScores / extraCreditAssignments
        : 0;

    // points added to the overall grade by extra credit:
    // (10% of extra credit sum) / number of exams
    decimal extraCreditPoints = ((decimal)sumExtraCreditScores * 0.1m) / examAssignments;

    // overall grade:
    // (exam sum + 10% of extra credit sum) / number of exams
    decimal overallGrade = ((decimal)sumExamScores + ((decimal)sumExtraCreditScores * 0.1m)) / examAssignments;

    // letter grade based on overallGrade
    string letterGrade = GetLetterGrade(overallGrade);

    // Output row (format aligned with the challenge)
    Console.WriteLine(
        $"{student}\t\t{examScore:F1}\t\t{overallGrade:F2}\t{letterGrade}\t{extraCreditScore:F0} ({extraCreditPoints:F2} pts)"
    );
}

Console.WriteLine("\nPress Enter to exit...");
Console.ReadLine();

// ----------------------------
// Helpers
// ----------------------------
static string GetLetterGrade(decimal score)
{
    if (score >= 97) return "A+";
    else if (score >= 93) return "A";
    else if (score >= 90) return "A-";
    else if (score >= 87) return "B+";
    else if (score >= 83) return "B";
    else if (score >= 80) return "B-";
    else if (score >= 77) return "C+";
    else if (score >= 73) return "C";
    else if (score >= 70) return "C-";
    else if (score >= 67) return "D+";
    else if (score >= 63) return "D";
    else if (score >= 60) return "D-";
    else return "F";
}

static int ReadInt(string prompt, int min, int max)
{
    while (true)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int value) && value >= min && value <= max)
            return value;

        Console.WriteLine($"Please enter an integer between {min} and {max}.");
    }
}

static int[] ReadIntList(string prompt, int requiredCount, int min, int max)
{
    while (true)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Please enter values.");
            continue;
        }

        string[] parts = input.Split(new[] { ' ', ',', ';', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != requiredCount)
        {
            Console.WriteLine($"Please enter exactly {requiredCount} numbers.");
            continue;
        }

        int[] values = new int[requiredCount];
        bool ok = true;

        for (int i = 0; i < parts.Length; i++)
        {
            if (!int.TryParse(parts[i], out int v) || v < min || v > max)
            {
                ok = false;
                break;
            }
            values[i] = v;
        }

        if (!ok)
        {
            Console.WriteLine($"All values must be integers between {min} and {max}.");
            continue;
        }

        return values;
    }
}
