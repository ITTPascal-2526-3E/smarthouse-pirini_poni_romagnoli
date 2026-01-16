using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.illumination;
using System;

namespace BlaisePascal.SmartHouse.Domain.illumination
{
    // NO PATTERN CHECKBOARD
    // FINO A 
    // REVERSE COLUMS
    public class MatrixLed : Device, IDimmable
    {
        public Led[][] Matrix { get; private set; } // Declare the matrix field

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int Luminosity => 0; // Matrix doesn't have a single luminosity value, returning 0 or derived
        public void SetLuminosity(int value) => SetAllIntensity(value);

        public MatrixLed(string name, bool status, int wid, int hei)
            : base(name, status)
        {
            Width = wid;
            Height = hei;

            // Initialize the matrix with the correct dimensions
            Matrix = new Led[Height][];
            for (int i = 0; i < Height; i++)
            {
                Matrix[i] = new Led[Width];
                for (int j = 0; j < Width; j++)
                {
                    // Corretto uso della concatenazione/interpolazione: includo le coordinate i,j nel nome
                    Matrix[i][j] = new Led($"led {i},{j}", false, ColorOption.White);
                }
            }
        }

        public void TurnallOn()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Matrix[i][j] is not null) // check if led is not absent
                    {
                        Matrix[i][j].ToggleOn();
                    }
                }
            }
        }

        public void TurnallOff()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Matrix[i][j] is not null) // check if led is not absent
                    {
                        Matrix[i][j].ToggleOff();
                    }
                }
            }
        }

        public void SetAllIntensity(int intensity)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Matrix[i][j] is not null) // check if led is not absent
                    {
                        Matrix[i][j].SetLightIntensity(intensity);
                    }
                }
            }
        }


        public Led GetLamp(int x, int y)
        {
            return Matrix[x][y];
        }



        public Led[]? GetLedsInRow(int row)
        {
            if (row < 0 || row >= Height)
            {
                return null;
            }
            return Matrix[row];
        }

        public Led[]? GetLampInColumn(int column)
        {
            if (column < 0 || column >= Width)
                return null;

            Led[] columnLeds = new Led[Height];// Create an array to hold the column LEDs
            for (int i = 0; i < Height; i++)
            {
                columnLeds[i] = Matrix[i][column];
            }
            return columnLeds;
        }
    }
}
