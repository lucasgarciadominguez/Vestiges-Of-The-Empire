using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class GridIA : MonoBehaviour
{
    private int size;

    private int tilesForEachCell = 10;
    public CellIA[,] gridArray;  //array bidimensional de casillas
    public GridTestHeatMapAgricultureAndPlantation gridWorldGenerator;  //array bidimensional de casillas

    private float distanceTile = 0.1f;

    public List<Vector3> AdjacentCells;
    public List<Vector3> Path;
    public List<CellIA> Path2;
    public GameObject pathObject;

    // Start is called before the first frame update
    private void Start()
    {
        this.size = gridWorldGenerator.size * tilesForEachCell;
        this.gridArray = new CellIA[this.size, this.size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                if (gridWorldGenerator.gridArray[(int)(x * distanceTile), (int)(z * distanceTile)].isWater)
                {
                    bool isWater = true;
                    CellIA cell = new CellIA(this, x, z, isWater, false, CellType.Empty);
                    gridArray[x, z] = cell;
                }
                else
                {
                    bool isWater = false;
                    CellIA cell = new CellIA(this, x, z, isWater, false, CellType.Empty);
                    gridArray[x, z] = cell;
                }
            }
        }
        pathObject=GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    public List<Vector3> GetPathBetween(Vector3 startPosition, Vector3 actualPosition)
    {
        var resultPath = GridSearchIA.AStarSearch(this, startPosition, actualPosition);
        return resultPath;
        //makes us a list of vector3 representing a path between start and end in the grid
    }

    public List<CellIA> GetPathBetween2(Vector3 startPosition, Vector3 actualPosition)
    {
        var resultPath = GridSearchIA.FindPath(this, startPosition, actualPosition);
        return resultPath;
        //makes us a list of vector3 representing a path between start and end in the grid
    }

    public bool CheckIfItsOcuppiedTheCell(Cell c)  //if it's not ocuppied the position, allows the players to instantiate a construction
    {
        if (c.cellType == CellType.Empty)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        Vector3 result = position;   // you give the vector3 to the original position

        result.x = (float)Math.Round((decimal)result.x, 1);
        result.z = (float)Math.Round((decimal)result.z, 1);
        decimal x = (decimal)result.x;
        decimal z = (decimal)result.z;
        decimal amo = 0.2M;
        if (x % amo == 0)
        {
            if (z % amo == 0)
            {
                return result;
            }
            else
            {
                result.z = (result.z + 0.1f) / 4 * 4;
                return result;
            }
        }
        else
        {
            if (z % amo != 0)
            {
                result.x = (result.x + 0.1f) / 4 * 4;
                result.z = (result.z + 0.1f) / 4 * 4;
                return result;
            }
            else
            {
                result.x = (result.x + 0.1f) / 4 * 4;

                return result;
            }
        }
    }

    public int CheckIfWaterIsNext(Vector3 positionForTheObject)  //if it's not ocuppied the position, allows the players to instantiate a construction
    {
        string positionresult = positionForTheObject.x.ToString() + "," + positionForTheObject.z.ToString();    //convert the position to string to compare it
        int countWater = 0;
        int x = (int)positionForTheObject.x;
        int z = (int)positionForTheObject.z;

        foreach (CellIA c in gridArray)
        {
            if (positionresult == c.GetPosition())    //look if is the same position as one cell and see if it's a water tile
            {
                if (gridArray[x - 1, z].isWater)
                {
                    countWater++;
                }
                if (gridArray[x + 1, z].isWater)
                {
                    countWater++;
                }
                if (gridArray[x, z - 1].isWater)
                {
                    countWater++;
                }
                if (gridArray[x, z + 1].isWater)
                {
                    countWater++;
                }
                else
                {
                }
            }
        }
        return countWater;
    }

    //[left, up, right,down]
    //[ 0 , 1 , 2 , 3 ]
    public CellType[] GetAllAdjacentCellTypes(int x, int z)
    {
        CellType[] neighbours = { CellType.None, CellType.None, CellType.None, CellType.None };
        if (x > 0)
        {
            neighbours[0] = gridArray[x - 1, z].cellType;   //left
        }
        if (x < size - 1)
        {
            neighbours[2] = gridArray[x + 1, z].cellType;   //right
        }
        if (z > 0)
        {
            neighbours[3] = gridArray[x, z - 1].cellType;   //down
        }
        if (z < size - 1)
        {
            neighbours[1] = gridArray[x, z + 1].cellType;   //up
        }
        return neighbours;
    }

    //[left, up, right,down]
    //[ 0 , 1 , 2 , 3 ]

    public bool CheckIfOneAdjacentCellIsOfType(int x, int z, CellType type)
    {
        List<CellType> neighbours = new List<CellType>();
        if (x > 0)
        {
            neighbours.Add(gridArray[x - 1, z].cellType);   //left
        }
        if (x < size - 1)
        {
            neighbours.Add(gridArray[x + 1, z].cellType);   //right
        }
        if (z > 0)
        {
            neighbours.Add(gridArray[x, z - 1].cellType);   //down
        }
        if (z < size - 1)
        {
            neighbours.Add(gridArray[x, z + 1].cellType);   //up
        }
        bool isOneExistent = neighbours.Exists(x => x == type);
        return isOneExistent;
    }

    public List<Vector3> GetAdjacentCellsOfType(Vector3 vector, CellType type)
    {
        List<Vector3> adjacentCells = GetAllAdjacentCells(vector);

        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (gridArray[Convert.ToInt32(adjacentCells[i].x), Convert.ToInt32(adjacentCells[i].z)].cellType != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        AdjacentCells = adjacentCells;
        return adjacentCells;
    }

    public List<Vector3> GetAllAdjacentCells(Vector3 vector3)
    {
        List<Vector3> cells = new List<Vector3>();
        if (vector3.x > 0 && vector3.z < size - distanceTile)
        {
            //left top corner
            cells.Add(new Vector3(vector3.x - distanceTile, 0, vector3.z + distanceTile));
        }
        if (vector3.x > 0)
        {
            //left
            cells.Add(new Vector3(vector3.x - distanceTile, 0, vector3.z));
        }
        if (vector3.x > 0 && vector3.z > 0)
        {
            //left bottom corner
            cells.Add(new Vector3(vector3.x - distanceTile, 0, vector3.z - distanceTile));
        }
        if (vector3.x < size - distanceTile && vector3.z > 0)
        {
            //right top corner
            cells.Add(new Vector3(vector3.x + distanceTile, 0, vector3.z + distanceTile));
        }
        if (vector3.x < size - distanceTile)
        {
            //right
            cells.Add(new Vector3(vector3.x + distanceTile, 0, vector3.z));
        }
        if (vector3.x < size - distanceTile && vector3.z > 0)
        {
            //right bottom corner
            cells.Add(new Vector3(vector3.x + distanceTile, 0, vector3.z - distanceTile));
        }
        if (vector3.z > 0)
        {
            //down
            cells.Add(new Vector3(vector3.x, 0, vector3.z - distanceTile));
        }
        if (vector3.z < size - distanceTile)
        {
            //up
            cells.Add(new Vector3(vector3.x, 0, vector3.z + distanceTile));
        }
        return cells;
    }

    public List<CellIA> GetAdjacentCellsLogic(CellIA vector3)
    {
        List<CellIA> cells = new List<CellIA>();
        if (vector3.x > 0 && vector3.z < size - distanceTile)
        {
            //left top corner
            cells.Add(gridArray[(int)vector3.x - 1, (int)vector3.z + 1]);
        }
        if (vector3.x > 0)
        {
            //left
            cells.Add(gridArray[(int)vector3.x - 1, (int)vector3.z]);
        }
        if (vector3.x > 0 && vector3.z > 0)
        {
            //left bottom corner
            cells.Add(gridArray[(int)vector3.x - 1, (int)vector3.z - 1]);
        }
        if (vector3.x < size - distanceTile && vector3.z > 0)
        {
            //right top corner
            cells.Add(gridArray[(int)vector3.x + 1, (int)vector3.z + 1]);
        }
        if (vector3.x < size - distanceTile)
        {
            //right
            cells.Add(gridArray[(int)vector3.x + 1, (int)vector3.z]);
        }
        if (vector3.x < size - distanceTile && vector3.z > 0)
        {
            //right bottom corner
            cells.Add(gridArray[(int)vector3.x + 1, (int)vector3.z - 1]);
        }
        if (vector3.z > 0)
        {
            //down
            cells.Add(gridArray[(int)vector3.x, (int)vector3.z - 1]);
        }
        if (vector3.z < size - distanceTile)
        {
            //up
            cells.Add(gridArray[(int)vector3.x, (int)vector3.z + 1]);
        }
        return cells;
    }

    public bool CheckIfCanBePlacedStructure2(Vector3 vector)
    {
        int vectorX = Convert.ToInt32(vector.x);
        int vectorZ = Convert.ToInt32(vector.z);
        if (gridArray[vectorX + 1, vectorZ].cellType == CellType.Empty)
        {
            Debug.Log("Empty for structure2");
            return true;
        }
        else
        {
            Debug.Log("Not Empty for structure2");

            return false;
        }
    }

    public bool CheckIfCanBePlacedStructure2x2(Vector3 vector)
    {
        int vectorX = Convert.ToInt32(vector.x);
        int vectorZ = Convert.ToInt32(vector.z);
        if (gridArray[vectorX + 1, vectorZ].cellType == CellType.Empty && gridArray[vectorX, vectorZ - 1].cellType == CellType.Empty && gridArray[vectorX + 1, vectorZ - 1].cellType == CellType.Empty)
        {
            Debug.Log("Empty for 2x2");
            return true;
        }
        else
        {
            Debug.Log("Not Empty for 2x2");

            return false;
        }
    }

    public List<Vector3> GetWakableAdjacentCells(Vector3 position, bool isAgent)
    {
        List<Vector3> adjacentCells = GetAllAdjacentCells(position);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (IsCellWakable(gridArray[Convert.ToInt32(adjacentCells[i].x), Convert.ToInt32(adjacentCells[i].z)].cellType, isAgent) == false)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    public float GetCostOfEnteringCell(Vector3 cellWorldPosition)
    {
        //Debug.Log(gridArray[(int)(cell.x * distanceTile), (int)(cell.z * distanceTile)].cellType);
        //switch (gridArray[(int)(cell.x * distanceTile), (int)(cell.z * distanceTile)].cellType)
        //{
        //    case CellType.Empty:
        //        if (gridArray[(int)(cell.x * distanceTile), (int)(cell.z * distanceTile)].isWater)
        //            return 100f;
        //        else
        //            return 0.5f;

        //    case CellType.Structure:
        //        return 1.3f;

        //    default:
        //        break;
        //}
        CellIA cellAs = ReturnCellFromVector(cellWorldPosition, gridArray);
        if (cellAs == null)
        {
            return 1f;
        }
        else
        {
            if (cellAs.isWater == true)
                return 100f;
        }
        return 1f;
    }

    public CellIA ReturnCellFromVector(Vector3 cellPosition, CellIA[,] gridArray)
    {
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                if (gridArray[x, z].positionWorld == cellPosition)
                {
                    return gridArray[x, z];
                }
                else
                {
                }
            }
        }
        //foreach (CellIA item in gridArray)
        //{
        //    if (item.positionWorld== cellPosition)
        //    {
        //        Debug.Log(item.positionWorld+" aasa "+cellPosition);
        //        return item;
        //    }
        //    else
        //    {
        //    }
        //}
        return null;
    }

    public List<Vector3> GetAdjacentCells(Vector3 cell, bool isAgent)
    {
        return GetWakableAdjacentCells(cell, isAgent);
    }

    public static bool IsCellWakable(CellType cellType, bool aiAgent = false)
    {
        if (aiAgent)
        {
            return cellType == CellType.Road;
        }
        return cellType == CellType.Empty || cellType == CellType.Road || cellType == CellType.StructureInsula;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;

    //    for (int z = 0; z < size; z++)
    //    {
    //        for (int x = 0; x < size; x++)
    //        {
    //            Gizmos.DrawSphere(new Vector3( x*0.1f, 0, z*0.1f), 0.1f);
    //        }

    //    }
    //}
}