/**
 * SymbolTable Class with IDictionary Implementation in C#
 * 
 * Bugs: Realized in ContainsKeyLocal and TryGetValueLocal methods, I wrote key.ToString()
 * which caused the corresponding ArgumentNullException tests to fail. Fixed by removing .ToString().
 * And used "nameof", which fixed the issue.
 * 
 * @author Jalil Garvin-Mingo
 * @date   February 4, 2026
 */

using System.Collections;

namespace Containers;

/// <summary>
/// A generic symbol table implementation that supports nested scopes through
/// parent-child relationships. Implements the IDictionary interface to provide
/// key-value storage with local and inherited lookup capabilities.
/// </summary>
/// <typeparam name="TKey">The type of keys stored in the symbol table</typeparam>
/// <typeparam name="TValue">The type of values associated with keys</typeparam>
public class SymbolTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> table;

    private readonly SymbolTable<TKey, TValue>? parent;

    public SymbolTable(): this(null)
    {}

    /// <summary>
    /// Initializes a new instance of the SymbolTable with an optional parent
    /// symbol table for scope inheritance.
    /// </summary>
    /// <param name="parent">The parent symbol table for scope inheritance, or
    /// null if this is a root-level symbol table</param>
    public SymbolTable(SymbolTable<TKey, TValue>? parent)
    {
        this.parent = parent;
        table = new Dictionary<TKey, TValue>();
    
    }

    /// <summary>
    /// Gets the parent symbol table, if one exists.
    /// </summary>
    public SymbolTable<TKey, TValue>? Parent => parent;

    /// <summary>
    /// Determines if a key exists in this symbol table's local scope only,
    /// without checking parent scopes.
    /// </summary>
    /// <param name="key">The key to check for existence</param>
    /// <returns>true if the key exists in the local scope; otherwise,
    /// false</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is
    /// null</exception>
    public bool ContainsKeyLocal(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        return table.ContainsKey(key);
    }

    /// <summary>
    /// Attempts to retrieve a value from this symbol table's local scope
    /// only, without checking parent scopes.
    /// </summary>
    /// <param name="key">The key to look up</param>
    /// <param name="value">The value associated with the key if found;
    /// otherwise, the default value for the type</param>
    /// <returns>true if the key exists in the local scope; otherwise,
    /// false</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is
    /// null</exception>
    public bool TryGetValueLocal(TKey key, out TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        return table.TryGetValue(key, out value);
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key to access</param>
    /// <returns>The value associated with the key</returns>
    /// <exception cref="KeyNotFoundException">Thrown when retrieving a value
    /// for a key that does not exist</exception>
    public TValue this[TKey key]
    {
        get => table[key];
        set => table[key] = value;
    }

    /// <summary>
    /// Gets a collection of all keys in this symbol table.
    /// </summary>
    public ICollection<TKey> Keys => table.Keys;

    /// <summary>
    /// Gets a collection of all values in this symbol table.
    /// </summary>
    public ICollection<TValue> Values => table.Values;

    /// <summary>
    /// Gets the number of key-value pairs stored in this symbol table.
    /// </summary>
    public int Count => table.Count;

    /// <summary>
    /// Gets a value indicating whether this symbol table is read-only.
    /// Always returns false.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Adds a key-value pair to the symbol table.
    /// </summary>
    /// <param name="key">The key to add</param>
    /// <param name="value">The value to associate with the key</param>
    /// <exception cref="ArgumentException">Thrown when the key already
    /// exists in the table</exception>
    public void Add(TKey key, TValue value)
    {
        table.Add(key, value);
    }

    /// <summary>
    /// Determines if a key exists in this symbol table.
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>true if the key exists; otherwise, false</returns>
    public bool ContainsKey(TKey key)
    {
        return table.ContainsKey(key);
    }

    /// <summary>
    /// Removes the key-value pair associated with the specified key.
    /// </summary>
    /// <param name="key">The key to remove</param>
    /// <returns>true if the key was found and removed; otherwise,
    /// false</returns>
    public bool Remove(TKey key)
    {
        return table.Remove(key);
    }

    /// <summary>
    /// Attempts to retrieve the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key to look up</param>
    /// <param name="value">The value if found; otherwise, the default value
    /// for the type</param>
    /// <returns>true if the key exists; otherwise, false</returns>
    public bool TryGetValue(TKey key, out TValue value)
    {
        return table.TryGetValue(key, out value);
    }

    /// <summary>
    /// Removes all key-value pairs from the symbol table.
    /// </summary>
    public void Clear()
    {
        table.Clear();
    }

    /// <summary>
    /// Adds a key-value pair to the symbol table.
    /// </summary>
    /// <param name="item">The KeyValuePair to add</param>
    /// <exception cref="ArgumentException">Thrown when the key already
    /// exists in the table</exception>
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        table.Add(item.Key, item.Value);
    }

    /// <summary>
    /// Determines if a key-value pair exists in the symbol table.
    /// </summary>
    /// <param name="item">The KeyValuePair to check</param>
    /// <returns>true if the exact key-value pair exists; otherwise,
    /// false</returns>
    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return table.Contains(item);
    }

    /// <summary>
    /// Copies all key-value pairs to an array starting at the specified
    /// index.
    /// </summary>
    /// <param name="array">The destination array</param>
    /// <param name="arrayIndex">The zero-based index where copying
    /// begins</param>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)table).CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Removes a key-value pair from the symbol table.
    /// </summary>
    /// <param name="item">The KeyValuePair to remove</param>
    /// <returns>true if the pair was found and removed; otherwise,
    /// false</returns>
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return ((IDictionary<TKey, TValue>)table).Remove(item);
    }

    /// <summary>
    /// Returns an enumerator that iterates through all key-value pairs in
    /// the symbol table.
    /// </summary>
    /// <returns>An enumerator for the dictionary</returns>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return table.GetEnumerator();
    }

    /// <summary>
    /// Returns a non-generic enumerator that iterates through all key-value
    /// pairs in the symbol table.
    /// </summary>
    /// <returns>An enumerator for the dictionary</returns>
    IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return table.GetEnumerator();
    }
}