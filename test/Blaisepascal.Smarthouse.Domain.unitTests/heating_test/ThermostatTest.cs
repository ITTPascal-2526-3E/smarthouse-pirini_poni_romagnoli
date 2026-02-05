using BlaisePascal.SmartHouse.Domain;
using BlaisePascal.SmartHouse.Domain.heating;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using System;
using Xunit;

namespace Blaisepascal.Smarthouse.Domain.unitTests.heating_test
{
    public class ThermostatTest
    {
        // Helper method to create a default thermostat
        private static Thermostat CreateDefaultThermostat()
        {
            return new Thermostat(new Temperature(20), ModeOptionThermostat.Off, new Temperature(22));
        }

        // Helper method to create a default heat pump
        private static HeatPump CreateDefaultHeatPump(string name = "DemoHeatPump")
        {
            return new HeatPump(new Temperature(20), name);
        }

        // Test that the constructor initializes basic properties and status correctly
        [Fact]
        public void Constructor_InitializesPropertiesAndStatus()
        {
            var thermostat = CreateDefaultThermostat();

            Assert.Equal(20, thermostat.CurrentTemperature.Value);
            Assert.Equal(22, thermostat.TargetTemperature.Value);
            Assert.Equal(ModeOptionThermostat.Off, thermostat.Mode);
            Assert.False(thermostat.Status);
        }

        // Test that adding a heat pump stores it and allows later commands to affect it
        [Fact]
        public void AddHeatPump_StoresPumpAndReceivesCommands()
        {
            var thermostat = CreateDefaultThermostat();
            var pump = CreateDefaultHeatPump();

            thermostat.AddHeatPump(pump);
            thermostat.SetTargetTemperature(new Temperature(25));

            Assert.Equal(25, pump.TargetTemperature.Value);
        }

        // Test that removing a heat pump prevents it from receiving further commands
        [Fact]
        public void RemoveHeatPump_PreventsFurtherCommands()
        {
            var thermostat = CreateDefaultThermostat();
            var pump = CreateDefaultHeatPump();

            thermostat.AddHeatPump(pump);
            thermostat.RemoveHeatPump(pump);

            thermostat.SetTargetTemperature(new Temperature(25));

            Assert.NotEqual(25, pump.TargetTemperature.Value);
        }

        // Test that SetTargetTemperature updates property and propagates to all pumps
        [Fact]
        public void SetTargetTemperature_UpdatesOwnValueAndAllPumps()
        {
            var thermostat = CreateDefaultThermostat();
            var pump1 = CreateDefaultHeatPump("Pump1");
            var pump2 = CreateDefaultHeatPump("Pump2");

            thermostat.AddHeatPump(pump1);
            thermostat.AddHeatPump(pump2);

            thermostat.SetTargetTemperature(new Temperature(24));

            Assert.Equal(24, thermostat.TargetTemperature.Value);
            Assert.Equal(24, pump1.TargetTemperature.Value);
            Assert.Equal(24, pump2.TargetTemperature.Value);
        }

        // Test that when current temperature is below target, mode becomes Heating for thermostat and pumps
        [Fact]
        public void UpdateCurrentTemperature_BelowTarget_SetsHeatingMode()
        {
            var thermostat = CreateDefaultThermostat();
            var pump1 = CreateDefaultHeatPump("Pump1");
            var pump2 = CreateDefaultHeatPump("Pump2");

            thermostat.AddHeatPump(pump1);
            thermostat.AddHeatPump(pump2);

            thermostat.SetTargetTemperature(new Temperature(22));
            thermostat.updateCurrentTemp(new Temperature(19));

            Assert.Equal(ModeOptionThermostat.Heating, thermostat.Mode);
            Assert.Equal(ModeOptionHeatPump.Heating, pump1.Mode);
            Assert.Equal(ModeOptionHeatPump.Heating, pump2.Mode);
            Assert.True(thermostat.Status);
        }

        // Test that when current temperature is above target, mode becomes Cooling for thermostat and pumps
        [Fact]
        public void UpdateCurrentTemperature_AboveTarget_SetsCoolingMode()
        {
            var thermostat = CreateDefaultThermostat();
            var pump = CreateDefaultHeatPump();

            thermostat.AddHeatPump(pump);

            thermostat.SetTargetTemperature(new Temperature(22));
            thermostat.updateCurrentTemp(new Temperature(24));

            Assert.Equal(ModeOptionThermostat.Cooling, thermostat.Mode);
            Assert.Equal(ModeOptionHeatPump.Cooling, pump.Mode);
            Assert.True(thermostat.Status);
        }

        // Test that when current temperature equals target, mode becomes Off for thermostat and pumps
        [Fact]
        public void UpdateCurrentTemperature_EqualTarget_SetsOffMode()
        {
            var thermostat = CreateDefaultThermostat();
            var pump = CreateDefaultHeatPump();

            thermostat.AddHeatPump(pump);

            thermostat.SetTargetTemperature(new Temperature(22));
            thermostat.updateCurrentTemp(new Temperature(22));

            Assert.Equal(ModeOptionThermostat.Off, thermostat.Mode);
            Assert.Equal(ModeOptionHeatPump.Off, pump.Mode);
            Assert.False(thermostat.Status);
        }

        // Test that SetMode directly changes mode, status and pump modes to Heating
        [Fact]
        public void SetMode_Heating_UpdatesStatusAndPumps()
        {
            var thermostat = CreateDefaultThermostat();
            var pump = CreateDefaultHeatPump();

            thermostat.AddHeatPump(pump);

            thermostat.SetMode(ModeOptionThermostat.Heating);

            Assert.Equal(ModeOptionThermostat.Heating, thermostat.Mode);
            Assert.True(thermostat.Status);
            Assert.Equal(ModeOptionHeatPump.Heating, pump.Mode);
        }

        // Test that SetMode Off turns OFF thermostat and all pumps
        [Fact]
        public void SetMode_Off_TurnsOffThermostatAndPumps()
        {
            var thermostat = CreateDefaultThermostat();
            var pump = CreateDefaultHeatPump();

            thermostat.AddHeatPump(pump);

            thermostat.SetMode(ModeOptionThermostat.Heating);
            thermostat.SetMode(ModeOptionThermostat.Off);

            Assert.Equal(ModeOptionThermostat.Off, thermostat.Mode);
            Assert.False(thermostat.Status);
            Assert.Equal(ModeOptionHeatPump.Off, pump.Mode);
        }
    }
}
