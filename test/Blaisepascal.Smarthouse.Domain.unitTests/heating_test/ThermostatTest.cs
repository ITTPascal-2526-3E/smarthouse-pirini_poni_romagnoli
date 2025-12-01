namespace Blaisepascal.Smarthouse.Domain.unitTests.heating_test;
public class ThermostatTest
{
    //Adding a heat pump to the thermostat
    [Fact]
    public void TestMethod1()
    {
        Thermostat thermostat = new Thermostat(20, Thermostat.ModeOption.Heating, 22);
        HeatPump heatPump = new HeatPump(20,"DemoPump","AmericanHeatPumps","Model Y");
        thermostat.AddHeatPump(heatPump);
        Assert.Contains(heatPump, thermostat);
    }
}
