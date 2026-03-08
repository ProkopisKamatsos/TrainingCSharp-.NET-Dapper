void Traverse(Node node)
{
    if (node == null)
        return;

    Console.WriteLine(node.Value);

    Traverse(node.Left);
    Traverse(node.Right);
}
class Node
{
    public int Value;
    public Node Left;
    public Node Right;
}

