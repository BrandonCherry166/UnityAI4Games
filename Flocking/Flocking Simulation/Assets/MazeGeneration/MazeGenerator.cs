using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour 
{
    public GameObject wallPrefab;
    public float cellSize = 1.0f;
    public int width = 10, height = 10;
    public bool isRunning;
    private Transform mazeParent;
    Maze maze;

    private GameObject[,] hWallObjects;
    private GameObject[,] vWallObjects;



    private void Start()
    {
        maze = new Maze(width, height);
        mazeParent = new GameObject("MazeWalls").transform;
        InitializeWalls();
    }

    private void Update()
    {
        if (isRunning)
        {
            if (maze.Step())
            {
                UpdateWalls(); // Only remove walls that were broken
            }
            else
            {
                isRunning = false; // Done
            }
        }
    }

    public bool StepMaze()
    {
        if (maze == null) return false;

        bool stepDone = maze.Step();
        UpdateWalls();
        return stepDone;
    }

    public void ResetMaze(int side)
    {
        width = height = side;

        if (mazeParent != null)
        {
            Destroy(mazeParent.gameObject);
        }
        mazeParent = new GameObject("MazeWalls").transform;

        if (maze == null || maze.width != width || maze.height != height)
        {
            maze = new Maze(width, height);
        } 
        else
        {
            maze.ResetStep();
        }
        InitializeWalls();
    }

    void InitializeWalls()
    {
        hWallObjects = new GameObject[maze.height - 1, maze.width];
        vWallObjects = new GameObject[maze.height, maze.width - 1];

        float offsetX = -maze.width * cellSize / 2f + cellSize / 2f;
        float offsetY = -maze.height * cellSize / 2f + cellSize / 2f;

        for (int r = 0; r < maze.height; r++)
        {
            for (int c = 0; c < maze.width; c++)
            {
                Vector3 cellPos = new Vector3(c * cellSize + offsetX, r * cellSize + offsetY, 0);

                if (r < maze.height - 1)
                {
                    hWallObjects[r, c] = Instantiate(wallPrefab, cellPos + new Vector3(0, cellSize / 2, 0), Quaternion.identity, mazeParent);
                }
                if (c < maze.width - 1)
                {
                    vWallObjects[r, c] = Instantiate(wallPrefab, cellPos + new Vector3(cellSize / 2, 0, 0), Quaternion.Euler(0, 0, 90), mazeParent);
                } 
            }
        }

        //Outer Walls Generation
        for (int c = 0; c < maze.width; c++)
        {
            //Top Right Exit
            if (c == maze.width - 1)
            {
                continue;
            }

            Vector3 topPos = new Vector3(c * cellSize + offsetX, (maze.height - 1) * cellSize + offsetY + cellSize / 2f, 0);
            Instantiate(wallPrefab, topPos, Quaternion.identity, mazeParent);
        }

        for (int c = 0; c < maze.width; c++)
        {
            //Bottom Left Exit
            if (c == 0)
            {
                continue;
            }

            Vector3 bottomPos = new Vector3(c * cellSize + offsetX, offsetY - cellSize / 2f, 0);
            Instantiate(wallPrefab, bottomPos, Quaternion.identity, mazeParent);
        }

        for (int r = 0; r < maze.height; r++)
        {
            //Bottom Left Exit
            if (r == 0)
            {
                continue;
            }

            Vector3 leftPos = new Vector3(offsetX - cellSize / 2f, r * cellSize + offsetY, 0);
            Instantiate(wallPrefab, leftPos, Quaternion.Euler(0, 0, 90), mazeParent);
        }

        for (int r = 0; r < maze.height; r++)
        {
            //Top Right Exit
            if (r == maze.height - 1)
            {
                continue;
            }

            Vector3 rightPos = new Vector3((maze.width - 1) * cellSize + offsetX + cellSize / 2f, r * cellSize + offsetY, 0);
            Instantiate(wallPrefab, rightPos, Quaternion.Euler(0, 0, 90), mazeParent);
        }
    }

    void UpdateWalls()
    {
        for (int r = 0; r < maze.height - 1; r++)
        {
            for (int c = 0; c < maze.width; c++)
            {
                if (!maze.horizontalWalls[r * maze.width + c] && hWallObjects[r, c] != null)
                {
                    Destroy(hWallObjects[r, c]);
                    hWallObjects[r, c] = null;
                }
            }       
        }
    
        for (int r = 0; r < maze.height; r++)
        {
            for (int c = 0; c < maze.width - 1; c++)
            {
                if (!maze.verticalWalls[r * (maze.width - 1) + c] && vWallObjects[r, c] != null)
                {
                    Destroy(vWallObjects[r, c]);
                    vWallObjects[r, c] = null;
                }
            }
                
        } 
    }
}
