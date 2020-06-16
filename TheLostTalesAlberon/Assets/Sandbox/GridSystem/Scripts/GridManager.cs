using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public bool DEBUG_MODE;

    public int SIZE { get; } = 128;

    public int CELL_SIZE { get; } = 1;

    public static GridManager Instance { get; private set; }

    private CellData[,] grid;
    private Pathfinding pathfinding;

    private GridDisplayer gridDisplayer;

    private void InitGrid()
    {
        Instance = this;

        grid = new CellData[SIZE, SIZE];

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                grid[i, j] = new CellData(i, j);
            }
        }

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            Vector3 startPos = wall.transform.position - wall.transform.localScale * 0.5f;
            Vector3 scale = wall.transform.localScale;
            for (int i = 0; i < scale.x; i++)
            {
                for (int j = 0; j < scale.y; j++)
                {
                    SetCellAsWall(startPos + new Vector3((i + 0.5f) * CELL_SIZE, (j + 0.5f) * CELL_SIZE, 0));
                }
            }
            Destroy(wall);
        }

        GameObject[] actionObjects = GameObject.FindGameObjectsWithTag("ActionObject");
        foreach (GameObject activeObject in actionObjects)
        {
            Vector3 startPos = activeObject.transform.position - activeObject.transform.localScale * 0.5f;
            Vector3 scale = activeObject.transform.localScale;
            for (int i = 0; i < scale.x; i++)
            {
                for (int j = 0; j < scale.y; j++)
                {
                    SetSellAsObject(startPos + new Vector3((i + 0.5f) * CELL_SIZE, (j + 0.5f) * CELL_SIZE, 0));
                }
            }
            Destroy(activeObject);
        }

        if (DEBUG_MODE)
        {
            gridDisplayer = GetComponent<GridDisplayer>();
            gridDisplayer.Init(this);
        }

        pathfinding = new Pathfinding(SIZE);
    }

    private void Awake()
    {
        InitGrid();
    }

    public List<CellData> GetPathFromTo(Vector3 startPosition, Vector3 finishPosition)
    {
        GetCellCoords(startPosition, out int startX, out int startY);
        GetCellCoords(finishPosition, out int finishX, out int finishY);

        if ((startX >= 0 && startX < SIZE && startY >= 0 && startY < SIZE) &&
            (finishX >= 0 && finishX < SIZE && finishY >= 0 && finishY < SIZE))
        {
            List<CellData> path = pathfinding.FindPath(startX, startY, finishX, finishY);

            if (DEBUG_MODE)
                gridDisplayer.DrawPath(path);

            return path;
        }

        return null;
    }

    public CellData GetCellData(int x, int y)
    {
        if (x >= 0 && x < SIZE && y >= 0 && y < SIZE)
            return grid[x, y];

        return null;
    }

    public void GetCellCoords(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x);
        y = Mathf.FloorToInt(worldPosition.y);
    }

    private void SetSellAsObject(Vector3 worldPosition)
    {
        GetCellCoords(worldPosition, out int x, out int y);

        if (x >= 0 && x < SIZE && y >= 0 && y < SIZE)
        {
            grid[x, y].isWalkable = false;
            grid[x, y].isActionable = true;
        }
    }

    private void SetCellAsWall(Vector3 worldPosition)
    {
        GetCellCoords(worldPosition, out int x, out int y);

        if (x >= 0 && x < SIZE && y >= 0 && y < SIZE)
        {
            grid[x, y].isWalkable = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 1f);
        Gizmos.DrawWireCube(new Vector3(SIZE/2, SIZE/2, 0), new Vector3(SIZE, SIZE, 1));

        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Gizmos.DrawCube(new Vector3(SIZE/2, SIZE/2, 0), new Vector3(SIZE, SIZE, 1));
    }
}
