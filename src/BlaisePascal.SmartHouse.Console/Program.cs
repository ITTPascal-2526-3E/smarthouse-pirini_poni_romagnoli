internal class Program
{
    static void Main(string[] args)
    {
        TwoLampsDevice device = new TwoLampsDevice();

        Console.WriteLine("1) Aggiungo lampade...");

        device.addLamp(new Lamp(60, Lamp.ColorOption.WarmWhite, "ModelX", "BrandY", "A++","lampada soggiorno"));
        device.addLamp(new Lamp(40, Lamp.ColorOption.White, "ModelA", "BrandZ", "A", "lampada smart soggiorno"));
        

        Console.WriteLine($"Lamps count dopo aggiunta: {device.getLampsCount()}");

        Console.WriteLine("\n2) Accendo tutte le lampade...");
        device.TurnOnAllLamps();
        Console.WriteLine($"Turned on lamps (count): {device.getONLampsCount()}");

       

        Console.WriteLine("\n3) Imposto luminosità globale al 50%...");
        device.SetLuminosityAllLamps(50);
        

        Console.WriteLine("\n4) Spengo la lamp alla posizione 0...");
        device.turnofflampsatindex(0);
        Console.WriteLine($"Turned on lamps (count): {device.getONLampsCount()}");
        

        Console.WriteLine("\n5) Riaccendo la lamp alla posizione 0 e setto luminosità indice 1 al 20%...");
        device.turnonlampsatindex(0);
        device.setluminosityatindex(1, 20);
        

        Console.WriteLine("\n6) Rimuovo la lamp alla posizione 0...");
        device.RemoveLampAtIndex(0);
        Console.WriteLine($"Lamps count dopo rimozione: {device.getLampsCount()}");
        

        Console.WriteLine("\n7) Test funzionalità EcoLamp: register presence, schedule, update...");

        // Registrazione presenza su tutte le EcoLamp
        device.RegisterPresenceAllEcoLamps();
        Console.WriteLine("Registrata presenza su tutte le EcoLamp.");

        // Schedule: accensione tra 1 sec e spegnimento tra 2 minuti
        DateTime onTime = DateTime.Now.AddSeconds(1);
        DateTime offTime = DateTime.Now.AddMinutes(2);
        device.ScheduleAllEcoLamps(onTime, offTime);
        Console.WriteLine($"Scheduled EcoLamps ON at {onTime}, OFF at {offTime}");

        // Simulo passaggio del tempo: aggiorno con un tempo futuro per verificare comportamento di UpdateAllEcoLamps
        DateTime later = DateTime.Now.AddMinutes(6); // oltre il timeout di presenza di 5 minuti => dovrebbe ridurre luminosità delle EcoLamp
        device.UpdateAllEcoLamps(later);
        Console.WriteLine($"Eseguito UpdateAllEcoLamps con ora = {later}");
        

        Console.WriteLine("\n8) Svuoto tutte le lampade...");
        device.ClearAllLamps();
        Console.WriteLine($"Lamps count dopo ClearAllLamps: {device.getLampsCount()}");

        CCTV camera = new CCTV("ModelC", "BrandD", "1080p", 5, 2, "Front Door Camera");
        camera.StartRecording();
        camera.zoom(3);
        camera.StopRecording();
        camera.StartRecording();
        camera.ToggleNightVision();
        camera.StopRecording();
    }
}