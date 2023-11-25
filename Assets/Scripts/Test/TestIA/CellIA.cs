using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellIA
{
    [SerializeField]
    private GridIA grid;

    [SerializeField]
    public float x;

    [SerializeField]
    public float z;

    public float gCost;
    public float fCost;
    public float hCost;
    public CellIA parent;
    public Vector3 positionWorld;

    private float sizeCell = 0.1f;
    public bool isWater;
    public CellType cellType = CellType.None;

    public CellIA(GridIA grid, int x, int z, bool isWater, bool isOcuppied, CellType cellType)    //works as a "constructor"
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        this.isWater = isWater;
        this.cellType = cellType;
        this.positionWorld = new Vector3(x * sizeCell, 0, z * sizeCell);
        this.fCost = GetCostOfEnteringCell();
    }

    public float GetCostOfEnteringCell()
    {
        if (isWater)
        {
            return 100f;
        }
        else
        {
            return 1f;
        }
    }

    public string GetPosition()
    {
        return x.ToString() + "," + z.ToString();
    }

    public bool GetWakableAdjacentCellsLogic()
    {
        return true;
    }

    public Vector3 GetWorldPosition()
    {
        return positionWorld;
    }

    public void ChangeOcuppation(CellType type)
    {
        cellType = type;
    }
}