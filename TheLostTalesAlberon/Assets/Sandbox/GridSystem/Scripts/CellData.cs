using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellData
{
    public int x { get; }
    public int y { get; }

    public bool isWalkable = true;

    public int gCost;
    public int hCost;
    public int fCost;

    public CellData cameFromCell;

    public CellData(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
