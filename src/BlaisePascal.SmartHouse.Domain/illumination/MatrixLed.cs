using BlaisePascal.SmartHouse.Domain;
using System;

// NO PATTERN CHECKBOARD
// FINO A 
// REVERSE COLUMS
public class MatrixLed : Device
{
    private Led[][] matrix; // Declare the matrix field

    private int width;
    private int height;

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
                    matrix[i][j].TurnOn();
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
                    matrix[i][j].TurnOff();
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
}
