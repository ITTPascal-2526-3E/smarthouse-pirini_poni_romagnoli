using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class CCTV : Device
{
    private Guid Id { get; }

    // CCTV attributes
    private string Model { get; set; }
    private string Brand { get; set; }
    private string Resolution { get; set; }
    
    public int zoomLevel { get; private set; }
    private int TelephotoLevel { get; set; }
    private int WideAngleLevel { get; set; }
    public bool IsNightVisionOn { get; private set; }

    

    // Constructor
    
    public CCTV(string model, string brand, string resolution, int cameraTelephotoLevel, int cameraWideAngleLevel, string Name , bool status)
        : base(Name, status) // Passiamo nome e status al genitore Device
    {
        Model = model;
        Brand = brand;
        Resolution = resolution;
        base.Status = status; // lo status simboleggia se la telecamera è accesa o spenta
        TelephotoLevel = cameraTelephotoLevel;
        WideAngleLevel = cameraWideAngleLevel;
        Id = Guid.NewGuid();
    }

    // Start recording
    public void StartRecording()
    {
        zoomLevel = WideAngleLevel;
        Status = true;
    }

    // Stop recording
    public void StopRecording()
    {
        Status = false;
        zoomLevel = 0;
    }

    public void zoom(int zoomLevelWanted)
    {
        if (Status == false)
        {
            return;
        }
        else if (Status == true)
        {
            if (zoomLevelWanted <= TelephotoLevel && zoomLevelWanted >= WideAngleLevel)
            {
                zoomLevel = zoomLevelWanted;
            }
        }
    }

    public void ToggleNightVision()
    {
        IsNightVisionOn = !IsNightVisionOn;
    }
}