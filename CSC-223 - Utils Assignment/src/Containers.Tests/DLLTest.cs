using Containers;
using Xunit;

namespace ContainersTests;

public class DLLTest
{
    #region Constructor Tests
    [Fact]
    public void Constructor_CreatesEmptyList()
    {
        var dll = new DLL<int>();
        Assert.True(dll.IsEmpty());
        Assert.Equal(0, dll.Count);
        Assert.Equal(0, dll.Size());
    }
    #endregion

    #region PushBack/PushFront Tests
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void PushBack_AddsMultipleElements_CountIncreases(int count)
    {
        var dll = new DLL<int>();
        for (int i = 0; i < count; i++)
        {
            dll.PushBack(i);
        }
        Assert.Equal(count, dll.Count);
        Assert.Equal(count, dll.Size());
    }

    [Fact]
    public void PushBack_AddsElementsInOrder()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);

        Assert.Equal(1, dll.Front());
        Assert.Equal(3, dll.Back());
    }

    [Fact]
    public void PushFront_AddsElementsToFront()
    {
        var dll = new DLL<int>();
        dll.PushFront(1);
        dll.PushFront(2);
        dll.PushFront(3);

        Assert.Equal(3, dll.Front());
        Assert.Equal(1, dll.Back());
    }

    [Fact]
    public void PushBackAndPushFront_MixedOperations()
    {
        var dll = new DLL<int>();
        dll.PushBack(2);
        dll.PushFront(1);
        dll.PushBack(3);

        Assert.Equal(1, dll.Front());
        Assert.Equal(3, dll.Back());
        Assert.Equal(3, dll.Count);
    }

    [Fact]
    public void PushBack_WithDuplicateData()
    {
        var dll = new DLL<int>();
        dll.PushBack(5);
        dll.PushBack(5);
        dll.PushBack(5);

        Assert.Equal(3, dll.Count);
        Assert.Equal(5, dll.Front());
        Assert.Equal(5, dll.Back());
    }
    #endregion

    #region PopBack/PopFront Tests
    [Fact]
    public void PopBack_RemovesFromEnd()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);
        dll.PopBack();

        Assert.Equal(2, dll.Count);
        Assert.Equal(2, dll.Back());
    }

    [Fact]
    public void PopFront_RemovesFromStart()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);
        dll.PopFront();

        Assert.Equal(2, dll.Count);
        Assert.Equal(2, dll.Front());
    }

    [Fact]
    public void PopBack_EmptyList_ThrowsException()
    {
        var dll = new DLL<int>();
        Assert.Throws<InvalidOperationException>(() => dll.PopBack());
    }

    [Fact]
    public void PopFront_EmptyList_ThrowsException()
    {
        var dll = new DLL<int>();
        Assert.Throws<InvalidOperationException>(() => dll.PopFront());
    }

    [Fact]
    public void PopBack_SingleElement()
    {
        var dll = new DLL<int>();
        dll.PushBack(42);
        dll.PopBack();

        Assert.True(dll.IsEmpty());
        Assert.Equal(0, dll.Count);
    }

    [Fact]
    public void PopFront_SingleElement()
    {
        var dll = new DLL<int>();
        dll.PushBack(42);
        dll.PopFront();

        Assert.True(dll.IsEmpty());
        Assert.Equal(0, dll.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void PopBack_MultipleElements_CountDecreases(int count)
    {
        var dll = new DLL<int>();
        for (int i = 0; i < count; i++)
        {
            dll.PushBack(i);
        }
        for (int i = 0; i < count; i++)
        {
            dll.PopBack();
        }
        Assert.True(dll.IsEmpty());
    }
    #endregion

    #region Front/Back Tests
    [Fact]
    public void Front_EmptyList_ThrowsException()
    {
        var dll = new DLL<int>();
        Assert.Throws<InvalidOperationException>(() => dll.Front());
    }

    [Fact]
    public void Back_EmptyList_ThrowsException()
    {
        var dll = new DLL<int>();
        Assert.Throws<InvalidOperationException>(() => dll.Back());
    }

    [Fact]
    public void Front_SingleElement()
    {
        var dll = new DLL<int>();
        dll.PushBack(99);

        Assert.Equal(99, dll.Front());
        Assert.Equal(99, dll.Back());
    }
    #endregion

    #region Add Tests
    [Fact]
    public void Add_AddsSameAsPushBack()
    {
        var dll = new DLL<int>();
        dll.Add(1);
        dll.Add(2);
        dll.Add(3);

        Assert.Equal(3, dll.Count);
        Assert.Equal(1, dll.Front());
        Assert.Equal(3, dll.Back());
    }
    #endregion

    #region Insert Tests
    [Fact]
    public void Insert_AtBeginning()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(3);
        dll.Insert(0, 2);

        Assert.Equal(3, dll.Count);
        Assert.Equal(2, dll[0]);
        Assert.Equal(1, dll[1]);
        Assert.Equal(3, dll[2]);
    }

    [Fact]
    public void Insert_AtValidEnd()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.Insert(1, 3); // Insert at index 1 (before the element at index 1)

        Assert.Equal(3, dll.Count);
        Assert.Equal(3, dll[1]);
        Assert.Equal(2, dll[2]);
    }

    [Fact]
    public void Insert_InMiddle()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(3);
        dll.Insert(1, 2);

        Assert.Equal(3, dll.Count);
        Assert.Equal(1, dll[0]);
        Assert.Equal(2, dll[1]);
        Assert.Equal(3, dll[2]);
    }

    [Fact]
    public void Insert_NegativeIndex_ThrowsException()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        Assert.Throws<ArgumentOutOfRangeException>(() => dll.Insert(-1, 2));
    }

    [Fact]
    public void Insert_IndexOutOfBounds_ThrowsException()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        Assert.Throws<ArgumentOutOfRangeException>(() => dll.Insert(5, 2));
    }

    [Fact]
    public void Insert_AtIndexZero_EmptyList_Throws()
    {
        var dll = new DLL<int>();
        // Insert at index 0 on an empty list should throw because there's no node at index 0
        Assert.Throws<ArgumentOutOfRangeException>(() => dll.Insert(0, 42));
    }
    #endregion

    #region Remove/RemoveAt Tests
    [Fact]
    public void Remove_ExistingElement_ReturnsTrue()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);

        bool result = dll.Remove(2);

        Assert.True(result);
        Assert.Equal(2, dll.Count);
        Assert.False(dll.Contains(2));
    }

    [Fact]
    public void Remove_NonExistentElement_ReturnsFalse()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);

        bool result = dll.Remove(99);

        Assert.False(result);
        Assert.Equal(1, dll.Count);
    }

    [Fact]
    public void Remove_FirstOccurrenceOfDuplicate()
    {
        var dll = new DLL<int>();
        dll.PushBack(5);
        dll.PushBack(5);
        dll.PushBack(5);

        bool result = dll.Remove(5);

        Assert.True(result);
        Assert.Equal(2, dll.Count);
        Assert.Equal(5, dll.Front());
    }

    [Fact]
    public void RemoveAt_AtBeginning()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);
        dll.RemoveAt(0);

        Assert.Equal(2, dll.Count);
        Assert.Equal(2, dll.Front());
    }

    [Fact]
    public void RemoveAt_AtEnd()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);
        dll.RemoveAt(2);

        Assert.Equal(2, dll.Count);
        Assert.Equal(2, dll.Back());
    }

    [Fact]
    public void RemoveAt_InMiddle()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);
        dll.RemoveAt(1);

        Assert.Equal(2, dll.Count);
        Assert.Equal(1, dll[0]);
        Assert.Equal(3, dll[1]);
    }

    [Fact]
    public void RemoveAt_InvalidIndex_ThrowsException()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        Assert.Throws<ArgumentOutOfRangeException>(() => dll.RemoveAt(5));
    }

    [Fact]
    public void RemoveAt_NegativeIndex_ThrowsException()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        Assert.Throws<ArgumentOutOfRangeException>(() => dll.RemoveAt(-1));
    }
    #endregion

    #region Contains Tests
    [Fact]
    public void Contains_ExistingElement_ReturnsTrue()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);

        Assert.True(dll.Contains(2));
    }

    [Fact]
    public void Contains_NonExistentElement_ReturnsFalse()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);

        Assert.False(dll.Contains(99));
    }

    [Fact]
    public void Contains_EmptyList_ReturnsFalse()
    {
        var dll = new DLL<int>();
        Assert.False(dll.Contains(1));
    }

    [Fact]
    public void Contains_WithDuplicates()
    {
        var dll = new DLL<int>();
        dll.PushBack(5);
        dll.PushBack(5);
        dll.PushBack(5);

        Assert.True(dll.Contains(5));
    }
    #endregion

    #region IndexOf Tests
    [Fact]
    public void IndexOf_ExistingElement()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);

        Assert.Equal(1, dll.IndexOf(2));
    }

    [Fact]
    public void IndexOf_NonExistentElement_ReturnsNegativeOne()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);

        Assert.Equal(-1, dll.IndexOf(99));
    }

    [Fact]
    public void IndexOf_FirstOccurrenceOfDuplicate()
    {
        var dll = new DLL<int>();
        dll.PushBack(5);
        dll.PushBack(3);
        dll.PushBack(5);

        Assert.Equal(0, dll.IndexOf(5));
    }

    [Fact]
    public void IndexOf_EmptyList()
    {
        var dll = new DLL<int>();
        Assert.Equal(-1, dll.IndexOf(1));
    }
    #endregion

    #region Indexer Tests
    [Fact]
    public void Indexer_Get()
    {
        var dll = new DLL<int>();
        dll.PushBack(10);
        dll.PushBack(20);
        dll.PushBack(30);

        Assert.Equal(10, dll[0]);
        Assert.Equal(20, dll[1]);
        Assert.Equal(30, dll[2]);
    }

    [Fact]
    public void Indexer_Set()
    {
        var dll = new DLL<int>();
        dll.PushBack(10);
        dll.PushBack(20);
        dll[1] = 25;

        Assert.Equal(25, dll[1]);
    }

    [Fact]
    public void Indexer_InvalidIndex_ThrowsException()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        Assert.Throws<ArgumentOutOfRangeException>(() => { var x = dll[5]; });
    }

    [Fact]
    public void Indexer_NegativeIndex_ThrowsException()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        Assert.Throws<ArgumentOutOfRangeException>(() => { var x = dll[-1]; });
    }
    #endregion

    #region Clear Tests
    [Fact]
    public void Clear_RemovesAllElements()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);
        dll.Clear();

        Assert.True(dll.IsEmpty());
        Assert.Equal(0, dll.Count);
    }

    [Fact]
    public void Clear_EmptyList()
    {
        var dll = new DLL<int>();
        dll.Clear();

        Assert.True(dll.IsEmpty());
    }

    [Fact]
    public void Clear_AllowsAddingAfterClear()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.Clear();
        dll.PushBack(2);

        Assert.Equal(1, dll.Count);
        Assert.Equal(2, dll.Front());
    }
    #endregion

    #region IsEmpty Tests
    [Fact]
    public void IsEmpty_EmptyList_ReturnsTrue()
    {
        var dll = new DLL<int>();
        Assert.True(dll.IsEmpty());
    }

    [Fact]
    public void IsEmpty_NonEmptyList_ReturnsFalse()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        Assert.False(dll.IsEmpty());
    }

    [Fact]
    public void IsEmpty_AfterRemovingAllElements()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PopFront();
        Assert.True(dll.IsEmpty());
    }
    #endregion

    #region Count/Size Tests
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Count_ReflectsNumberOfElements(int count)
    {
        var dll = new DLL<int>();
        for (int i = 0; i < count; i++)
        {
            dll.PushBack(i);
        }

        Assert.Equal(count, dll.Count);
        Assert.Equal(count, dll.Size());
    }
    #endregion

    #region CopyTo Tests
    [Fact]
    public void CopyTo_CopiesAllElements()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);

        int[] array = new int[3];
        dll.CopyTo(array, 0);

        Assert.Equal(new int[] { 1, 2, 3 }, array);
    }

    [Fact]
    public void CopyTo_WithOffset()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);

        int[] array = new int[4];
        array[0] = 99;
        dll.CopyTo(array, 1);

        Assert.Equal(new int[] { 99, 1, 2, 0 }, array);
    }

    [Fact]
    public void CopyTo_EmptyList()
    {
        var dll = new DLL<int>();
        int[] array = new int[0];
        dll.CopyTo(array, 0); // Should not throw

        Assert.Empty(array);
    }
    #endregion

    #region ToString Tests
    [Fact]
    public void ToString_EmptyList()
    {
        var dll = new DLL<int>();
        Assert.Equal("[]", dll.ToString());
    }

    [Fact]
    public void ToString_SingleElement()
    {
        var dll = new DLL<int>();
        dll.PushBack(42);
        Assert.Equal("[42]", dll.ToString());
    }

    [Fact]
    public void ToString_MultipleElements()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);
        Assert.Equal("[1, 2, 3]", dll.ToString());
    }

    [Fact]
    public void ToString_WithDuplicates()
    {
        var dll = new DLL<int>();
        dll.PushBack(5);
        dll.PushBack(5);
        dll.PushBack(5);
        Assert.Equal("[5, 5, 5]", dll.ToString());
    }

    [Fact]
    public void ToString_WithStrings()
    {
        var dll = new DLL<string>();
        dll.PushBack("hello");
        dll.PushBack("world");
        Assert.Equal("[hello, world]", dll.ToString());
    }
    #endregion

    #region Enumerator Tests
    [Fact]
    public void Enumerator_IteratesThroughAllElements()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);
        dll.PushBack(3);

        var list = new List<int>();
        foreach (var item in dll)
        {
            list.Add(item);
        }

        Assert.Equal(new int[] { 1, 2, 3 }, list);
    }

    [Fact]
    public void Enumerator_EmptyList()
    {
        var dll = new DLL<int>();
        var list = new List<int>();

        foreach (var item in dll)
        {
            list.Add(item);
        }

        Assert.Empty(list);
    }

    [Fact]
    public void Enumerator_MultipleIterations()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushBack(2);

        var list1 = new List<int>();
        foreach (var item in dll)
        {
            list1.Add(item);
        }

        var list2 = new List<int>();
        foreach (var item in dll)
        {
            list2.Add(item);
        }

        Assert.Equal(list1, list2);
    }
    #endregion

    #region IsReadOnly Tests
    [Fact]
    public void IsReadOnly_IsFalse()
    {
        var dll = new DLL<int>();
        Assert.False(dll.IsReadOnly);
    }
    #endregion

    #region Integration Tests
    [Fact]
    public void ComplexScenario_MultipleOperations()
    {
        var dll = new DLL<int>();
        dll.PushBack(1);
        dll.PushFront(0);
        dll.PushBack(2);
        dll.Insert(2, 1);
        dll.Remove(1);
        dll.PushBack(3);

        Assert.Equal(4, dll.Count);
        Assert.Equal("[0, 1, 2, 3]", dll.ToString());
    }

    [Fact]
    public void ComplexScenario_WithDuplicatesAndRemoval()
    {
        var dll = new DLL<int>();
        for (int i = 0; i < 5; i++)
        {
            dll.PushBack(i % 2); // [0, 1, 0, 1, 0]
        }

        dll.Remove(0); // Remove first 0: [1, 0, 1, 0]
        Assert.Equal(4, dll.Count);
        Assert.Equal(0, dll.IndexOf(1)); // First 1 is at index 0
    }

    [Fact]
    public void ComplexScenario_LargeList()
    {
        var dll = new DLL<int>();
        int size = 100;
        for (int i = 0; i < size; i++)
        {
            dll.PushBack(i);
        }

        Assert.Equal(size, dll.Count);
        Assert.Equal(0, dll.Front());
        Assert.Equal(size - 1, dll.Back());
        Assert.True(dll.Contains(50));
        Assert.Equal(50, dll.IndexOf(50));
    }

    [Fact]
    public void ComplexScenario_AlternatingPushes()
    {
        var dll = new DLL<int>();
        dll.PushBack(2);
        dll.PushFront(1);
        dll.PushBack(3);
        dll.PushFront(0);
        dll.PushBack(4);

        Assert.Equal(5, dll.Count);
        Assert.Equal("[0, 1, 2, 3, 4]", dll.ToString());
    }
    #endregion
}
