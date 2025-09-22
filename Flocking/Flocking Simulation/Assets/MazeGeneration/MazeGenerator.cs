using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public float cellSize = 1.0f;
    public int width = 10, height = 10;

    private void Start()
    {
        Maze maze = new Maze(width, height);
        maze.Generate();
        DrawMaze(maze);
    }

    void DrawMaze(Maze maze)
    {
        for (int r = 0; r < maze.height; r++)
        {
            for (int c = 0; c < maze.width; c++)
            {
                if (r < maze.height - 1 && maze.horizontalWalls[r * maze.width + c])
                {
                    Instantiate(wallPrefab, new Vector3(c * cellSize, r * cellSize + cellSize / 2, 0), Quaternion.identity);
                }

                if (c < maze.width - 1 && maze.verticalWalls[r * (maze.width - 1) + c])
                {
                    Instantiate(wallPrefab, new Vector3(c * cellSize + cellSize / 2, r * cellSize, 0), Quaternion.Euler(0,0,90));
                }
            }
        }
    }
}
