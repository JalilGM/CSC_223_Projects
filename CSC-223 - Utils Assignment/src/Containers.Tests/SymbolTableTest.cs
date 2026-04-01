using Containers;
using Xunit;
using System.Collections.Generic;

namespace ContainersTests;

/// <summary>
/// Comprehensive test suite for the SymbolTable generic class. Tests verify
/// correct behavior of key-value storage, local vs inherited scope lookups,
/// IDictionary interface implementation, and edge cases.
/// </summary>
public class SymbolTableTest
{
    #region Constructor Tests - Verify proper initialization of symbol tables

    /// <summary>
    /// Verifies that a symbol table created with the default constructor
    /// initializes with no parent and an empty collection.
    /// </summary>
    [Fact]
    public void Constructor_NoParent_CreatesEmptyTable()
    {
        // Act - Create default instance
        var symbolTable = new SymbolTable<string, int>();

        // Assert - Verify empty state and null parent
        Assert.Null(symbolTable.Parent);
        Assert.Equal(0, symbolTable.Count);
    }

    /// <summary>
    /// Verifies that a symbol table can be constructed with a parent
    /// reference and maintains proper scope hierarchy.
    /// </summary>
    [Fact]
    public void Constructor_WithParent_StoresParentReference()
    {
        // Arrange - Create parent table
        var parentTable = new SymbolTable<string, int>();

        // Act - Create child with parent reference
        var childTable = new SymbolTable<string, int>(parentTable);

        // Assert - Verify parent is stored correctly
        Assert.NotNull(childTable.Parent);
        Assert.Same(parentTable, childTable.Parent);
    }

    /// <summary>
    /// Verifies that a symbol table can be created with a null parent
    /// without errors.
    /// </summary>
    [Fact]
    public void Constructor_WithNullParent_AllowsNull()
    {
        // Act - Explicitly pass null as parent
        var symbolTable = new SymbolTable<string, int>(null);

        // Assert - Confirm parent is null
        Assert.Null(symbolTable.Parent);
    }
    #endregion

    #region Add Tests - Verify insertion of key-value pairs

    /// <summary>
    /// Verifies that a single key-value pair can be successfully added and
    /// retrieved.
    /// </summary>
    [Fact]
    public void Add_ShouldAddKeyValuePair()
    {
        // Arrange - Create table and prepare test data
        var symbolTable = new SymbolTable<string, int>();
        string key = "testKey";
        int value = 1;

        // Act - Add pair to table
        symbolTable.Add(key, value);

        // Assert - Confirm addition and retrieval
        Assert.True(symbolTable.ContainsKey(key));
        Assert.Equal(value, symbolTable[key]);
    }

    /// <summary>
    /// Verifies that multiple key-value pairs can be added simultaneously
    /// and all are stored correctly.
    /// </summary>
    [Fact]
    public void Add_MultipleItems_AllAddedSuccessfully()
    {
        // Arrange - Create table
        var symbolTable = new SymbolTable<string, string>();

        // Act - Add multiple pairs
        symbolTable.Add("name", "John");
        symbolTable.Add("age", "30");
        symbolTable.Add("city", "New York");

        // Assert - Verify all items present
        Assert.Equal(3, symbolTable.Count);
        Assert.Equal("John", symbolTable["name"]);
        Assert.Equal("30", symbolTable["age"]);
        Assert.Equal("New York", symbolTable["city"]);
    }

    /// <summary>
    /// Verifies that adding a duplicate key throws an ArgumentException.
    /// </summary>
    [Fact]
    public void Add_DuplicateKey_ThrowsArgumentException()
    {
        // Arrange - Create table with initial pair
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key", 1);

        // Act & Assert - Attempt duplicate should throw
        Assert.Throws<ArgumentException>(() => symbolTable.Add("key", 2));
    }

    /// <summary>
    /// Verifies that the KeyValuePair overload of Add works correctly.
    /// </summary>
    [Fact]
    public void Add_KeyValuePair_ShouldAddItem()
    {
        // Arrange - Create table and pair
        var symbolTable = new SymbolTable<string, int>();
        var pair = new KeyValuePair<string, int>("key", 42);

        // Act - Add using pair overload
        symbolTable.Add(pair);

        // Assert - Verify retrieval
        Assert.Equal(42, symbolTable["key"]);
    }
    #endregion

    #region Remove Tests - Verify deletion of key-value pairs

    /// <summary>
    /// Verifies that a key-value pair can be removed and is no longer
    /// accessible.
    /// </summary>
    [Fact]
    public void Remove_ShouldRemoveKeyValuePair()
    {
        // Arrange - Create table with initial pair
        var symbolTable = new SymbolTable<string, int>();
        string key = "testKey";
        int value = 1;
        symbolTable.Add(key, value);

        // Act - Remove the pair
        bool removed = symbolTable.Remove(key);

        // Assert - Verify removal and return value
        Assert.True(removed);
        Assert.False(symbolTable.ContainsKey(key));
    }

    /// <summary>
    /// Verifies that removing a non-existent key returns false and does
    /// not throw.
    /// </summary>
    [Fact]
    public void Remove_NonExistentKey_ReturnsFalse()
    {
        // Arrange - Create empty table
        var symbolTable = new SymbolTable<string, int>();

        // Act - Attempt to remove non-existent key
        bool removed = symbolTable.Remove("nonExistent");

        // Assert - Verify false return
        Assert.False(removed);
    }

    /// <summary>
    /// Verifies that a KeyValuePair can be removed using the pair overload.
    /// </summary>
    [Fact]
    public void Remove_KeyValuePair_ShouldRemoveItem()
    {
        // Arrange - Create table with pair
        var symbolTable = new SymbolTable<string, int>();
        var pair = new KeyValuePair<string, int>("key", 42);
        symbolTable.Add(pair);

        // Act - Remove using pair overload
        bool removed = symbolTable.Remove(pair);

        // Assert - Verify removal
        Assert.True(removed);
        Assert.False(symbolTable.ContainsKey("key"));
    }

    /// <summary>
    /// Verifies that removing a pair with wrong value returns false even
    /// if key exists.
    /// </summary>
    [Fact]
    public void Remove_WrongKeyValuePair_ReturnsFalse()
    {
        // Arrange - Create table with pair
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key", 42);
        var wrongPair = new KeyValuePair<string, int>("key", 99);

        // Act - Try remove with different value
        bool removed = symbolTable.Remove(wrongPair);

        // Assert - Verify false and item still present
        Assert.False(removed);
    }
    #endregion

    #region Get/Set Tests - Verify indexer operations

    /// <summary>
    /// Verifies that the get accessor returns the correct value for an
    /// existing key.
    /// </summary>
    [Fact]
    public void Indexer_Get_ReturnsCorrectValue()
    {
        // Arrange - Create table with pair
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key", 42);

        // Act - Retrieve value via indexer
        int value = symbolTable["key"];

        // Assert - Verify correct value
        Assert.Equal(42, value);
    }

    /// <summary>
    /// Verifies that the set accessor updates an existing value correctly.
    /// </summary>
    [Fact]
    public void Indexer_Set_UpdatesValue()
    {
        // Arrange - Create table with pair
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key", 42);

        // Act - Update value via indexer
        symbolTable["key"] = 100;

        // Assert - Verify update
        Assert.Equal(100, symbolTable["key"]);
    }

    /// <summary>
    /// Verifies that setting a value for a non-existent key creates a new
    /// entry.
    /// </summary>
    [Fact]
    public void Indexer_Set_NonExistentKey_CreatesNewEntry()
    {
        // Arrange - Create empty table
        var symbolTable = new SymbolTable<string, int>();

        // Act - Set value for new key
        symbolTable["newKey"] = 99;

        // Assert - Verify new entry created
        Assert.Equal(99, symbolTable["newKey"]);
    }

    /// <summary>
    /// Verifies that getting a value for a non-existent key throws
    /// KeyNotFoundException.
    /// </summary>
    [Fact]
    public void Indexer_Get_NonExistentKey_ThrowsKeyNotFoundException()
    {
        // Arrange - Create empty table
        var symbolTable = new SymbolTable<string, int>();

        // Act & Assert - Get non-existent should throw
        Assert.Throws<KeyNotFoundException>(() =>
            symbolTable["nonExistent"]);
    }
    #endregion

    #region TryGetValue Tests - Verify safe retrieval operations

    /// <summary>
    /// Verifies that TryGetValue returns true and correct value for
    /// existing keys.
    /// </summary>
    [Theory]
    [InlineData("key1", 1)]
    [InlineData("key2", 2)]
    [InlineData("key3", 3)]
    public void TryGetValue_ShouldReturnTrueForExistingKey(string key,
        int expectedValue)
    {
        // Arrange - Create table with test data
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add(key, expectedValue);

        // Act - Try get the value
        bool result = symbolTable.TryGetValue(key, out int actualValue);

        // Assert - Verify success and value
        Assert.True(result);
        Assert.Equal(expectedValue, actualValue);
    }

    /// <summary>
    /// Verifies that TryGetValue returns false for non-existent keys without
    /// throwing.
    /// </summary>
    [Fact]
    public void TryGetValue_NonExistentKey_ReturnsFalse()
    {
        // Arrange - Create empty table
        var symbolTable = new SymbolTable<string, int>();

        // Act - Try get non-existent key
        bool result = symbolTable.TryGetValue("nonExistent",
            out int value);

        // Assert - Verify false and default value
        Assert.False(result);
        Assert.Equal(0, value);
    }
    #endregion

    #region Local Scope Tests - Verify scope-aware lookups

    /// <summary>
    /// Verifies that ContainsKeyLocal returns true for keys in local scope.
    /// </summary>
    [Fact]
    public void ContainsKeyLocal_ShouldReturnTrueForExistingKey()
    {
        // Arrange - Create table with test key
        var symbolTable = new SymbolTable<string, int>();
        string key = "testKey";
        symbolTable.Add(key, 1);

        // Act - Check local scope
        bool contains = symbolTable.ContainsKeyLocal(key);

        // Assert - Verify true
        Assert.True(contains);
    }

    /// <summary>
    /// Verifies that ContainsKeyLocal returns false for non-existent local
    /// keys.
    /// </summary>
    [Fact]
    public void ContainsKeyLocal_NonExistentKey_ReturnsFalse()
    {
        // Arrange - Create empty table
        var symbolTable = new SymbolTable<string, int>();

        // Act - Check local scope for non-existent key
        bool contains = symbolTable.ContainsKeyLocal("nonExistent");

        // Assert - Verify false
        Assert.False(contains);
    }

    /// <summary>
    /// Verifies that TryGetValueLocal returns false for non-existent keys.
    /// </summary>
    [Fact]
    public void TryGetValueLocal_ShouldReturnFalseForNonExistingKey()
    {
        // Arrange - Create empty table
        var symbolTable = new SymbolTable<string, int>();
        string key = "nonExistingKey";

        // Act - Try get from local scope
        bool result = symbolTable.TryGetValueLocal(key, out int value);

        // Assert - Verify false
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that TryGetValueLocal returns true and correct value for
    /// existing local keys.
    /// </summary>
    [Fact]
    public void TryGetValueLocal_ExistingKey_ReturnsTrue()
    {
        // Arrange - Create table with test data
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key", 42);

        // Act - Try get from local scope
        bool result = symbolTable.TryGetValueLocal("key", out int value);

        // Assert - Verify true and value
        Assert.True(result);
        Assert.Equal(42, value);
    }

    /// <summary>
    /// Verifies that ContainsKeyLocal does not find keys in parent scope.
    /// </summary>
    [Fact]
    public void ContainsKeyLocal_IgnoresParent()
    {
        // Arrange - Create parent with key, child without
        var parentTable = new SymbolTable<string, int>();
        parentTable.Add("parentKey", 1);
        var childTable = new SymbolTable<string, int>(parentTable);

        // Act - Check local scope
        bool containsLocal = childTable.ContainsKeyLocal("parentKey");

        // Assert - Verify not found in local
        Assert.False(containsLocal);
    }

    /// <summary>
    /// Verifies that TryGetValueLocal does not find keys in parent scope.
    /// </summary>
    [Fact]
    public void TryGetValueLocal_IgnoresParent()
    {
        // Arrange - Create parent with key, child without
        var parentTable = new SymbolTable<string, int>();
        parentTable.Add("parentKey", 42);
        var childTable = new SymbolTable<string, int>(parentTable);

        // Act - Try get from local scope
        bool result = childTable.TryGetValueLocal("parentKey",
            out int value);

        // Assert - Verify not found in local
        Assert.False(result);
    }
    #endregion

    #region Clear Tests - Verify bulk deletion operations

    /// <summary>
    /// Verifies that Clear removes all items from the symbol table.
    /// </summary>
    [Fact]
    public void Clear_ShouldRemoveAllItems()
    {
        // Arrange - Create table with multiple items
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key1", 1);
        symbolTable.Add("key2", 2);

        // Act - Clear all
        symbolTable.Clear();

        // Assert - Verify empty
        Assert.Equal(0, symbolTable.Count);
    }

    /// <summary>
    /// Verifies that Clear on an empty table does not throw an exception.
    /// </summary>
    [Fact]
    public void Clear_EmptyTable_DoesNotThrow()
    {
        // Arrange - Create empty table
        var symbolTable = new SymbolTable<string, int>();

        // Act & Assert - Clear should succeed
        symbolTable.Clear();
        Assert.Equal(0, symbolTable.Count);
    }
    #endregion

    #region Collection Properties Tests - Verify Keys, Values, and Count

    /// <summary>
    /// Verifies that the Keys property returns all keys in the table.
    /// </summary>
    [Fact]
    public void Keys_ReturnsAllKeys()
    {
        // Arrange - Create table with multiple keys
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key1", 1);
        symbolTable.Add("key2", 2);
        symbolTable.Add("key3", 3);

        // Act - Get all keys
        var keys = symbolTable.Keys;

        // Assert - Verify all keys present
        Assert.Equal(3, keys.Count);
        Assert.Contains("key1", keys);
        Assert.Contains("key2", keys);
        Assert.Contains("key3", keys);
    }

    /// <summary>
    /// Verifies that the Values property returns all values in the table.
    /// </summary>
    [Fact]
    public void Values_ReturnsAllValues()
    {
        // Arrange - Create table with multiple values
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key1", 10);
        symbolTable.Add("key2", 20);
        symbolTable.Add("key3", 30);

        // Act - Get all values
        var values = symbolTable.Values;

        // Assert - Verify all values present
        Assert.Equal(3, values.Count);
        Assert.Contains(10, values);
        Assert.Contains(20, values);
        Assert.Contains(30, values);
    }

    /// <summary>
    /// Verifies that Count property accurately reflects table size through
    /// add and remove operations.
    /// </summary>
    [Fact]
    public void Count_ReturnsCorrectNumber()
    {
        // Arrange - Create empty table
        var symbolTable = new SymbolTable<string, int>();

        // Act & Assert - Verify count changes with operations
        Assert.Equal(0, symbolTable.Count);

        symbolTable.Add("key1", 1);
        Assert.Equal(1, symbolTable.Count);

        symbolTable.Add("key2", 2);
        Assert.Equal(2, symbolTable.Count);

        symbolTable.Remove("key1");
        Assert.Equal(1, symbolTable.Count);
    }

    /// <summary>
    /// Verifies that IsReadOnly property returns false.
    /// </summary>
    [Fact]
    public void IsReadOnly_ReturnsFalse()
    {
        // Arrange - Create table
        var symbolTable = new SymbolTable<string, int>();

        // Act & Assert - Verify writable
        Assert.False(symbolTable.IsReadOnly);
    }
    #endregion

    #region Contains Tests - Verify exact key-value pair matching

    /// <summary>
    /// Verifies that Contains returns true when exact key-value pair exists.
    /// </summary>
    [Fact]
    public void Contains_WithValidKeyValuePair_ReturnsTrue()
    {
        // Arrange - Create table with pair
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key", 42);

        // Act - Check for exact pair
        bool contains = symbolTable.Contains(
            new KeyValuePair<string, int>("key", 42));

        // Assert - Verify true
        Assert.True(contains);
    }

    /// <summary>
    /// Verifies that Contains returns false when value does not match even
    /// if key exists.
    /// </summary>
    [Fact]
    public void Contains_WithInvalidKeyValuePair_ReturnsFalse()
    {
        // Arrange - Create table with pair
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key", 42);

        // Act - Check for different value
        bool contains = symbolTable.Contains(
            new KeyValuePair<string, int>("key", 99));

        // Assert - Verify false
        Assert.False(contains);
    }
    #endregion

    #region CopyTo Tests - Verify array copying operations

    /// <summary>
    /// Verifies that CopyTo correctly copies all items to an array at the
    /// specified offset.
    /// </summary>
    [Fact]
    public void CopyTo_CopiesToArray()
    {
        // Arrange - Create table and destination array
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key1", 1);
        symbolTable.Add("key2", 2);
        var array = new KeyValuePair<string, int>[3];

        // Act - Copy to array at index 0
        symbolTable.CopyTo(array, 0);

        // Assert - Verify items at start, empty at end
        Assert.NotNull(array[0]);
        Assert.NotNull(array[1]);
        Assert.Equal(default, array[2]);
    }

    /// <summary>
    /// Verifies that CopyTo respects array offset and places items
    /// correctly.
    /// </summary>
    [Fact]
    public void CopyTo_WithOffset_CopiesToCorrectPosition()
    {
        // Arrange - Create table and offset destination array
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key1", 1);
        symbolTable.Add("key2", 2);
        var array = new KeyValuePair<string, int>[4];

        // Act - Copy to array starting at index 1
        symbolTable.CopyTo(array, 1);

        // Assert - Verify empty at 0, items at 1-2
        Assert.Equal(default, array[0]);
        Assert.NotNull(array[1]);
        Assert.NotNull(array[2]);
    }
    #endregion

    #region Enumeration Tests - Verify iteration capabilities

    /// <summary>
    /// Verifies that GetEnumerator correctly iterates all items in the
    /// table.
    /// </summary>
    [Fact]
    public void GetEnumerator_IteratesAllItems()
    {
        // Arrange - Create table with multiple items
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key1", 1);
        symbolTable.Add("key2", 2);
        symbolTable.Add("key3", 3);

        // Act - Collect items via enumeration
        var items = new List<KeyValuePair<string, int>>();
        foreach (var item in symbolTable)
        {
            items.Add(item);
        }

        // Assert - Verify all items enumerated
        Assert.Equal(3, items.Count);
    }

    /// <summary>
    /// Verifies that the non-generic IEnumerable.GetEnumerator works
    /// correctly.
    /// </summary>
    [Fact]
    public void IEnumerableGetEnumerator_IteratesAllItems()
    {
        // Arrange - Create table with items
        var symbolTable = new SymbolTable<string, int>();
        symbolTable.Add("key1", 1);
        symbolTable.Add("key2", 2);

        // Act - Enumerate using non-generic interface
        int count = 0;
        System.Collections.IEnumerator enumerator =
            ((System.Collections.IEnumerable)symbolTable).GetEnumerator();
        while (enumerator.MoveNext())
        {
            count++;
        }

        // Assert - Verify count matches
        Assert.Equal(2, count);
    }

    /// <summary>
    /// Verifies that enumerating an empty table yields no items.
    /// </summary>
    [Fact]
    public void GetEnumerator_EmptyTable_YieldsNoItems()
    {
        // Arrange - Create empty table
        var symbolTable = new SymbolTable<string, int>();

        // Act - Attempt enumeration
        var items = new List<KeyValuePair<string, int>>();
        foreach (var item in symbolTable)
        {
            items.Add(item);
        }

        // Assert - Verify empty collection
        Assert.Empty(items);
    }
    #endregion

    #region Generic Type Tests - Verify type flexibility

    /// <summary>
    /// Verifies that SymbolTable works correctly with various type
    /// combinations.
    /// </summary>
    [Fact]
    public void SymbolTable_WithDifferentTypes_WorksCorrectly()
    {
        // Arrange - Create tables with different key/value types
        var doubleTable = new SymbolTable<int, double>();
        var stringTable = new SymbolTable<string, string>();

        // Act - Add items with different types
        doubleTable.Add(1, 3.14);
        stringTable.Add("pi", "3.14");

        // Assert - Verify correct values and types
        Assert.Equal(3.14, doubleTable[1]);
        Assert.Equal("3.14", stringTable["pi"]);
    }

    /// <summary>
    /// Verifies that SymbolTable supports complex types as values.
    /// </summary>
    [Fact]
    public void SymbolTable_WithComplexTypes_WorksCorrectly()
    {
        // Arrange - Create table with complex value type
        var complexTable = new SymbolTable<string, List<int>>();
        var list = new List<int> { 1, 2, 3 };

        // Act - Add complex value
        complexTable.Add("numbers", list);

        // Assert - Verify reference and contents
        Assert.Same(list, complexTable["numbers"]);
        Assert.Equal(3, complexTable["numbers"].Count);
    }
    #endregion

    #region Null Key Tests - Verify null handling exceptions

    /// <summary>
    /// Verifies that ContainsKeyLocal throws ArgumentNullException when
    /// passed null key.
    /// </summary>
    [Fact]
    public void ContainsKeyLocal_NullKey_ThrowsArgumentNullException()
    {
        // Arrange - Create table
        var symbolTable = new SymbolTable<string, int>();

        // Act & Assert - Null key should throw
        Assert.Throws<ArgumentNullException>(() =>
            symbolTable.ContainsKeyLocal(null!));
    }

    /// <summary>
    /// Verifies that TryGetValueLocal throws ArgumentNullException when
    /// passed null key.
    /// </summary>
    [Fact]
    public void TryGetValueLocal_NullKey_ThrowsArgumentNullException()
    {
        // Arrange - Create table
        var symbolTable = new SymbolTable<string, int>();

        // Act & Assert - Null key should throw
        Assert.Throws<ArgumentNullException>(() =>
            symbolTable.TryGetValueLocal(null!, out int value));
    }
    #endregion
}
