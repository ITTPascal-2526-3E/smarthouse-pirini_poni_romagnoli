namespace Blaisepascal.Smarthouse.Domain.unitTests;

public class MatrixLedTest
{
    // All leds in the matrix should be turned on
    [Fact]
    public void allLedsTurnedOn()
    {
        MatrixLed matrixDemo = new MatrixLed("TestMatrix", false, 3, 3);
        matrixDemo.TurnallOn();
        for (int i = 0; i < matrixDemo.height; i++)
        {
            for (int j = 0; j < matrixDemo.width; j++)
            {
                Assert.True(matrixDemo.matrix[i][j].Status);
            }
               
        }
    }
    // All leds in the matrix should be turned off
    [Fact]
    public void allLedsTurnedOff()
    {
        MatrixLed matrixDemo = new MatrixLed("TestMatrix", true, 3, 3);
        matrixDemo.TurnallOff();
        for (int i = 0; i < matrixDemo.height; i++)
        {
            for (int j = 0; j < matrixDemo.width; j++)
            {
                Assert.False(matrixDemo.matrix[i][j].Status);
            }
        }
    }
    // All leds in the matrix should be turned off with specific intensity
    [Fact]
    public void allLedsTurnedOffWithIntensity()
    {
        MatrixLed matrixDemo = new MatrixLed("TestMatrix", true, 3, 3);
        matrixDemo.TurnallOff(50);
        for (int i = 0; i < matrixDemo.height; i++)
        {
            for (int j = 0; j < matrixDemo.width; j++)
            {
                Assert.False(matrixDemo.matrix[i][j].Status);
                Assert.Equal(50, matrixDemo.matrix[i][j].LightIntensity);
            }
        }
    }
    // Get a specific led in the matrix
    [Fact]
    public void GetLeds()
    {
        MatrixLed matrixDemo = new MatrixLed("TestMatrix", true, 3, 3);
        matrixDemo.GetLamp(1, 1);
    }
    // Get all leds in a specific row
    [Fact]
    public void GetLedsInARow()
    {
        MatrixLed matrixDemo = new MatrixLed("TestMatrix", true, 3, 3);
        matrixDemo.GetLedsInRow(1);
    }
    // Get all leds in a specific column
    [Fact]
    public void GetLedsInAColumn()
    {
        MatrixLed matrixDemo = new MatrixLed("TestMatrix", true, 3, 3);
        matrixDemo.GetLampInColumn(1);
    }
}
