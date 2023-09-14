public class Node<T>
{
    public Node<T> Next { get; set; }
    public string Key { get; set; }
    public T Value { get; set; }

}

public class HashTable<T>
{
    private readonly Node<T>[] _buckets;
    public HashTable(int size)
    {
        _buckets = new Node<T>[size];
    }
    public void Add(string key, T value)
    {
        CheckKey(key);
        var valueNode = new Node<T> { Key = key, Value = value, Next = null };
        int position = GetBucketByKey(key);
        Node<T> listNode = _buckets[position];
        if (listNode == null)
        {
            _buckets[position] = valueNode;
        }
        else
        {
            while (listNode.Next != null)
            {
                listNode = listNode.Next;  
            }
            listNode.Next = valueNode;
        }
    }
    public T Get(string key)
    {
        CheckKey(key);
        var (_, node) = GetNodeByKey(key);
        if (node == null) throw new ArgumentOutOfRangeException(nameof(key), $"The key '{key}' is not found");
        return node.Value;
    }
    protected void CheckKey(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));
    }
    protected (Node<T> prev, Node<T> curr) GetNodeByKey(string key)
    {
        int position = GetBucketByKey(key);
        Node<T> listNode = _buckets[position];
        Node<T> prev = null;
        while (listNode != null)
        {
            if (listNode.Key == key)
            {
                return (prev, listNode);
            }
            prev = listNode;
            listNode = listNode.Next;
        }
        return (null, null);
    }
    public bool Remove(string key)
    {
        CheckKey(key);
        int position = GetBucketByKey(key);
        var (prev, curr) = GetNodeByKey(key);
        if (curr == null) return false;
        if (prev == null)
        {
            _buckets[position] = null;
            return true;
        }
        prev.Next = curr.Next;
        return true;
    } 
    public int GetBucketByKey(string key)
    {
        return Math.Abs(key.GetHashCode() % _buckets.Length);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var hashT = new HashTable<int>(4);
        Console.WriteLine(hashT.GetBucketByKey("One"));
        Console.WriteLine(hashT.GetBucketByKey("Two"));
        Console.WriteLine(hashT.GetBucketByKey("Three"));
        Console.WriteLine(hashT.GetBucketByKey("Four"));
        Console.WriteLine(hashT.GetBucketByKey("Five"));

        hashT.Add("One", 32);
        hashT.Add("Three", 333);

        Console.WriteLine(hashT.Get("One"));
        Console.WriteLine(hashT.Get("Three"));

        try
        {
            Console.WriteLine(hashT.Get("abarakadabra"));
        }
        catch (Exception exception)
        {

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine(hashT.Remove("One"));
        Console.WriteLine(hashT.Remove("One"));
    }
}
