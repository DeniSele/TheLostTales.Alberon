using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public bool DEBUG_MODE;

    public int SIZE => 128;

    public int CELL_SIZE => 1;

    public static GridManager Instance { get; private set; }

    private CellData[,] grid;
    private Pathfinding pathfinding;

    private IDisplayable<List<CellData>> gridDisplayer;
    private IUploader<CellData[,]> gridUploader;

    private void InitializeGrid()
    {
        Instance = this;

        pathfinding = new Pathfinding(SIZE);

        gridDisplayer = GetComponent<IDisplayable<List<CellData>>>();

        gridUploader = GetComponent<IUploader<CellData[,]>>();
        grid = gridUploader.Updload();
    }

    private void Awake()
    {
        InitializeGrid();
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
                gridDisplayer.Display(path);

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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 1f);
        Gizmos.DrawWireCube(new Vector3(SIZE/2, SIZE/2, 0), new Vector3(SIZE, SIZE, 1));

        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Gizmos.DrawCube(new Vector3(SIZE/2, SIZE/2, 0), new Vector3(SIZE, SIZE, 1));
    }
}
