using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellData
{
    private int x;
    private int y;

    public int X {
        get { return x; }
    }

    public int Y {
        get { return y; }
    }

    public bool isWalkable = true;
    public bool isActionable = false;

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
