static int CountChar(string text, char c)
{
    int counter=0;
    
    foreach (char character in text)
    {
        if (character == c)
        {
            counter++;
        }
    }
    return counter;
}