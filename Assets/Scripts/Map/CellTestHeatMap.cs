using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTestHeatMap : MonoBehaviour
{
    [SerializeField]
    private GridTestHeatMapAgricultureAndPlantation grid;

    [SerializeField]
    public int x;

    [SerializeField]
    public int z;
    public bool isWater;
    public Material materialColor;
    public CellType cellType = CellType.None;

    public CellTestHeatMap(GridTestHeatMapAgricultureAndPlantation grid, int x, int z, bool isWater, bool isOcuppied, CellType cellType)    //works as a "constructor"
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        this.isWater = isWater;
        this.cellType = cellType;
    }

    public void Start()
    {
    }

    public void Update()
    {
    }

    public void ChangeMaterial()
    {
        if (isWater)
        {
            materialColor = (Material)Instantiate(Resources.Load("BlueM"));
            this.gameObject.GetComponent<Renderer>().material = materialColor;
        }
        else if (!isWater)
        {
            materialColor = (Material)Instantiate(Resources.Load("GreenM"));
            this.gameObject.GetComponent<Renderer>().material = materialColor;
        }
    }   //not used

    public void ChangeOcuppation(CellType type)
    {
        cellType = type;
    }
}