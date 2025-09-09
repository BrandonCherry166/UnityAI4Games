using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameBoard : MonoBehaviour
{
    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap newState;

    [SerializeField] private Tile deadTile;
    [SerializeField] private Tile aliveTile;

    [SerializeField] private float updateInterval = 0.05f;
    [SerializeField] private Pattern pattern;

    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;

    private void Awake() //Runs First
    {
        //Instantiating Hash Sets
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();
    }
    public void Begin() //Runs When Called
    {
        SetPattern(pattern);
    }

    private void SetPattern(Pattern pattern)
    {
        Clear();
        
        Vector2Int center = pattern.GetCenter();

        for (int i = 0; i < pattern.cells.Length; i++)
        {
            Vector3Int cell = (Vector3Int) (pattern.cells[i] - center); //One cast call
            currentState.SetTile(cell, aliveTile);
            aliveCells.Add(cell);
        }
    }

    private void Clear()
    {
        currentState.ClearAllTiles();
        newState.ClearAllTiles();
    }

    private void OnEnable()
    {
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        var interval = new WaitForSeconds(updateInterval); //Done for efficiency of memory  
        while (enabled)
        {
            UpdateState();
            yield return interval;

        }
    }

    private void UpdateState()
    {
        cellsToCheck.Clear();
        foreach(Vector3Int cell in aliveCells)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    cellsToCheck.Add(cell + new Vector3Int(dx, dy, 0));
                }
            }
        }

        foreach (Vector3Int cell in cellsToCheck)
        {
            int neighbors = CountNeighbors(cell);
            bool alive = IsAlive(cell);

            if (!alive && neighbors == 3)
            {
                newState.SetTile(cell, aliveTile);
                aliveCells.Add(cell);
            }
            else if (alive && (neighbors < 2 || neighbors > 3))
            {
                newState.SetTile(cell, deadTile);
                aliveCells.Remove(cell);
            }
            else
            {
                newState.SetTile(cell, currentState.GetTile(cell)); 
            }
        }

        //Temp Swap
        Tilemap temp = currentState;
        currentState = newState;
        newState = temp;
        newState.ClearAllTiles();
    }

    private int CountNeighbors(Vector3Int cell)
    {
        int count = 0;

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                Vector3Int neighbor = cell + new Vector3Int(dx, dy, 0);

                if (dy == 0 && dx == 0)
                {
                    continue;
                }
                else if (IsAlive(neighbor))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private bool IsAlive(Vector3Int cell)
    {
        return currentState.GetTile(cell) == aliveTile;
    }
}
