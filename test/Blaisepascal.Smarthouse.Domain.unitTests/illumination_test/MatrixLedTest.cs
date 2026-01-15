using BlaisePascal.SmartHouse.Domain.illumination;
using Xunit;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test;

public class MatrixLedTest
{
    // All leds in the matrix should be turned on
    [Fact]
    public void AllLedsTurnedOn()
    {
        MatrixLed matrixDemo = new("TestMatrix", false, 3, 3);
        matrixDemo.TurnallOn();
        for (int i = 0; i < matrixDemo.Height; i++)
        {
            for (int j = 0; j < matrixDemo.Width; j++)
            {
                Assert.True(matrixDemo.Matrix[i][j].Status);
            }
               
        }
    }
    // All leds in the matrix should be turned off
    [Fact]
    public void AllLedsTurnedOff()
    {
        MatrixLed matrixDemo = new("TestMatrix", true, 3, 3);
        matrixDemo.TurnallOff();
        for (int i = 0; i < matrixDemo.Height; i++)
        {
            for (int j = 0; j < matrixDemo.Width; j++)
            {
                Assert.False(matrixDemo.Matrix[i][j].Status);
            }
        }
    }
    // All leds in the matrix should be turned off with specific intensity
    [Fact]
    public void AllLedsTurnedOffWithIntensity()
    {
        MatrixLed matrixDemo = new("TestMatrix", true, 3, 3);
        matrixDemo.TurnallOff();
        matrixDemo.SetAllIntensity(50);
        for (int i = 0; i < matrixDemo.Height; i++)
        {
            for (int j = 0; j < matrixDemo.Width; j++)
            {
                Assert.False(matrixDemo.Matrix[i][j].Status);
                Assert.Equal(50, matrixDemo.Matrix[i][j].LightIntensity);
            }
        }
    }
    // Get a specific led in the matrix
    [Fact]
    public void GetLeds()
    {
        MatrixLed matrixDemo = new("TestMatrix", true, 3, 3);
        matrixDemo.GetLamp(1, 1);
    }
    // Get all leds in a specific row
    [Fact]
    public void GetLedsInARow()
    {
        MatrixLed matrixDemo = new("TestMatrix", true, 3, 3);
        matrixDemo.GetLedsInRow(1);
    }
    // Get all leds in a specific column
    [Fact]
    public void GetLedsInAColumn()
    {
        MatrixLed matrixDemo = new("TestMatrix", true, 3, 3);
        matrixDemo.GetLampInColumn(1);
    }
}
