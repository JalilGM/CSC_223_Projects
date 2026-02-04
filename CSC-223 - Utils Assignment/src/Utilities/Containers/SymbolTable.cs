
using System.Collections;


namespace Containers;

public class SymbolTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> table;

    private readonly SymbolTable<TKey, TValue>? parent;

    public SymbolTable(): this(null)
    {}

    public SymbolTable(SymbolTable<TKey, TValue>? parent)
    {
        this.parent = parent;
        table = new Dictionary<TKey, TValue>();
    
    }

    public SymbolTable<TKey, TValue>? Parent => parent;

    public bool ContainsKeyLocal(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(key.ToString());
        return table.ContainsKey(key);
    }

    public bool TryGetValueLocal(TKey key, out TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(key.ToString());
        return table.TryGetValue(key, out value);
    }

    public TValue this[TKey key]
    {
        get => table[key];
        set => table[key] = value;
    }

    public ICollection<TKey> Keys => table.Keys;

    public ICollection<TValue> Values => table.Values;

    public int Count => table.Count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        table.Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return table.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return table.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return table.TryGetValue(key, out value);
    }

    public void Clear()
    {
        table.Clear();
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        table.Add(item.Key, item.Value);
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return table.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)table).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return ((IDictionary<TKey, TValue>)table).Remove(item);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return table.GetEnumerator();
    }

    IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return table.GetEnumerator();
    }

    
}