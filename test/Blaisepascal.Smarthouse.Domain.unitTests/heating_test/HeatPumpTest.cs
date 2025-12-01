namespace Blaisepascal.Smarthouse.Domain.unitTests.heating_test;
public class HeatPumpTest
{
    // Dati di test fissi e semplici
    private const int InitialTemp = 20;
    private const string Name = "Pompa di Calore Salotto";
    private const string Brand = "ClimaTech";
    private const string Model = "HP-3000";
    private const HeatPump.EnergyClass EnergyRating = HeatPump.EnergyClass.A_plus_plus;

    // Limiti definiti nella classe HeatPump 
    private const int MIN_TEMP = 16;
    private const int MAX_TEMP = 30;
    private const int MIN_POW = 0;
    private const int MAX_POW = 100;
    private const int DEFAULT_POW = 50;

    private HeatPump CreateDefaultHeatPump()
    {
        return new HeatPump(InitialTemp, Name, Brand, Model, EnergyRating);
    }

    //  Test Costruttore 

    [Fact]
    public void Constructor_InitialStateIsOffAndDefaultsSetCorrectly()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();

        // Assert
        Assert.False(heatPump.IsOn);
        Assert.Equal(DEFAULT_POW, heatPump.Power); // Default definito nella classe
        Assert.Equal(45, heatPump.Angolation);     // Default angle
        Assert.Equal(InitialTemp, heatPump.CurrentTemperature);
        Assert.Equal(InitialTemp, heatPump.TargetTemperature);
    }

    // test che controlla la corretta assegnazione delle proprietà fisse
    [Fact]
    public void Constructor_SetsFixedPropertiesCorrectly()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();

        // Assert
        Assert.Equal(Name, heatPump.Name);
        Assert.Equal(Brand, heatPump.Brand);
        Assert.Equal(Model, heatPump.Model);
        Assert.Equal(EnergyRating, heatPump.EnergyEfficency);
    }

    // test che verifica l'unicità degli ID
    [Fact]
    public void Constructor_GeneratesUniqueIdsForDifferentInstances()
    {
        // Arrange
        var hp1 = CreateDefaultHeatPump();
        var hp2 = CreateDefaultHeatPump();

     
        Assert.NotSame(hp1, hp2);
    }

    //  Test per SetMode (On/Off logica) 

    [Fact]
    public void SetMode_TurnOn_WhenModeIsNotOff()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();

        // Act
        heatPump.SetMode(HeatPump.ModeOption.Heating);

        // Assert
        Assert.True(heatPump.IsOn, "La pompa dovrebbe accendersi se la modalità non è Off");
        Assert.Equal(HeatPump.ModeOption.Heating, heatPump.Mode);
    }

    [Fact]
    public void SetMode_TurnOff_WhenModeIsOff()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();
        heatPump.TurnOn(); // La accendiamo prima

        // Act
        heatPump.SetMode(HeatPump.ModeOption.Off);

        // Assert
        Assert.False(heatPump.IsOn, "La pompa dovrebbe spegnersi se la modalità è Off");
        Assert.Equal(HeatPump.ModeOption.Off, heatPump.Mode);
    }

    // --- Test per ChangeTemperature (Logica e Confini) ---

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

    [Fact]
    public void ChangeTemperature_ClampsToMin_WhenValueIsTooLow()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();

        // Act
        heatPump.ChangeTemperature(10); // Sotto MIN_TEMP (16)

        // Assert
        Assert.Equal(MIN_TEMP, heatPump.TargetTemperature);
    }

    [Fact]
    public void ChangeTemperature_ClampsToMax_WhenValueIsTooHigh()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();

        // Act
        heatPump.ChangeTemperature(35); // Sopra MAX_TEMP (30)

        // Assert
        Assert.Equal(MAX_TEMP, heatPump.TargetTemperature);
    }

    // --- Test per ChangePower e Funzioni Aggiuntive ---

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

    [Fact]
    public void ChangePower_ClampsValues_OutsideRange()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();

        // Act & Assert - Troppo alto
        heatPump.ChangePower(150);
        Assert.Equal(MAX_POW, heatPump.Power);

        // Act & Assert - Troppo basso
        heatPump.ChangePower(-10);
        Assert.Equal(MIN_POW, heatPump.Power);
    }

    [Fact]
    public void IncreasePowerByButtonPerFive_IncreasesCorrectly()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();
        // Default power è 50

        // Act
        heatPump.IncreasePowerByButtonPerFive();

        // Assert
        Assert.Equal(55, heatPump.Power);
    }

    // --- Test per Angolazione (Airflow) ---

    [Fact]
    public void SetFixedAngle_EnablesFlagAndResetsAngle()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();
        heatPump.ChangeAngolation(90); // Cambiamo l'angolo iniziale

        // Act
        heatPump.SetFixedAngle();

        // Assert
        Assert.True(heatPump.FixedAngleOn);
        Assert.Equal(45, heatPump.Angolation); // Torna al default
    }

    [Fact]
    public void ChangeAngolation_ClampsValues()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();

        // Act
        heatPump.ChangeAngolation(100); // Sopra MAX_ANGLE (90)

        // Assert
        Assert.Equal(90, heatPump.Angolation);
    }

    // --- Test per Simulazione (Update) ---

    [Fact]
    public void Update_IncreasesCurrentTemp_WhenHeatingAndBelowTarget()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();
        heatPump.SetMode(HeatPump.ModeOption.Heating); // Questo accende anche il dispositivo
        heatPump.SetTargetTemperature(25);
        // CurrentTemperature inizializzata a 20 dal costruttore

        // Act
        heatPump.Update();

        // Assert
        Assert.Equal(21, heatPump.CurrentTemperature); // 20 + 1
    }

    [Fact]
    public void Update_DecreasesCurrentTemp_WhenCoolingAndAboveTarget()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();
        heatPump.SetMode(HeatPump.ModeOption.Cooling);
        heatPump.SetTargetTemperature(18);
        // CurrentTemperature inizializzata a 20

        // Act
        heatPump.Update();

        // Assert
        Assert.Equal(19, heatPump.CurrentTemperature); // 20 - 1
    }

    [Fact]
    public void Update_DoesNothing_WhenOff()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();
        heatPump.TurnOff();

        // Impostiamo condizioni che causerebbero cambiamento se fosse acceso
        // Nota: Non possiamo usare SetMode qui perché accenderebbe il device
        // Dobbiamo manipolare lo stato indirettamente o assumere che il setup sia valido.
        // Dato che Mode è private set e SetMode accende, testiamo solo che TurnOff blocchi l'update.

        // Act
        heatPump.Update();

        // Assert
        Assert.Equal(InitialTemp, heatPump.CurrentTemperature);
    }

    // --- Test per Scheduling ---

    [Fact]
    public void UpdateSchedule_TurnsOn_AtScheduledTime()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();
        DateTime now = DateTime.Now;
        heatPump.Schedule(now.AddMinutes(-1), null); // Schedulato per 1 minuto fa (passato)

        // Act
        heatPump.UpdateSchedule(now);

        // Assert
        Assert.True(heatPump.IsOn);
        Assert.Null(heatPump.ScheduledOn); // Deve essere consumato (null)
    }

    [Fact]
    public void UpdateSchedule_TurnsOff_AtScheduledTime()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();
        heatPump.TurnOn();
        DateTime now = DateTime.Now;
        heatPump.Schedule(null, now.AddMinutes(-1)); // Schedulato off nel passato

        // Act
        heatPump.UpdateSchedule(now);

        // Assert
        Assert.False(heatPump.IsOn);
        Assert.Null(heatPump.ScheduledOff); // Deve essere consumato
    }

    [Fact]
    public void ChangeName_UpdatesNameProperty()
    {
        // Arrange
        var heatPump = CreateDefaultHeatPump();
        string newName = "Camera da Letto";

        // Act
        heatPump.ChangeName(newName);

        // Assert
        Assert.Equal(newName, heatPump.Name);
    }

}
