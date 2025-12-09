using System;
using Xunit;
using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.heating;

namespace Blaisepascal.Smarthouse.Domain.unitTests.heating_test
{
    public class HeatPumpTest
    {
        // Fixed test data for readability
        private const int InitialTemp = 20;
        private const string Name = "Living Room Heat Pump";
        private const string Brand = "ClimaTech";
        private const string Model = "HP-3000";
        private const EnergyClass EnergyRating = EnergyClass.A_plus_plus;

        // Limits defined in the HeatPump class
        private const int MinTemp = 16;
        private const int MaxTemp = 30;
        private const int MinPower = 0;
        private const int MaxPower = 100;
        private const int DefaultPower = 50;
        private const int DefaultAngle = 45;

        // Helper method to create a default heat pump
        private HeatPump CreateDefaultHeatPump()
        {
            return new HeatPump(InitialTemp, Name, Brand, Model, EnergyRating);
        }

        // The test verifies that the constructor sets defaults and initial OFF state correctly
        [Fact]
        public void Constructor_InitialStateIsOffAndDefaultsSetCorrectly()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();

            // Assert
            Assert.False(heatPump.IsOn);
            Assert.Equal(DefaultPower, heatPump.Power);
            Assert.Equal(DefaultAngle, heatPump.Angle);
            Assert.Equal(InitialTemp, heatPump.CurrentTemperature);
            Assert.Equal(InitialTemp, heatPump.TargetTemperature);
        }

        // The test checks that fixed properties are correctly assigned by the constructor
        [Fact]
        public void Constructor_SetsFixedPropertiesCorrectly()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();

            // Assert
            Assert.Equal(Name, heatPump.Name);
            Assert.Equal(Brand, heatPump.Brand);
            Assert.Equal(Model, heatPump.Model);
            Assert.Equal(EnergyRating, heatPump.EnergyEfficiency);
        }

        // The test checks that each instance gets a unique DeviceId
        [Fact]
        public void Constructor_GeneratesUniqueIdsForDifferentInstances()
        {
            // Arrange
            var hp1 = CreateDefaultHeatPump();
            var hp2 = CreateDefaultHeatPump();

            // Assert
            Assert.NotEqual(hp1.DeviceId, hp2.DeviceId);
        }

        // The test verifies that setting a non-Off mode turns the heat pump ON
        [Fact]
        public void SetMode_TurnOn_WhenModeIsNotOff()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();

            // Act
            heatPump.SetMode(ModeOptionHeatPump.Heating);

            // Assert
            Assert.True(heatPump.IsOn);
            Assert.Equal(ModeOptionHeatPump.Heating, heatPump.Mode);
        }

        // The test verifies that setting Off mode turns the heat pump OFF
        [Fact]
        public void SetMode_TurnOff_WhenModeIsOff()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();
            heatPump.TurnOn();

            // Act
            heatPump.SetMode(ModeOptionHeatPump.Off);

            // Assert
            Assert.False(heatPump.IsOn);
            Assert.Equal(ModeOptionHeatPump.Off, heatPump.Mode);
        }

        // The test checks that a valid temperature is applied as target
        [Fact]
        public void ChangeTemperature_UpdatesValue_WhenValueIsValid()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();
            int validTemp = 24;

            // Act
            heatPump.ChangeTemperature(validTemp);

            // Assert
            Assert.Equal(validTemp, heatPump.TargetTemperature);
        }

        // The test checks that temperature is clamped to the minimum when too low
        [Fact]
        public void ChangeTemperature_ClampsToMin_WhenValueIsTooLow()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();

            // Act
            heatPump.ChangeTemperature(10);

            // Assert
            Assert.Equal(MinTemp, heatPump.TargetTemperature);
        }

        // The test checks that temperature is clamped to the maximum when too high
        [Fact]
        public void ChangeTemperature_ClampsToMax_WhenValueIsTooHigh()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();

            // Act
            heatPump.ChangeTemperature(35);

            // Assert
            Assert.Equal(MaxTemp, heatPump.TargetTemperature);
        }

        // The test verifies that ChangePower accepts values inside the allowed range
        [Fact]
        public void ChangePower_UpdatesValue_WithinRange()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();

            // Act
            heatPump.ChangePower(80);

            // Assert
            Assert.Equal(80, heatPump.Power);
        }

        // The test verifies that ChangePower clamps values outside the allowed range
        [Fact]
        public void ChangePower_ClampsValues_OutsideRange()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();

            // Act
            heatPump.ChangePower(150);
            // Assert: too high
            Assert.Equal(MaxPower, heatPump.Power);

            // Act
            heatPump.ChangePower(-10);
            // Assert: too low
            Assert.Equal(MinPower, heatPump.Power);
        }

        // The test checks that the power increases by 5 when using the button method
        [Fact]
        public void IncreasePowerByButtonPerFive_IncreasesCorrectly()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();

            // Act
            heatPump.IncreasePowerByButtonPerFive();

            // Assert
            Assert.Equal(DefaultPower + 5, heatPump.Power);
        }

        // The test verifies that SetFixedAngle enables the flag and resets angle to default
        [Fact]
        public void SetFixedAngle_EnablesFlagAndResetsAngle()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();
            heatPump.ChangeAngle(90);

            // Act
            heatPump.SetFixedAngle();

            // Assert
            Assert.True(heatPump.FixedAngleOn);
            Assert.Equal(DefaultAngle, heatPump.Angle);
        }

        // The test verifies that ChangeAngle clamps values above the maximum
        [Fact]
        public void ChangeAngle_ClampsValuesAboveMax()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();

            // Act
            heatPump.ChangeAngle(100);

            // Assert
            Assert.Equal(90, heatPump.Angle);
        }

        // The test checks that Update increases temperature by 1 in heating mode when below target
        [Fact]
        public void Update_IncreasesCurrentTemp_WhenHeatingAndBelowTarget()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();
            heatPump.SetMode(ModeOptionHeatPump.Heating);
            heatPump.SetTargetTemperature(25);

            // Act
            heatPump.Update();

            // Assert
            Assert.Equal(InitialTemp + 1, heatPump.CurrentTemperature);
        }

        // The test checks that Update decreases temperature by 1 in cooling mode when above target
        [Fact]
        public void Update_DecreasesCurrentTemp_WhenCoolingAndAboveTarget()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();
            heatPump.SetMode(ModeOptionHeatPump.Cooling);
            heatPump.SetTargetTemperature(18);

            // Act
            heatPump.Update();

            // Assert
            Assert.Equal(InitialTemp - 1, heatPump.CurrentTemperature);
        }

        // The test verifies that Update does nothing when the pump is OFF
        [Fact]
        public void Update_DoesNothing_WhenOff()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();
            heatPump.TurnOff();

            // Act
            heatPump.Update();

            // Assert
            Assert.Equal(InitialTemp, heatPump.CurrentTemperature);
        }

        // The test verifies that UpdateSchedule turns the heat pump ON at the scheduled ON time
        [Fact]
        public void UpdateSchedule_TurnsOn_AtScheduledTime()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();
            DateTime now = DateTime.Now;
            heatPump.Schedule(now.AddMinutes(-1), null);

            // Act
            heatPump.UpdateSchedule(now);

            // Assert
            Assert.True(heatPump.IsOn);
            Assert.Null(heatPump.ScheduledOn);
        }

        // The test verifies that UpdateSchedule turns the heat pump OFF at the scheduled OFF time
        [Fact]
        public void UpdateSchedule_TurnsOff_AtScheduledTime()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();
            heatPump.TurnOn();
            DateTime now = DateTime.Now;
            heatPump.Schedule(null, now.AddMinutes(-1));

            // Act
            heatPump.UpdateSchedule(now);

            // Assert
            Assert.False(heatPump.IsOn);
            Assert.Null(heatPump.ScheduledOff);
        }

        // The test verifies that ChangeName updates the Name property
        [Fact]
        public void ChangeName_UpdatesNameProperty()
        {
            // Arrange
            var heatPump = CreateDefaultHeatPump();
            string newName = "Bedroom Heat Pump";

            // Act
            heatPump.ChangeName(newName);

            // Assert
            Assert.Equal(newName, heatPump.Name);
        }
    }
}
