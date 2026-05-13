using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Exploration.Tests;

public class CalculatorTests
{
    private readonly Calculator _calculator;

    public CalculatorTests()
    {
        _calculator = new Calculator();
    }

[Fact]

public void Calculate_WhenAdd_ReturnCorrectResult()
    {
        // Arrange
        var calculator = new Calculator();
        int x = 5;
        int y = 2;
        int expected = 7;

        // Act
        var actual = calculator.Add(x,y);

        // Assert
        Assert.Equal(expected, actual);

    }


[Fact]
public void Calculate_WhenSubtract_ReturnCorrectResult()
    {
        // Arrange
        var calculator = new Calculator();
        int x = 5;
        int y = 2;
        int expected = 3;

        // Act
        var actual = calculator.Subtract(x,y);

        // Assert
        Assert.Equal(expected, actual);

    }

[Fact]
public void Calculate_WhenDivideByZero_ThrowsDivideByZeroException()
    {
        Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10,0));
    }

[Theory]
//[InlineData(5,2,10)]
// [InlineData(int.MinValue, 2, int.MinValue)]
[MemberData(nameof(MultiplyTestData))]

public void Calculate_WhenMultiply_ReturnCorrectResult(int x, int y, int expected)
    {

        var actual = _calculator.Multiply(x,y);

        //Assert.Equal(expected, actual);

        actual.Should().Be(expected); //FluentAssertion macht das so
    }

    public static TheoryData<int, int, int> MultiplyTestData =>
    new()
    {
        {1, 2, 2},
        {2, 5, 10},
        {3, 6, 18},

    };
}