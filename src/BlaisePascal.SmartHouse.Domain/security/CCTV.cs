using System;
using BlaisePascal.SmartHouse.Domain.Abstraction;

namespace BlaisePascal.SmartHouse.Domain.security
{
    // Represents a CCTV camera device with zoom and night vision capabilities
    public class CCTV : Device
    {
        // CCTV model name
        public string Model { get; private set; }

        // CCTV brand name
        public string Brand { get; private set; }

        // CCTV video resolution (for example "1080p")
        public string Resolution { get; private set; }

        // Current zoom level of the camera
        public int ZoomLevel { get; private set; }

        // Maximum telephoto zoom level allowed by the camera
        public int TelephotoLevel { get; private set; }

        // Minimum wide-angle zoom level allowed by the camera
        public int WideAngleLevel { get; private set; }

        // Indicates whether night vision mode is active
        public bool IsNightVisionOn { get; private set; }

        // Constructor initializes the CCTV camera with model, brand, resolution and zoom levels
        public CCTV(string model,string brand,string resolution,int cameraTelephotoLevel,int cameraWideAngleLevel,string name,bool status)
            : base(name, status) // Pass name and status to the Device base class
        {
            Model = model;
            Brand = brand;
            Resolution = resolution;
            TelephotoLevel = cameraTelephotoLevel;
            WideAngleLevel = cameraWideAngleLevel;

            // Initialize zoom level based on current status
            ZoomLevel = status ? WideAngleLevel : 0;

            Touch();
        }

        // Starts recording by turning the camera ON and setting the zoom to wide angle
        public void StartRecording()
        {
            ZoomLevel = WideAngleLevel;
            ToggleOn();
            Touch();
        }

        // Stops recording by turning the camera OFF and resetting the zoom level
        public void StopRecording()
        {
            ToggleOff();
            ZoomLevel = 0;
            Touch();
        }

        // Changes the zoom level if the camera is ON and the requested level is in the valid range
        public void Zoom(int zoomLevelWanted)
        {
            // If the camera is OFF, zoom changes are ignored
            if (!Status)
            {
                return;
            }

            // Zoom is valid only if it is between wide angle and telephoto levels
            if (zoomLevelWanted <= TelephotoLevel && zoomLevelWanted >= WideAngleLevel)
            {
                ZoomLevel = zoomLevelWanted;
                Touch();
            }
        }

        // Toggles night vision mode ON or OFF and updates the last modified timestamp
        public void ToggleNightVision()
        {
            IsNightVisionOn = !IsNightVisionOn;
            Touch();
        }
    }
}
