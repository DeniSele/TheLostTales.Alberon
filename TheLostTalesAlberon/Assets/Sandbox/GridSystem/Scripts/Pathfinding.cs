using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private GridManager gridManager;

    private SortedList<int, CellData> openList;
    private List<CellData> closedList;

    public Pathfinding(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public List<CellData> FindPath(int startX, int startY, int finishX, int finishY)
    {
        CellData startCell = GetCell(startX, startY);
        CellData finishCell = GetCell(finishX, finishY);
        if (startCell == null || finishCell == null)
            return null;

        openList = new SortedList<int, CellData>(new DuplicateKeyComparer<int>())
        {
            { startCell.fCost, startCell }
        };

        closedList = new List<CellData>();

        for (int i = 0; i < gridManager.SIZE; i++)
        {
            for (int j = 0; j < gridManager.SIZE; j++)
            {
                CellData cellData = GetCell(i, j);
                cellData.gCost = int.MaxValue;
                cellData.CalculateFCost();
                cellData.cameFromCell = null;
            }
        }

        startCell.gCost = 0;
        startCell.hCost = CalculateDistance(startCell, finishCell);
        startCell.CalculateFCost();

        while (openList.Count > 0)
        {
            CellData curCell = GetLowestFCostCell(openList);
            if (curCell == finishCell)
            {
                return CalculatePath(finishCell);
            }

            openList.RemoveAt(0);
            closedList.Add(curCell);

            foreach (CellData cell in GetNeighborList(curCell))
            {
                if (closedList.Contains(cell)) continue;
                if (!cell.isWalkable)
                {
                    closedList.Add(cell);
                    continue;
                }

                int tentativeCost = curCell.gCost + CalculateDistance(curCell, cell);
                if (tentativeCost < cell.gCost)
                {
                    cell.cameFromCell = curCell;
                    cell.gCost = tentativeCost;
                    cell.hCost = CalculateDistance(cell, finishCell);
                    cell.CalculateFCost();

                    if (!openList.ContainsValue(cell))
                    {
                        openList.Add(cell.fCost, cell);
                    }
                }
            }
        }

        return null;
    }

    // TODO: store the list of neighbors for each cell (save at the initialization stage)
    // Although it doesn't load the system properly, is it worth it?
    private List<CellData> GetNeighborList(CellData curCell)
    {
        List<CellData> neighborList = new List<CellData>();

        if (curCell.x - 1 >= 0)
        {
            neighborList.Add(GetCell(curCell.x - 1, curCell.y));
            if (curCell.y - 1 >= 0) neighborList.Add(GetCell(curCell.x - 1, curCell.y - 1));
            if (curCell.y + 1 < gridManager.SIZE) neighborList.Add(GetCell(curCell.x - 1, curCell.y + 1));
        }
        if (curCell.x + 1 < gridManager.SIZE)
        {
            neighborList.Add(GetCell(curCell.x + 1, curCell.y));
            if (curCell.y - 1 >= 0) neighborList.Add(GetCell(curCell.x + 1, curCell.y - 1));
            if (curCell.y + 1 < gridManager.SIZE) neighborList.Add(GetCell(curCell.x + 1, curCell.y + 1));
        }
        if (curCell.y - 1 >= 0) neighborList.Add(GetCell(curCell.x, curCell.y - 1));
        if (curCell.y + 1 >= 0) neighborList.Add(GetCell(curCell.x, curCell.y + 1));

        return neighborList;
    }

    private CellData GetCell(int x, int y)
    {
        return gridManager.GetCellData(x, y);
    }

    private List<CellData> CalculatePath(CellData finishCell)
    {
        List<CellData> path = new List<CellData>
        {
            finishCell
        };

        CellData curCell = finishCell;
        while (curCell.cameFromCell != null)
        {
            path.Add(curCell.cameFromCell);
            curCell = curCell.cameFromCell;
        }

        path.Reverse();
        return path;
    }

    private CellData GetLowestFCostCell(SortedList<int, CellData> cells)
    {
        return cells.Values[0];
    }

    private int CalculateDistance(CellData a, CellData b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDist - yDist);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_STRAIGHT_COST * remaining;
    }
}
