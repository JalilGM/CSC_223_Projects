/**
 * Doubly Linked List (DLL) Implementation in C#
 * 
 * Bugs: Had some issues with IEnumerable.GetEnumerator() implementation. Asked Ai to see the issue and got it fixed
 * 
 * @author Jalil Garvin-Mingo
 * @date   January 27, 2026
 */



namespace Containers;

/// <summary>
/// Represents a node in a doubly linked list.
/// Each node contains data and references to both the next and previous nodes.
/// </summary>
/// <typeparam name="T">The type of data stored in the node</typeparam>
public class Dnode<T>
{
    /// <summary>Gets or sets the data stored in this node</summary>
    public T Data { get; set; }
    
    /// <summary>Gets or sets the reference to the next node in the list</summary>
    public Dnode<T>? Next { get; set; }
    
    /// <summary>Gets or sets the reference to the previous node in the list</summary>
    public Dnode<T>? Prev { get; set; }

    /// <summary>
    /// Initializes a new node with the specified data.
    /// Both Next and Prev references are set to null.
    /// </summary>
    /// <param name="data">The data to store in this node</param>
    public Dnode(T data)
    {
        Data = data;
        Next = null;
        Prev = null;
    }
}

/// <summary>
/// A generic doubly linked list implementation that provides efficient insertion and deletion
/// at both ends of the list. Uses sentinel nodes (head and tail) to simplify boundary conditions.
/// </summary>
/// <typeparam name="T">The type of elements in the list</typeparam>
public class DLL<T>: IEnumerable<T>, IList<T>
{
    /// <summary>Sentinel node at the beginning of the list (contains no data)</summary>
    private readonly Dnode<T> head = null!;
    
    /// <summary>Sentinel node at the end of the list (contains no data)</summary>
    private readonly Dnode<T> tail = null!;

    /// <summary>Tracks the number of elements currently in the list</summary>
    public int size = 0;
    
    /// <summary>
    /// Initializes a new empty doubly linked list with sentinel head and tail nodes.
    /// The head and tail are connected to each other, forming an empty circular structure.
    /// </summary>
    public DLL()
    {
        // Create sentinel nodes that will never contain actual data
        head = new Dnode<T>(default(T)!);
        tail = new Dnode<T>(default(T)!);
        
        // Connect sentinels: head <-> tail
        head.Next = tail;
        tail.Prev = head;
        size = 0;
    }

    /// <summary>
    /// Inserts an item after the specified node and increments the size counter.
    /// This is the core insertion operation used by all other insertion methods.
    /// </summary>
    /// <param name="node">The node after which to insert the new item</param>
    /// <param name="item">The item to insert</param>
    private void _Insert(Dnode<T> node, T item)
    {
        // Create the new node with the given data
        Dnode<T> newNode = new Dnode<T>(item);
        
        // Link the new node to its neighbors
        newNode.Next = node.Next;      // New node points to what node was pointing to
        newNode.Prev = node;            // New node's previous is the insertion point
        node.Next.Prev = newNode;       // Update the old next's previous reference
        node.Next = newNode;            // Update the insertion point's next reference
        size++;
    }

    /// <summary>
    /// Removes the specified node from the list and decrements the size counter.
    /// This is the core removal operation used by all other deletion methods.
    /// </summary>
    /// <param name="node">The node to remove from the list</param>
    private void _Remove(Dnode<T> node)
    {
        // Remove the node by updating its neighbors' references to bypass it
        node.Prev.Next = node.Next;    // Predecessor's next points to successor
        node.Next.Prev = node.Prev;    // Successor's previous points to predecessor
        size--;
    }

    /// <summary>
    /// Retrieves the node at the specified index.
    /// Throws an exception if the index is out of bounds.
    /// </summary>
    /// <param name="index">The zero-based index of the node to retrieve</param>
    /// <returns>The node at the specified index</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if index is negative or >= size</exception>
    private Dnode<T> _GetNode(int index)
    {
        if (index < 0 || index >= size) throw new ArgumentOutOfRangeException("Index out of range.");
        
        // Traverse the list from the head to reach the desired index
        Dnode<T> current = head.Next;
        for (int i = 0; i < index; i++)
        {
            current = current.Next;
        }
        return current;
    }

    /// <summary>
    /// Determines whether the list contains a specific item.
    /// Uses value equality comparison via EqualityComparer.
    /// </summary>
    /// <param name="item">The item to search for</param>
    /// <returns>True if the item is found; otherwise, false</returns>
    public bool Contains(T item)
    {
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            // Compare using the default equality comparer for type T
            if (EqualityComparer<T>.Default.Equals(current.Data, item)) return true;
            current = current.Next;
        }
        return false;
    }

    /// <summary>
    /// Gets the number of elements in the list.
    /// </summary>
    /// <returns>The count of elements in the list</returns>
    public int Size()
    {
        return size;
    }

    /// <summary>
    /// Returns a string representation of the list in the format "[element1, element2, ...]"
    /// </summary>
    /// <returns>A formatted string of all elements in the list</returns>
    public string ToString()
    {
        List<string> elements = new List<string>();
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            elements.Add(current.Data?.ToString() ?? "null"); // "??" handles null values
            current = current.Next;
        }
        return "[" + string.Join(", ", elements) + "]";
    }

    /// <summary>
    /// Removes the first occurrence of a specific item from the list.
    /// </summary>
    /// <param name="item">The item to remove</param>
    /// <returns>True if the item was found and removed; otherwise, false</returns>
    public bool Remove(T item)
    {
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            // Check if this node contains the item we're looking for
            if (EqualityComparer<T>.Default.Equals(current.Data, item))
            {
                _Remove(current);
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    /// <summary>
    /// Returns the first element in the list without removing it.
    /// </summary>
    /// <returns>The element at the front of the list</returns>
    /// <exception cref="InvalidOperationException">Thrown if the list is empty</exception>
    public T Front()
    {
        if (size == 0) throw new InvalidOperationException("List is empty.");
        return head.Next.Data;
    }

    /// <summary>
    /// Returns the last element in the list without removing it.
    /// </summary>
    /// <returns>The element at the back of the list</returns>
    /// <exception cref="InvalidOperationException">Thrown if the list is empty</exception>
    public T Back()
    {
        if (size == 0) throw new InvalidOperationException("List is empty.");
        return tail.Prev.Data;
    }

    /// <summary>
    /// Adds an element to the front of the list.
    /// </summary>
    /// <param name="item">The item to add</param>
    public void PushFront(T item)
    {
        _Insert(head, item);
    }

    /// <summary>
    /// Adds an element to the back of the list.
    /// </summary>
    /// <param name="item">The item to add</param>
    public void PushBack(T item)
    {
        _Insert(tail.Prev, item);
    }

    /// <summary>
    /// Removes and discards the first element in the list.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the list is empty</exception>
    public void PopFront()
    {
        if (size == 0) throw new InvalidOperationException("List is empty.");
        _Remove(head.Next);
    }

    /// <summary>
    /// Removes and discards the last element in the list.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the list is empty</exception>
    public void PopBack()
    {
        if (size == 0) throw new InvalidOperationException("List is empty.");
        _Remove(tail.Prev);
    }

    /// <summary>
    /// Removes all elements from the list, making it empty.
    /// Resets the list to its initial state with only sentinel nodes.
    /// </summary>
    public void Clear()
    {
        // Disconnect all data nodes by reconnecting sentinels directly
        head.Next = tail;
        tail.Prev = head;
        size = 0;
    }

    /// <summary>
    /// Determines whether the list is empty.
    /// </summary>
    /// <returns>True if the list contains no elements; otherwise, false</returns>
    public bool IsEmpty()
    {
        return size == 0;
    }

    /// <summary>Gets the number of elements in the list (IList implementation)</summary>
    public int Count => size;
    
    /// <summary>Gets a value indicating whether the list is read-only (always false for DLL)</summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Adds an element to the end of the list (ICollection implementation).
    /// Equivalent to PushBack.
    /// </summary>
    /// <param name="item">The item to add</param>
    public void Add(T item)
    {
        PushBack(item);
    }

    /// <summary>
    /// Inserts an element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index where the element should be inserted</param>
    /// <param name="item">The element to insert</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if index is negative or > size</exception>
    public void Insert(int index, T item)
    {
        if (index < 0 || index > size) throw new ArgumentOutOfRangeException("Index out of range.");
        Dnode<T> node = _GetNode(index);
        _Insert(node.Prev, item); // Insert before the node at the index, so node is at index intended for
    }

    /// <summary>
    /// Finds the zero-based index of the first occurrence of an element.
    /// </summary>
    /// <param name="item">The element to search for</param>
    /// <returns>The index of the element if found; otherwise, -1</returns>
    public int IndexOf(T item)
    {
        Dnode<T> current = head.Next;
        int index = 0;
        while (current != tail)
        {
            // Return the index when the item is found
            if (EqualityComparer<T>.Default.Equals(current.Data, item)) return index;
            current = current.Next;
            index++;
        }
        return -1; // Not found
    }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element</param>
    /// <returns>The element at the specified index</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if index is out of bounds</exception>
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

    /// <summary>
    /// Removes the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if index is out of bounds</exception>
    public void RemoveAt(int index)
    {
        Dnode<T> node = _GetNode(index);
        _Remove(node);
    }

    /// <summary>
    /// Copies all elements of the list to an array, starting at the specified array index.
    /// </summary>
    /// <param name="array">The destination array</param>
    /// <param name="arrayIndex">The zero-based index in the array where copying begins</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            array[arrayIndex++] = current.Data;
            current = current.Next;
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the list from front to back.
    /// </summary>
    /// <returns>An enumerator for the list</returns>
    public IEnumerator<T> GetEnumerator()
    {
        Dnode<T> current = head.Next;
        while (current != tail)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the list (non-generic implementation).
    /// </summary>
    /// <returns>An enumerator for the list</returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}    