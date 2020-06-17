using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUploader : MonoBehaviour, IUploader<CellData[,]>
{
    public CellData[,] Updload()
    {
        var gridSize = GridManager.Instance.SIZE;
        var cellSize = GridManager.Instance.CELL_SIZE;

        var grid = new CellData[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
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
                    SetCellAsWall(grid, startPos + new Vector3((i + 0.5f) * cellSize, (j + 0.5f) * cellSize, 0), gridSize);
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
                    SetSellAsObject(grid, startPos + new Vector3((i + 0.5f) * cellSize, (j + 0.5f) * cellSize, 0), gridSize);
                }
            }
            Destroy(activeObject);
        }

        return grid;
    }

    private void SetSellAsObject(CellData[,] grid, Vector3 worldPosition, int gridSize)
    {
        GridManager.Instance.GetCellCoords(worldPosition, out int x, out int y);

        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            grid[x, y].isWalkable = false;
            grid[x, y].isActionable = true;
        }
    }

    private void SetCellAsWall(CellData[,] grid, Vector3 worldPosition, int gridSize)
    {
        GridManager.Instance.GetCellCoords(worldPosition, out int x, out int y);

        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            grid[x, y].isWalkable = false;
            grid[x, y].isActionable = false;
        }
    }
}
