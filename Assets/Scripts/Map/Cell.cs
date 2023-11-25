using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LUCAS GARCIA DOMINGUEZ
/// </summary>
public enum CellType
{
    Empty,
    PivotRoad,
    Road,
    HouseConnection,
    StructureInsula,
    StructureMidClass,
    StructureUpperClass,
    Mine,
    LumberCamp,
    Pottery,
    Farm,
    Plantation,
    Bakery,
    BlackSmith,
    TailorShop,
    Temple,
    Sanctuary,
    None
}
[Serializable]
public class Cell 
{
    [SerializeField]
    private WorldGenerator grid;
    public bool isCorner { private get; set; }=false;

    [SerializeField]
    public int x;

    [SerializeField]
    public int z;
    [SerializeField]
    public Dictionary<ExtractiveResources, int> valueHeatMaps=new Dictionary<ExtractiveResources, int>();

    public bool isWater;
    public Material materialColor;
    public CellType cellType = CellType.None;
    public float gCost;
    public float fCost=1;
    public float hCost;
    public Cell parent;
    public bool isWalkableForRiver { private get; set; }=false;
    public Cell(WorldGenerator grid, int x, int z, bool isWater, bool isOcuppied, CellType cellType)    //works as a "constructor"
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        this.isWater = isWater;
        this.cellType = cellType;
    }


    public string getPosition()
    {
        return x.ToString() + "," + z.ToString();
    }
    public void ChangeValueHeatMap(ExtractiveResources resources,int value)
    {
        valueHeatMaps.Add(resources,value);
    }
    public int ReturnValueHeatMap(ExtractiveResources resourcesNeeded)
    {
        if (resourcesNeeded==ExtractiveResources.None)
        {
            return 0;
        }
       return valueHeatMaps[resourcesNeeded];
    }
    public bool ReturnIsCorner()
    { return isCorner; }
    public void ChangeOcuppation(CellType type)
    {
        cellType = type;
    }
    public bool GetWakableAdjacentCellsLogic()
    {
        return true;
    }
    public bool ReturnIsWalkable()
    {
        return isWalkableForRiver;
    }
}