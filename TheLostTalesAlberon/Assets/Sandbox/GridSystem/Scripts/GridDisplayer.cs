using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDisplayer : MonoBehaviour, IDisplayable<List<CellData>>
{
    public GameObject CellPrefab;

    private GameObject[,] cells;
    private List<CellData> prevPath = null;

    private bool initialized;

    public void Initialize()
    {
        int SIZE = GridManager.Instance.SIZE;
        int CELL_SIZE = GridManager.Instance.CELL_SIZE;

        cells = new GameObject[SIZE, SIZE];

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                cells[i, j] = Instantiate<GameObject>(CellPrefab, new Vector3(CELL_SIZE * (i + CELL_SIZE * 0.5f), CELL_SIZE * (j + CELL_SIZE * 0.5f), 0), new Quaternion());
                cells[i, j].transform.parent = gameObject.transform;

                CellData cellData = GridManager.Instance.GetCellData(i, j);

                if (!cellData.isWalkable)
                    cells[i, j].GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);

                if (cellData.isActionable)
                    cells[i, j].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 1f);
            }
        }

        initialized = true;
    }

    public void Start()
    {
        Initialize();
    }

    public void Display(List<CellData> path)
    {
        if (initialized == false)
            Initialize();

        if (path == null)
            return;

        if (prevPath != null)
            ClearPrevPathCells();

        foreach (CellData cell in path)
        {
            SelectCell(cell, new Color(0f, 0f, 1f));
        }

        prevPath = path;
    }

    private void ClearPrevPathCells()
    {
        foreach (CellData cell in prevPath)
        {
            if (cell.isActionable)
            {
                SelectCell(cell, new Color(1f, 1f, 0f, 1f));
            }
            else
            {
                SelectCell(cell, new Color(1f, 1f, 1f, 0.5f));
            }
        }
    }

    private void SelectCell(CellData cell, Color color)
    {
        cells[cell.X, cell.Y].GetComponent<SpriteRenderer>().color = color;
    }
}
