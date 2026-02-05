using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.SmartHouse.Domain
{
    public class Refrigerator : Device
    {
        public Fridge MyFridge { get; set; }
        public Freezer MyFreezer { get; set; }
        public Refrigerator(Fridge fridge , string name, Freezer myFreezer) : base(name, true)
        {
            MyFridge = fridge;
            MyFreezer = myFreezer;
        }

        public void OpenFridge()
        {
            MyFridge.ToggleOn();
        }

        public void CloseFridge()
        { 
            MyFridge.ToggleOff();
        }

        public void SetMyFridgeTemp(double targetTemp)
        {
            MyFridge.SetFridgeTemperature(targetTemp);
        }

        public void ReturnFridgeInformation()
        { 
            MyFridge.ToString();
        }

        public void OpenFreezer()
        { 
            MyFreezer.ToggleOn();
        }

        public void CloseFreezer()
        { 
            MyFreezer.ToggleOff();
        }

        public void SetMyFreezerTemp(double targetTemp)
        {
            MyFreezer.SetFreezerTemperature(targetTemp);
        }

        public void ReturnFreezerInformation()
        { 
            MyFreezer.ToString();
        }
    }
}
