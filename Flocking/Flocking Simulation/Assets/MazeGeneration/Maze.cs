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

    List<Neighbor> GetVisitableNeighbors(int row, int col)
    {
        var neighbors = new List<Neighbor>();

        if (row > 0 && !IsVisited(row - 1, col)) //Up
        {
            neighbors.Add(new Neighbor(row - 1, col, 'U'));
        }

        if (col + 1 < width && !IsVisited(row, col + 1)) //Right
        {
            neighbors.Add(new Neighbor(row, col + 1, 'R'));
        }

        if (row + 1 < height && !IsVisited(row + 1, col)) // Down
        {
            neighbors.Add(new Neighbor(row + 1, col, 'D'));
        }

        if (col > 0 && !IsVisited(row, col - 1)) //Left
        {
            neighbors.Add(new Neighbor(row, col - 1, 'L'));
        }

        return neighbors;
    }

    void RemoveWall(int row, int col, Neighbor nb)
    {
        switch (nb.direction)
        {
            case 'U':
                horizontalWalls[HWallIndex(row - 1, col)] = false;
                break;
            case 'R':
                verticalWalls[VWallIndex(row, col)] = false;
                break;
            case 'D':
                horizontalWalls[HWallIndex(row, col)] = false;
                break;
            case 'L':
                verticalWalls[VWallIndex(row, col - 1)] = false;
                break;
        }
    }

    public void Generate()
    {
        var stack = new Stack<(int row, int col)>();
        int row = 0, col = 0;
        MarkVisited(row, col);
        stack.Push((row,col));

        while (stack.Count > 0)
        {
            (int curRow, int curCol) = stack.Peek();
            var neighbors = GetVisitableNeighbors(curRow, curCol);

            if (neighbors.Count == 0)
            {
                stack.Pop();
            }
            else
            {
                Neighbor chosen;
                if (neighbors.Count == 1)
                {
                    chosen = neighbors[0];
                }
                else
                {
                    chosen = neighbors[rng.Next(neighbors.Count)];
                }

                RemoveWall(curRow, curCol, chosen);
                MarkVisited(chosen.row, chosen.col);
                stack.Push((chosen.row, chosen.col));
            }
        }
    }
}
