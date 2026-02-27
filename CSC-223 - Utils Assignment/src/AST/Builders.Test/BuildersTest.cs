/**
 * Comprehensive test suite for Builder classes (DefaultBuilder, DebugBuilder, NullBuilder).
 * Tests node creation, unparsing, statement generation, and debug output behavior.
 *
 * Bugs: None known at this time
 *
 * @author Jalil Garvin-Mingo
 * @date   February 27, 2026
 */

using Xunit;
using AST;
using Containers;
using System;
using System.IO;

namespace AST.Tests
{
    public class DefaultBuilderTests
    {
        private readonly DefaultBuilder _builder = new DefaultBuilder();

        #region LiteralNode Tests
        // Tests for LiteralNode creation with various integer values and type validation
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(42)]
        [InlineData(-100)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void CreateLiteralNode_WithVariousIntegers_CreatesNodeWithCorrectValue(int value)
        {
            // Arrange & Act
            var node = _builder.CreateLiteralNode(value);

            // Assert
            Assert.NotNull(node);
            Assert.IsType<LiteralNode>(node);
            Assert.Equal(value, node.Value);
            Assert.Equal(value.ToString(), node.Unparse());
        }

        [Fact]
        public void CreateLiteralNode_WithNonIntegerValue_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => _builder.CreateLiteralNode("string"));
            Assert.Throws<ArgumentException>(() => _builder.CreateLiteralNode(3.14));
            Assert.Throws<ArgumentException>(() => _builder.CreateLiteralNode(new object()));
        }
        #endregion

        #region VariableNode Tests
        // Tests for VariableNode creation with various naming patterns
        [Theory]
        [InlineData("x")]
        [InlineData("y")]
        [InlineData("variable")]
        [InlineData("_var")]
        [InlineData("var123")]
        [InlineData("a")]
        [InlineData("longVariableNameExample")]
        public void CreateVariableNode_WithVariousNames_CreatesNodeWithCorrectName(string name)
        {
            // Arrange & Act
            var node = _builder.CreateVariableNode(name);

            // Assert
            Assert.NotNull(node);
            Assert.IsType<VariableNode>(node);
            Assert.Equal(name, node.Name);
            Assert.Equal(name, node.Unparse());
        }
        #endregion

        #region Binary Operator Tests
        // Tests for all binary operators (Plus, Minus, Times, FloatDiv, IntDiv, Modulus, Exponentiation)
        // including correctness of operands, structure, and unparsed output
        [Fact]
        public void CreatePlusNode_WithExpressionNodes_CreatesNodeWithCorrectOperands()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(5);
            var right = _builder.CreateLiteralNode(3);

            // Act
            var node = _builder.CreatePlusNode(left, right);

            // Assert
            Assert.NotNull(node);
            Assert.IsType<PlusNode>(node);
            Assert.Same(left, node.Left);
            Assert.Same(right, node.Right);
            Assert.Equal("(5 + 3)", node.Unparse());
        }

        [Fact]
        public void CreatePlusNode_WithComplexExpressions_CreatesCorrectStructure()
        {
            // Arrange
            var lit1 = _builder.CreateLiteralNode(2);
            var lit2 = _builder.CreateLiteralNode(3);
            var plusNode = _builder.CreatePlusNode(lit1, lit2);
            var lit3 = _builder.CreateLiteralNode(4);

            // Act
            var result = _builder.CreatePlusNode(plusNode, lit3);

            // Assert
            Assert.Equal("((2 + 3) + 4)", result.Unparse());
        }

        [Fact]
        public void CreateMinusNode_WithExpressionNodes_CreatesNodeWithCorrectOperands()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(10);
            var right = _builder.CreateLiteralNode(4);

            // Act
            var node = _builder.CreateMinusNode(left, right);

            // Assert
            Assert.NotNull(node);
            Assert.IsType<MinusNode>(node);
            Assert.Same(left, node.Left);
            Assert.Same(right, node.Right);
            Assert.Equal("(10 - 4)", node.Unparse());
        }

        [Fact]
        public void CreateTimesNode_WithExpressionNodes_CreatesNodeWithCorrectOperands()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(6);
            var right = _builder.CreateLiteralNode(7);

            // Act
            var node = _builder.CreateTimesNode(left, right);

            // Assert
            Assert.NotNull(node);
            Assert.IsType<TimesNode>(node);
            Assert.Same(left, node.Left);
            Assert.Same(right, node.Right);
            Assert.Equal("(6 * 7)", node.Unparse());
        }

        [Fact]
        public void CreateFloatDivNode_WithExpressionNodes_CreatesNodeWithCorrectOperands()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(20);
            var right = _builder.CreateLiteralNode(4);

            // Act
            var node = _builder.CreateFloatDivNode(left, right);

            // Assert
            Assert.NotNull(node);
            Assert.IsType<FloatDivNode>(node);
            Assert.Same(left, node.Left);
            Assert.Same(right, node.Right);
            Assert.Equal("(20 / 4)", node.Unparse());
        }

        [Fact]
        public void CreateIntDivNode_WithExpressionNodes_CreatesNodeWithCorrectOperands()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(17);
            var right = _builder.CreateLiteralNode(5);

            // Act
            var node = _builder.CreateIntDivNode(left, right);

            // Assert
            Assert.NotNull(node);
            Assert.IsType<IntDivNode>(node);
            Assert.Same(left, node.Left);
            Assert.Same(right, node.Right);
            Assert.Equal("(17 // 5)", node.Unparse());
        }

        [Fact]
        public void CreateModulusNode_WithExpressionNodes_CreatesNodeWithCorrectOperands()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(17);
            var right = _builder.CreateLiteralNode(5);

            // Act
            var node = _builder.CreateModulusNode(left, right);

            // Assert
            Assert.NotNull(node);
            Assert.IsType<ModulusNode>(node);
            Assert.Same(left, node.Left);
            Assert.Same(right, node.Right);
            Assert.Equal("(17 % 5)", node.Unparse());
        }

        [Fact]
        public void CreateExponentiationNode_WithExpressionNodes_CreatesNodeWithCorrectOperands()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(2);
            var right = _builder.CreateLiteralNode(8);

            // Act
            var node = _builder.CreateExponentiationNode(left, right);

            // Assert
            Assert.NotNull(node);
            Assert.IsType<ExponentiationNode>(node);
            Assert.Same(left, node.Left);
            Assert.Same(right, node.Right);
            Assert.Equal("(2 ** 8)", node.Unparse());
        }

        [Fact]
        public void BinaryOperators_WithVariableNodes_CreatesCorrectUnparse()
        {
            // Arrange
            var x = _builder.CreateVariableNode("x");
            var y = _builder.CreateVariableNode("y");

            // Act & Assert
            Assert.Equal("(x + y)", _builder.CreatePlusNode(x, y).Unparse());
            Assert.Equal("(x - y)", _builder.CreateMinusNode(x, y).Unparse());
            Assert.Equal("(x * y)", _builder.CreateTimesNode(x, y).Unparse());
            Assert.Equal("(x / y)", _builder.CreateFloatDivNode(x, y).Unparse());
            Assert.Equal("(x // y)", _builder.CreateIntDivNode(x, y).Unparse());
            Assert.Equal("(x % y)", _builder.CreateModulusNode(x, y).Unparse());
            Assert.Equal("(x ** y)", _builder.CreateExponentiationNode(x, y).Unparse());
        }

        [Fact]
        public void BinaryOperators_WithMixedNodeTypes_CreatesCorrectStructure()
        {
            // Arrange
            var lit = _builder.CreateLiteralNode(5);
            var var = _builder.CreateVariableNode("x");

            // Act & Assert
            Assert.Equal("(5 + x)", _builder.CreatePlusNode(lit, var).Unparse());
            Assert.Equal("(x - 5)", _builder.CreateMinusNode(var, lit).Unparse());
        }
        #endregion

        #region Statement Tests
        // Tests for statement creation (AssignmentStmt, ReturnStmt, BlockStmt)
        // including correct structure, unparsing, and indentation handling
        [Fact]
        public void CreateAssignmentStmt_WithVariableAndExpression_CreatesStatementWithCorrectOperands()
        {
            // Arrange
            var variable = _builder.CreateVariableNode("x");
            var expression = _builder.CreateLiteralNode(42);

            // Act
            var stmt = _builder.CreateAssignmentStmt(variable, expression);

            // Assert
            Assert.NotNull(stmt);
            Assert.IsType<AssignmentStmt>(stmt);
            Assert.Same(variable, stmt.Variable);
            Assert.Same(expression, stmt.Expression);
            Assert.Equal("x = 42", stmt.Unparse(0));
        }

        [Fact]
        public void CreateAssignmentStmt_WithComplexExpression_CreatesCorrectUnparse()
        {
            // Arrange
            var variable = _builder.CreateVariableNode("result");
            var left = _builder.CreateLiteralNode(10);
            var right = _builder.CreateLiteralNode(5);
            var expression = _builder.CreatePlusNode(left, right);

            // Act
            var stmt = _builder.CreateAssignmentStmt(variable, expression);

            // Assert
            Assert.Equal("result = (10 + 5)", stmt.Unparse(0));
        }

        [Fact]
        public void CreateAssignmentStmt_WithIndentation_CreatesCorrectIndentedUnparse()
        {
            // Arrange
            var variable = _builder.CreateVariableNode("x");
            var expression = _builder.CreateLiteralNode(100);
            var stmt = _builder.CreateAssignmentStmt(variable, expression);

            // Act & Assert
            Assert.Equal("x = 100", stmt.Unparse(0));
            Assert.Equal("    x = 100", stmt.Unparse(1));
            Assert.Equal("        x = 100", stmt.Unparse(2));
        }

        [Fact]
        public void CreateReturnStmt_WithExpression_CreatesStatementWithCorrectExpression()
        {
            // Arrange
            var expression = _builder.CreateLiteralNode(99);

            // Act
            var stmt = _builder.CreateReturnStmt(expression);

            // Assert
            Assert.NotNull(stmt);
            Assert.IsType<ReturnStmt>(stmt);
            Assert.Same(expression, stmt.Expression);
            Assert.Equal("return 99", stmt.Unparse(0));
        }

        [Fact]
        public void CreateReturnStmt_WithComplexExpression_CreatesCorrectUnparse()
        {
            // Arrange
            var left = _builder.CreateVariableNode("x");
            var right = _builder.CreateLiteralNode(10);
            var expression = _builder.CreateTimesNode(left, right);

            // Act
            var stmt = _builder.CreateReturnStmt(expression);

            // Assert
            Assert.Equal("return (x * 10)", stmt.Unparse(0));
        }

        [Fact]
        public void CreateReturnStmt_WithIndentation_CreatesCorrectIndentedUnparse()
        {
            // Arrange
            var expression = _builder.CreateLiteralNode(42);
            var stmt = _builder.CreateReturnStmt(expression);

            // Act & Assert
            Assert.Equal("return 42", stmt.Unparse(0));
            Assert.Equal("    return 42", stmt.Unparse(1));
            Assert.Equal("        return 42", stmt.Unparse(2));
        }

        [Fact]
        public void CreateBlockStmt_WithSymbolTable_CreatesBlockWithCorrectTable()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var block = _builder.CreateBlockStmt(symbolTable);

            // Assert
            Assert.NotNull(block);
            Assert.IsType<BlockStmt>(block);
            Assert.Same(symbolTable, block.SymbolTable);
        }

        [Fact]
        public void CreateBlockStmt_WithEmptySymbolTable_CreatesCorrectUnparse()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var block = _builder.CreateBlockStmt(symbolTable);
            var result = block.Unparse(0);

            // Assert
            Assert.Contains("{", result);
            Assert.Contains("}", result);
        }

        [Fact]
        public void CreateBlockStmt_WithStatements_CreatesCorrectUnparse()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            var stmt1 = _builder.CreateAssignmentStmt(_builder.CreateVariableNode("x"), _builder.CreateLiteralNode(10));
            var stmt2 = _builder.CreateReturnStmt(_builder.CreateVariableNode("x"));
            
            symbolTable.Add("stmt1", stmt1);
            symbolTable.Add("stmt2", stmt2);

            // Act
            var block = _builder.CreateBlockStmt(symbolTable);
            var result = block.Unparse(0);

            // Assert
            Assert.Contains("{", result);
            Assert.Contains("}", result);
            Assert.Contains("x = 10", result);
            Assert.Contains("return x", result);
        }
        #endregion

        #region Complex Expression Tests
        // Tests for nested and complex expressions to verify correct AST structure
        [Fact]
        public void CreateComplexExpression_WithNestedOperators_CreatesCorrectStructure()
        {
            // Arrange - Create: ((2 + 3) * (4 - 1))
            var lit2 = _builder.CreateLiteralNode(2);
            var lit3 = _builder.CreateLiteralNode(3);
            var plus = _builder.CreatePlusNode(lit2, lit3);

            var lit4 = _builder.CreateLiteralNode(4);
            var lit1 = _builder.CreateLiteralNode(1);
            var minus = _builder.CreateMinusNode(lit4, lit1);

            // Act
            var times = _builder.CreateTimesNode(plus, minus);

            // Assert
            Assert.Equal("((2 + 3) * (4 - 1))", times.Unparse());
        }

        [Fact]
        public void CreateComplexExpression_WithDeeplyNestedOperators_CreatesCorrectStructure()
        {
            // Arrange - Create: (((1 + 2) - 3) * 4)
            var lit1 = _builder.CreateLiteralNode(1);
            var lit2 = _builder.CreateLiteralNode(2);
            var plus = _builder.CreatePlusNode(lit1, lit2);

            var lit3 = _builder.CreateLiteralNode(3);
            var minus = _builder.CreateMinusNode(plus, lit3);

            var lit4 = _builder.CreateLiteralNode(4);

            // Act
            var times = _builder.CreateTimesNode(minus, lit4);

            // Assert
            Assert.Equal("(((1 + 2) - 3) * 4)", times.Unparse());
        }
        #endregion
    }

    public class DebugBuilderTests
    {
        private readonly DebugBuilder _builder = new DebugBuilder();

        #region Node Creation Tests
        // Tests that DebugBuilder produces console output while creating correct node types
        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-100)]
        public void CreateLiteralNode_ProducesDebugOutputAndCreatesCorrectNode(int value)
        {
            // Arrange
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreateLiteralNode(value);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating LiteralNode", output);
                    Assert.Contains(value.ToString(), output);
                    Assert.NotNull(node);
                    Assert.Equal(value, node.Value);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateVariableNode_ProducesDebugOutputAndCreatesCorrectNode()
        {
            // Arrange
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreateVariableNode("testVar");

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating VariableNode", output);
                    Assert.Contains("testVar", output);
                    Assert.NotNull(node);
                    Assert.Equal("testVar", node.Name);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreatePlusNode_ProducesDebugOutputAndCreatesCorrectNode()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(5);
            var right = _builder.CreateLiteralNode(3);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreatePlusNode(left, right);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating PlusNode", output);
                    Assert.NotNull(node);
                    Assert.IsType<PlusNode>(node);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateMinusNode_ProducesDebugOutputAndCreatesCorrectNode()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(10);
            var right = _builder.CreateLiteralNode(4);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreateMinusNode(left, right);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating MinusNode", output);
                    Assert.NotNull(node);
                    Assert.IsType<MinusNode>(node);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateTimesNode_ProducesDebugOutputAndCreatesCorrectNode()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(6);
            var right = _builder.CreateLiteralNode(7);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreateTimesNode(left, right);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating TimesNode", output);
                    Assert.NotNull(node);
                    Assert.IsType<TimesNode>(node);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateFloatDivNode_ProducesDebugOutputAndCreatesCorrectNode()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(20);
            var right = _builder.CreateLiteralNode(4);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreateFloatDivNode(left, right);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating FloatDivNode", output);
                    Assert.NotNull(node);
                    Assert.IsType<FloatDivNode>(node);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateIntDivNode_ProducesDebugOutputAndCreatesCorrectNode()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(17);
            var right = _builder.CreateLiteralNode(5);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreateIntDivNode(left, right);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating IntDivNode", output);
                    Assert.NotNull(node);
                    Assert.IsType<IntDivNode>(node);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateModulusNode_ProducesDebugOutputAndCreatesCorrectNode()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(17);
            var right = _builder.CreateLiteralNode(5);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreateModulusNode(left, right);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating ModulusNode", output);
                    Assert.NotNull(node);
                    Assert.IsType<ModulusNode>(node);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateExponentiationNode_ProducesDebugOutputAndCreatesCorrectNode()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(2);
            var right = _builder.CreateLiteralNode(8);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreateExponentiationNode(left, right);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating ExponentiationNode", output);
                    Assert.NotNull(node);
                    Assert.IsType<ExponentiationNode>(node);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateAssignmentStmt_ProducesDebugOutputAndCreatesCorrectStatement()
        {
            // Arrange
            var variable = _builder.CreateVariableNode("x");
            var expression = _builder.CreateLiteralNode(42);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var stmt = _builder.CreateAssignmentStmt(variable, expression);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating AssignmentStmt", output);
                    Assert.NotNull(stmt);
                    Assert.IsType<AssignmentStmt>(stmt);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateReturnStmt_ProducesDebugOutputAndCreatesCorrectStatement()
        {
            // Arrange
            var expression = _builder.CreateLiteralNode(99);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var stmt = _builder.CreateReturnStmt(expression);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating ReturnStmt", output);
                    Assert.NotNull(stmt);
                    Assert.IsType<ReturnStmt>(stmt);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void CreateBlockStmt_ProducesDebugOutputAndCreatesCorrectStatement()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var block = _builder.CreateBlockStmt(symbolTable);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating BlockStmt", output);
                    Assert.NotNull(block);
                    Assert.IsType<BlockStmt>(block);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }
        #endregion

        #region Operator Output Tests
        // Tests that debug output includes operand values and detailed creation information
        [Fact]
        public void DebugBuilder_WithBinaryOperators_IncludesOperandValuesInOutput()
        {
            // Arrange
            var left = _builder.CreateLiteralNode(5);
            var right = _builder.CreateLiteralNode(3);
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var node = _builder.CreatePlusNode(left, right);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("left: 5", output);
                    Assert.Contains("right: 3", output);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void DebugBuilder_WithComplexExpressions_ProducesDetailedOutput()
        {
            // Arrange
            var originalOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                try
                {
                    // Act
                    var lit2 = _builder.CreateLiteralNode(2);
                    var lit3 = _builder.CreateLiteralNode(3);
                    var plus = _builder.CreatePlusNode(lit2, lit3);

                    // Assert
                    var output = stringWriter.ToString();
                    Assert.Contains("Creating LiteralNode", output);
                    Assert.Contains("Creating PlusNode", output);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }
        #endregion

        #region Inheritance Tests
        // Tests that DebugBuilder properly inherits from DefaultBuilder and provides expected behavior
        [Fact]
        public void DebugBuilder_InheritsFromDefaultBuilder_CreatesSameNodeTypes()
        {
            // Arrange
            var debugBuilder = new DebugBuilder();
            var defaultBuilder = new DefaultBuilder();

            // Act
            var debugNode = debugBuilder.CreateLiteralNode(42);
            var defaultNode = defaultBuilder.CreateLiteralNode(42);

            // Assert
            Assert.Equal(debugNode.GetType(), defaultNode.GetType());
            Assert.Equal(debugNode.Unparse(), defaultNode.Unparse());
        }
        #endregion
    }

    public class NullBuilderTests
    {
        private readonly NullBuilder _builder = new NullBuilder();

        #region LiteralNode Tests
        // Tests that NullBuilder returns null for all LiteralNode creation attempts
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(42)]
        [InlineData(-100)]
        public void CreateLiteralNode_ReturnsNull(int value)
        {
            // Arrange & Act
            var node = _builder.CreateLiteralNode(value);

            // Assert
            Assert.Null(node);
        }
        #endregion

        #region VariableNode Tests
        // Tests that NullBuilder returns null for all VariableNode creation attempts
        [Theory]
        [InlineData("x")]
        [InlineData("y")]
        [InlineData("variable")]
        [InlineData("_var")]
        public void CreateVariableNode_ReturnsNull(string name)
        {
            // Arrange & Act
            var node = _builder.CreateVariableNode(name);

            // Assert
            Assert.Null(node);
        }
        #endregion

        #region Binary Operator Tests
        // Tests that NullBuilder returns null for all binary operators
        [Fact]
        public void CreatePlusNode_ReturnsNull()
        {
            // Arrange
            var left = new LiteralNode(5);
            var right = new LiteralNode(3);

            // Act
            var node = _builder.CreatePlusNode(left, right);

            // Assert
            Assert.Null(node);
        }

        [Fact]
        public void CreateMinusNode_ReturnsNull()
        {
            // Arrange
            var left = new LiteralNode(10);
            var right = new LiteralNode(4);

            // Act
            var node = _builder.CreateMinusNode(left, right);

            // Assert
            Assert.Null(node);
        }

        [Fact]
        public void CreateTimesNode_ReturnsNull()
        {
            // Arrange
            var left = new LiteralNode(6);
            var right = new LiteralNode(7);

            // Act
            var node = _builder.CreateTimesNode(left, right);

            // Assert
            Assert.Null(node);
        }

        [Fact]
        public void CreateFloatDivNode_ReturnsNull()
        {
            // Arrange
            var left = new LiteralNode(20);
            var right = new LiteralNode(4);

            // Act
            var node = _builder.CreateFloatDivNode(left, right);

            // Assert
            Assert.Null(node);
        }

        [Fact]
        public void CreateIntDivNode_ReturnsNull()
        {
            // Arrange
            var left = new LiteralNode(17);
            var right = new LiteralNode(5);

            // Act
            var node = _builder.CreateIntDivNode(left, right);

            // Assert
            Assert.Null(node);
        }

        [Fact]
        public void CreateModulusNode_ReturnsNull()
        {
            // Arrange
            var left = new LiteralNode(17);
            var right = new LiteralNode(5);

            // Act
            var node = _builder.CreateModulusNode(left, right);

            // Assert
            Assert.Null(node);
        }

        [Fact]
        public void CreateExponentiationNode_ReturnsNull()
        {
            // Arrange
            var left = new LiteralNode(2);
            var right = new LiteralNode(8);

            // Act
            var node = _builder.CreateExponentiationNode(left, right);

            // Assert
            Assert.Null(node);
        }
        #endregion

        #region Statement Tests
        // Tests that NullBuilder returns null for all statement creation attempts
        [Fact]
        public void CreateAssignmentStmt_ReturnsNull()
        {
            // Arrange
            var variable = new VariableNode("x");
            var expression = new LiteralNode(42);

            // Act
            var stmt = _builder.CreateAssignmentStmt(variable, expression);

            // Assert
            Assert.Null(stmt);
        }

        [Fact]
        public void CreateReturnStmt_ReturnsNull()
        {
            // Arrange
            var expression = new LiteralNode(99);

            // Act
            var stmt = _builder.CreateReturnStmt(expression);

            // Assert
            Assert.Null(stmt);
        }

        [Fact]
        public void CreateBlockStmt_ReturnsNull()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, object>();

            // Act
            var block = _builder.CreateBlockStmt(symbolTable);

            // Assert
            Assert.Null(block);
        }
        #endregion

        #region All Methods Test
        // Comprehensive test verifying that all creation methods return null
        [Fact]
        public void NullBuilder_AllMethods_ReturnNull()
        {
            // This test verifies that every creation method in NullBuilder returns null

            // Act & Assert - Literals and Variables
            Assert.Null(_builder.CreateLiteralNode(0));
            Assert.Null(_builder.CreateVariableNode("x"));

            // Act & Assert - Binary Operators
            var lit = new LiteralNode(1);
            var var = new VariableNode("x");
            Assert.Null(_builder.CreatePlusNode(lit, lit));
            Assert.Null(_builder.CreateMinusNode(lit, lit));
            Assert.Null(_builder.CreateTimesNode(lit, lit));
            Assert.Null(_builder.CreateFloatDivNode(lit, lit));
            Assert.Null(_builder.CreateIntDivNode(lit, lit));
            Assert.Null(_builder.CreateModulusNode(lit, lit));
            Assert.Null(_builder.CreateExponentiationNode(lit, lit));

            // Act & Assert - Statements
            Assert.Null(_builder.CreateAssignmentStmt(var, lit));
            Assert.Null(_builder.CreateReturnStmt(lit));
            Assert.Null(_builder.CreateBlockStmt(new SymbolTable<string, object>()));
        }
        #endregion

        #region Inheritance Tests
        // Tests that NullBuilder properly inherits from DefaultBuilder
        [Fact]
        public void NullBuilder_InheritsFromDefaultBuilder()
        {
            // Assert
            Assert.IsAssignableFrom<DefaultBuilder>(_builder);
        }
        #endregion
    }
}
