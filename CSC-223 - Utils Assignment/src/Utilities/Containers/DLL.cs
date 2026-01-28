
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

    public int size = 0;
    public DLL()
    {
        
        head = new Dnode<T>(default(T)!);
        tail = new Dnode<T>(default(T)!);
        head.Next = tail;
        tail.Prev = head;
        size = 0;
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

    public T Back()
    {
        if (size == 0) throw new InvalidOperationException("List is empty.");
        return tail.Prev.Data;
    }

    public void PushFront(T item)
    {
        _Insert(head, item);
    }

    public void PushBack(T item)
    {
        _Insert(tail.Prev, item);
    }

    public void PopFront()
    {
        if (size == 0) throw new InvalidOperationException("List is empty.");
        _Remove(head.Next);
    }

    public void PopBack()
    {
        if (size == 0) throw new InvalidOperationException("List is empty.");
        _Remove(tail.Prev);
    }

    public void Clear()
    {
        head.Next = tail;
        tail.Prev = head;
        size = 0;
    }

    public bool IsEmpty()
    {
        return size == 0;
    }

    public int Count { get; }
    public bool IsReadOnly { get; }

    public void Add(T item)
    {
        PushBack(item);
    }

    public void Insert(int index, T item)
    {
        if (index < 0 || index > size) throw new ArgumentOutOfRangeException("Index out of range.");
        Dnode<T> node = _GetNode(index);
        _Insert(node.Prev, item); // Insert before the node at the index, so node is at index intended for
    }

    public int IndexOf(T item)
    {
        Dnode<T> current = head.Next;
        int index = 0;
        while (current != tail)
        {
            if (EqualityComparer<T>.Default.Equals(current.Data, item)) return index;
            current = current.Next;
            index++;
        }
        return -1; // Not found
    }

    public T this[int index] 
    {
        get
        {
            Dnode<T> node = _GetNode(index);
            return node.Data;
        }
        set
        {
            Dnode<T> node = _GetNode(index);
            node.Data = value;
        }
    }

    public void RemoveAt(int index)
    {
        Dnode<T> node = _GetNode(index);
        _Remove(node);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            array[arrayIndex++] = current.Data;
            current = current.Next;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}    