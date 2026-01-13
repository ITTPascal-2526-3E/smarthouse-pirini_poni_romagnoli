using BlaisePascal.SmartHouse.Domain.Abstraction;
using BlaisePascal.SmartHouse.Domain.illumination;
using System;

// NO PATTERN CHECKBOARD
// FINO A 
// REVERSE COLUMS
public class MatrixLed : Device
{
    public Led[][] matrix { get; private set; } // Declare the matrix field

    public int width { get; private set; }
    public int height { get; private set; }

    public MatrixLed(string name, bool status, int wid, int hei)
        : base(name, status)
    {
        width = wid;
        height = hei;

        // Initialize the matrix with the correct dimensions
        matrix = new Led[height][];
        for (int i = 0; i < height; i++)
        {
            matrix[i] = new Led[width];
            for (int j = 0; j < width; j++)
            {
                // Corretto uso della concatenazione/interpolazione: includo le coordinate i,j nel nome
                matrix[i][j] = new Led($"led {i},{j}", false, ColorOption.White);
            }
        }
    }

    public void TurnallOn()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (matrix[i][j] != null) // check if led is not absent
                {
                    matrix[i][j].ToggleOn();
                }
            }
        }
    }

    public void TurnallOff()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (matrix[i][j] != null) // check if led is not absent
                {
                    matrix[i][j].ToggleOff();
                }
            }
        }
    }

    public void SetAllIntensity(int intensity)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (matrix[i][j] != null) // check if led is not absent
                {
                    matrix[i][j].SetLightIntensity(intensity);
                }
            }
        }
    }


    public Led GetLamp(int x, int y)
    {
        return matrix[x][y];
    }



    public Led[] GetLedsInRow(int row)
    {
        if (row < 0 || row >= height)
        {
            return null;
        }
        return matrix[row];
    }

    public Led[] GetLampInColumn(int column)
    {
        if (column < 0 || column >= width)
            return null;

        Led[] columnLeds = new Led[height];// Create an array to hold the column LEDs
        for (int i = 0; i < height; i++)
        {
            columnLeds[i] = matrix[i][column];
        }
        return columnLeds;
    }
}
