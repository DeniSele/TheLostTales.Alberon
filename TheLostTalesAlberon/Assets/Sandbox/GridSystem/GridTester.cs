using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTester : MonoBehaviour
{
    private GridManager gridManager;

    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<CellData> path = gridManager.GetPathFromTo(new Vector3(0, 0, 0), Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
