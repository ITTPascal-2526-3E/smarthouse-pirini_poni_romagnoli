using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampCompositions;
using BlaisePascal.SmartHouse.Domain.Illumination.LampAbstraction;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using Xunit;

namespace Blaisepascal.Smarthouse.Domain.unitTests.illumination_test
{
    public class MatrixLedExtendedTest
    {
        // Helper to create a 3x3 matrix
        private static MatrixLed CreateMatrix()
        {
            return new MatrixLed("TestMatrix", false, 3, 3);
        }

        // The test verifies that the IDimmable.Luminosity setter delegates to SetAllIntensity
        [Fact]
        public void IDimmable_Luminosity_Set_DelegatesToSetAllIntensity()
        {
            // Arrange
            var matrix = CreateMatrix();
            matrix.TurnallOn(); // Lamps must be ON for SetLuminosity to work

            // Act — set through IDimmable interface
            IDimmable dimmable = matrix;
            dimmable.Luminosity = new Luminosity(50);

            // Assert — all LEDs should have intensity 50
            for (int i = 0; i < matrix.Height; i++)
            {
                for (int j = 0; j < matrix.Width; j++)
                {
                    Assert.Equal(50, matrix.Matrix[i][j].CurrentLuminosity.Value);
                }
            }
        }

        // The test verifies that SetLuminosity via IDimmable interface works correctly
        [Fact]
        public void IDimmable_SetLuminosity_DelegatesToSetAllIntensity()
        {
            // Arrange
            var matrix = CreateMatrix();
            matrix.TurnallOn();

            // Act
            IDimmable dimmable = matrix;
            dimmable.SetLuminosity(new Luminosity(70));

            // Assert
            for (int i = 0; i < matrix.Height; i++)
            {
                for (int j = 0; j < matrix.Width; j++)
                {
                    Assert.Equal(70, matrix.Matrix[i][j].CurrentLuminosity.Value);
                }
            }
        }

        // The test verifies that Luminosity getter always returns 0 (MatrixLed has no single value)
        [Fact]
        public void IDimmable_Luminosity_Get_AlwaysReturnsZero()
        {
            // Arrange
            var matrix = CreateMatrix();
            matrix.TurnallOn();
            matrix.SetAllIntensity(80);

            // Act
            IDimmable dimmable = matrix;
            var luminosity = dimmable.Luminosity;

            // Assert — getter always returns 0 as per design
            Assert.Equal(0, luminosity.Value);
        }

        // The test verifies that GetLedsInRow returns null for invalid row
        [Fact]
        public void GetLedsInRow_ReturnsNull_ForInvalidRow()
        {
            // Arrange
            var matrix = CreateMatrix();

            // Act
            var result = matrix.GetLedsInRow(-1);
            var result2 = matrix.GetLedsInRow(10);

            // Assert
            Assert.Null(result);
            Assert.Null(result2);
        }

        // The test verifies that GetLampInColumn returns null for invalid column
        [Fact]
        public void GetLampInColumn_ReturnsNull_ForInvalidColumn()
        {
            // Arrange
            var matrix = CreateMatrix();

            // Act
            var result = matrix.GetLampInColumn(-1);
            var result2 = matrix.GetLampInColumn(10);

            // Assert
            Assert.Null(result);
            Assert.Null(result2);
        }

        // The test verifies valid row returns the correct number of LEDs
        [Fact]
        public void GetLedsInRow_ReturnsCorrectCount_ForValidRow()
        {
            // Arrange
            var matrix = CreateMatrix();

            // Act
            var row = matrix.GetLedsInRow(0);

            // Assert
            Assert.NotNull(row);
            Assert.Equal(3, row.Length);
        }

        // The test verifies valid column returns the correct number of LEDs
        [Fact]
        public void GetLampInColumn_ReturnsCorrectCount_ForValidColumn()
        {
            // Arrange
            var matrix = CreateMatrix();

            // Act
            var column = matrix.GetLampInColumn(0);

            // Assert
            Assert.NotNull(column);
            Assert.Equal(3, column.Length);
        }
    }
}
