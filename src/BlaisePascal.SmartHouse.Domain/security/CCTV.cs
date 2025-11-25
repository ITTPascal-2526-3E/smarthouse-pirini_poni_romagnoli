using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



     public class CCTV
    {
        private Guid Id { get; }
        // CCTV attributes
        private string Model { get; set; }
        private string Brand { get; set; }
        private string Resolution { get; set; }
        public bool IsRecording { get; private set; }
        public int zoomLevel { get; private set; }
        private int TelephotoLevel { get; set; }
        private int WideAngleLevel { get; set; }
        public bool IsNightVisionOn { get; private set; }
        private string name { get; set; }
        // Constructor
        public CCTV(string model, string brand, string resolution, int cameraTelephotoLevel, int cameraWideAngleLevel, string cameraName)
        {
            Model = model;
            Brand = brand;
            Resolution = resolution;
            IsRecording = false;
            TelephotoLevel = cameraTelephotoLevel;
            WideAngleLevel = cameraWideAngleLevel;
            name = cameraName;
            Id = Guid.NewGuid();
        }
        // Start recording
        public void StartRecording()
        {
            zoomLevel = WideAngleLevel;
            IsRecording = true;
        }
        // Stop recording
        public void StopRecording()
        {
            IsRecording = false;
            zoomLevel = 0;
        }
        public void zoom(int zoomLevelWanted)
        {
            if (IsRecording == false)
            {
                return;
            }
            else if (IsRecording == true)
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

