using Xunit;

public class CalculatorTests
{
    [Fact]
    public void Add_TwoNumbers_ReturnsCorrectSum()
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        int result = calculator.Add(2, 3);

        // Assert
        Assert.Equal(5, result);
    }
}
