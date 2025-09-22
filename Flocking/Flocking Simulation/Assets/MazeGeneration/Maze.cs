using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public int width, height;
    public bool[] horizontalWalls;
    public bool[] verticalWalls;
    public bool[] visitedCells;

    private System.Random rng;

    public Maze(int w, int h, int seed = -1)
    {
        this.width = w;
        this.height = h;

        horizontalWalls = new bool[(height - -1) * width];
        verticalWalls = new bool[(width - 1) * height];
        visitedCells = new bool[height * width];


        //Set everything to true 
        for (int i = 0; i < horizontalWalls.Length; i++)
        {
            horizontalWalls[i] = true;
        }

        for (int i = 0; i < verticalWalls.Length; i++)
        {
            verticalWalls[i] = true;
        }

        rng = seed == -1 ? new System.Random() : new System.Random(seed);
    }

    //Linearization
    int CellIndex(int row, int col)
    {
        return row * width + col;
    }

    int HWallIndex(int row, int col)
    {
        return row * width + col;
    }

    int VWallIndex(int row, int col)
    {
        return row * (width - 1) + col;
    }

    void MarkVisited(int row, int col)
    {
        visitedCells[CellIndex(row, col)] = true;
    }

    bool IsVisited(int row, int col)
    {
        return visitedCells[CellIndex(row, col)];
    }
}
