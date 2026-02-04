using Containers;
using Xunit;

namespace ContainersTests;

public class SymbolTableTest
{
        [Fact]
        public void Add_ShouldAddKeyValuePair()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            string key = "testKey";
            int value = 1;

            // Act
            symbolTable.Add(key, value);

            // Assert
            Assert.True(symbolTable.ContainsKey(key));
            Assert.Equal(value, symbolTable[key]);
        }

        [Fact]
        public void Remove_ShouldRemoveKeyValuePair()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            string key = "testKey";
            int value = 1;
            symbolTable.Add(key, value);

            // Act
            bool removed = symbolTable.Remove(key);

            // Assert
            Assert.True(removed);
            Assert.False(symbolTable.ContainsKey(key));
        }

        [Theory]
        [InlineData("key1", 1)]
        [InlineData("key2", 2)]
        public void TryGetValue_ShouldReturnTrueForExistingKey(string key, int expectedValue)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add(key, expectedValue);

            // Act
            bool result = symbolTable.TryGetValue(key, out int actualValue);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Clear_ShouldRemoveAllItems()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 1);
            symbolTable.Add("key2", 2);

            // Act
            symbolTable.Clear();

            // Assert
            Assert.Equal(0, symbolTable.Count);
        }

        [Fact]
        public void ContainsKeyLocal_ShouldReturnTrueForExistingKey()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            string key = "testKey";
            symbolTable.Add(key, 1);

            // Act
            bool contains = symbolTable.ContainsKeyLocal(key);

            // Assert
            Assert.True(contains);
        }

        [Fact]
        public void TryGetValueLocal_ShouldReturnFalseForNonExistingKey()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            string key = "nonExistingKey";

            // Act
            bool result = symbolTable.TryGetValueLocal(key, out int value);

            // Assert
            Assert.False(result);
        }
    }
