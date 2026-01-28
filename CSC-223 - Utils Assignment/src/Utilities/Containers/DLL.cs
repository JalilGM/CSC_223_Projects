
using System.Security.Cryptography.X509Certificates;

namespace Containers;

public class Dnode<T>
{
    public T Data { get; set; }
    public Dnode<T>? Next { get; set; }
    public Dnode<T>? Prev { get; set; }

    public Dnode(T data)
    {
        Data = data;
        Next = null;
        Prev = null;
    }
}

public class DLL<T>: IEnumerable<T>, IList<T>
{
    private readonly Dnode<T> head = null!;
    private readonly Dnode<T> tail = null!;

    public DLL()
    {
        
        head = new Dnode<T>(default(T)!);
        tail = new Dnode<T>(default(T)!);
        head.Next = tail;
        tail.Prev = head;
        public int size = 0;
    }


    private void _Insert(Dnode<T> node, T item)
    {
        Dnode<T> newNode = new Dnode<T>(item);
        newNode.Next = node.Next;
        newNode.Prev = node;
        node.Next.Prev = newNode;
        node.Next = newNode;
        size++;
    }

    private void _Remove(Dnode<T> node)
    {
        node.Prev.Next = node.Next;
        node.Next.Prev = node.Prev;
        size--;
    }

    private Dnode<T> _GetNode(int index)
    {
        if (index < 0 || index >= size) throw new ArgumentOutOfRangeException("Index out of range.");
        
        Dnode<T> current = head.Next;
        for (int i = 0; i < index; i++)
        {
            current = current.Next;
        }
        return current;
    }

    public bool Contains(T item)
    {
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            if (EqualityComparer<T>.Default.Equals(current.Data, item)) return true;
            
            current = current.Next;
        }
        return false;
    }

    public int Size()
    {
        return size;
    }

    public string ToString()
    {
        List<string> elements = new List<string>();
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            elements.Add(current.Data?.ToString() ?? "null"); // ?? handles null values
            current = current.Next;
        }
        return "[" + string.Join(", ", elements) + "]";
    }

    public bool Remove(T item)
    {
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            if (EqualityComparer<T>.Default.Equals(current.Data, item))
            {
                _Remove(current);
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    public T Front()
    {
        if (size == 0) throw new InvalidOperationException("List is empty.");
        return head.Next.Data;
    }

    
}

