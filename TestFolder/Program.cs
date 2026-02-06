// Specify the data source.
int[] scores = [97, 92, 81, 60];

// Define the query expression.
var scoreQuery =
    from score in scores
    orderby score descending
    select score;

// Execute the query.
foreach (var i in scoreQuery)
{
    Console.Write(i + " ");
}

// Output: 97 92 81