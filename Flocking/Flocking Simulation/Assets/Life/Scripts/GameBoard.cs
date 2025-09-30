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

    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;

    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;

    private Vector2 cellSize;

    private bool setupMode = true;
    private void Awake() //Runs First
    {
        //Instantiating Hash Sets
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();

        cellSize = this.GetComponent<Grid>().cellSize;
        AdjustCamera();
    }
    public void Begin() //Runs When Called
    {
        StartCoroutine(Simulate());
    }
    private void OnDrawGizmos()
    {
        if (cellSize == Vector2.zero) 
        {
            cellSize = GetComponent<Grid>().cellSize;
        }

        Gizmos.color = Color.gray;

        Vector3 origin = new Vector3(
            -(gridWidth * cellSize.x) / 2f,
            -(gridHeight * cellSize.y) / 2f,
            0f
        );

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = origin + new Vector3(x * cellSize.x, 0, 0);
            Vector3 end = origin + new Vector3(x * cellSize.x, gridHeight * cellSize.y, 0);
            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = origin + new Vector3(0, y * cellSize.y, 0);
            Vector3 end = origin + new Vector3(gridWidth * cellSize.x, y * cellSize.y, 0);
            Gizmos.DrawLine(start, end);
        }
    }

    private Vector3Int GridOrigin()
    {
        return new Vector3Int(-gridWidth / 2, -gridHeight / 2, 0);
    }

    private void AdjustCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float gridWorldWidth = gridWidth * cellSize.x;
        float gridWorldHeight = gridHeight * cellSize.y;

        Vector3 center = new Vector3(0, 0, -10f); 
        cam.transform.position = center;

        float aspect = cam.aspect;
        float verticalSize = gridWorldHeight / 2f;
        float horizontalSize = gridWorldWidth / (2f * aspect);

        cam.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && setupMode)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = currentState.WorldToCell(mouseWorld) - GridOrigin();
            cell.x = Mathf.Clamp(cell.x, 0, gridWidth - 1);
            cell.y = Mathf.Clamp(cell.y, 0, gridHeight - 1);


            if (InBounds(cell))
            {
                if (IsAlive(cell))
                {
                    currentState.SetTile(cell + GridOrigin(), deadTile);
                    aliveCells.Remove(cell);
                }
                else
                {
                    currentState.SetTile(cell + GridOrigin(), aliveTile);
                    aliveCells.Add(cell);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            setupMode = !setupMode;

            if (setupMode)
            {
                StopAllCoroutines();
                Clear();
                
            }
            else
            {
                Begin();
            }
        }
    }

    private bool InBounds(Vector3Int cell)
    {
        return cell.x >= 0 && cell.x < gridWidth &&
           cell.y >= 0 && cell.y < gridHeight;
    }

    private Vector3Int Wrap(Vector3Int cell)
    {
        int x = (cell.x + gridWidth) % gridWidth;
        int y = (cell.y + gridHeight) % gridHeight;
        return new Vector3Int(x, y, 0);
    }

    private void Clear()
    {
        currentState.ClearAllTiles();
        newState.ClearAllTiles();
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
        foreach (Vector3Int cell in aliveCells)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    Vector3Int neighbor = cell + new Vector3Int(dx, dy, 0);
                    if (InBounds(neighbor))
                    {
                        cellsToCheck.Add(neighbor);
                    }
                }
            }
        }

        HashSet<Vector3Int> newAlive = new HashSet<Vector3Int>(aliveCells);

        foreach (Vector3Int cell in cellsToCheck)
        {
            int neighbors = CountNeighbors(cell);
            bool alive = aliveCells.Contains(cell);

            if (!alive && neighbors == 3)
            {
                newState.SetTile(cell + GridOrigin(), aliveTile);
                newAlive.Add(cell);
            }
            else if (alive && (neighbors < 2 || neighbors > 3))
            {
                newState.SetTile(cell + GridOrigin(), deadTile);
                newAlive.Remove(cell);
            }
            else if (alive)
            {
                newState.SetTile(cell + GridOrigin(), aliveTile);
            }
            else
            {
                newState.SetTile(cell + GridOrigin(), deadTile);
            }
        }

        aliveCells = newAlive;

        //Swap Tilemaps
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
                if (dx == 0 && dy == 0) continue;

                Vector3Int neighbor = new Vector3Int(cell.x + dx, cell.y + dy, 0);

                if (true) 
                {
                    neighbor = Wrap(neighbor);
                }
                else if (!InBounds(neighbor))
                {
                    continue;
                }

                if (aliveCells.Contains(neighbor))
                {
                    count++;
                }
            }
        }

        return count;
    }


    private bool IsAlive(Vector3Int cell)
    {
        return currentState.GetTile(cell + GridOrigin()) == aliveTile;
    }

}
