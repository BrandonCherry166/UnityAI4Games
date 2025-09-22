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
        //This is for centering the maze on screen
        float mazeWidth = maze.width * cellSize;
        float mazeHeight = maze.height * cellSize;
        Vector2 offset = new Vector2(-mazeWidth / 2f + cellSize / 2f, -mazeHeight / 2f + cellSize / 2f);

        for (int r = 0; r < maze.height; r++)
        {
            for (int c = 0; c < maze.width; c++)
            {
                Vector3 cellPos = new Vector3(c * cellSize, r * cellSize, 0) + (Vector3)offset;
                if (r < maze.height - 1 && maze.horizontalWalls[r * maze.width + c])
                {
                    Instantiate(wallPrefab, cellPos + new Vector3(0, cellSize / 2, 0), Quaternion.identity);
                }

                if (c < maze.width - 1 && maze.verticalWalls[r * (maze.width - 1) + c])
                {
                    Instantiate(wallPrefab, cellPos + new Vector3(cellSize / 2, 0, 0), Quaternion.Euler(0,0,90));
                }
            }
        }
    }
}
