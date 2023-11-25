using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class RoadFixer : MonoBehaviour
{
    public GameObject roadStraight, deadEnd, corner, threeWay, fourWay, bridgeRamp, bridgeWater;

    public void FixRoadAtPosititon(RoadManager roadmanager, Vector3 temporaryPosition)
    {
        //[right,up,left,down]
        CellType[] result = roadmanager.builder.GetNeighbourtTypesFor(temporaryPosition);
        int resultWater = roadmanager.builder.GetNeighbourtTypesForWithWater(temporaryPosition);
        int roadCount = 0;
        roadCount = result.Where(x => x == CellType.PivotRoad).Count();
        if (roadmanager.builder.isThisNotCellWater(temporaryPosition))
        {
            if (roadCount == 0 || roadCount == 1)
            {
                if (roadCount == 1)
                {
                    CreateDeadEnd(roadmanager, result, temporaryPosition);
                }
                else
                {
                    CreateDeadEnd(roadmanager, result, temporaryPosition);
                }
            }
            else if (roadCount == 2)
            {
                if (CreateStraightRoad(roadmanager, result, temporaryPosition))
                    return;
                CreateCorner(roadmanager, result, temporaryPosition);
            }
            else if (roadCount == 3)
            {
                Create3Way(roadmanager, result, temporaryPosition);
            }
            else if (roadCount == 4)
            {
                Create4Way(roadmanager, result, temporaryPosition);
            }
        }
        else
        {
            if (roadCount == 1 && resultWater != 0)
            {
                if (resultWater == 4)
                {
                    CreateWaterBridge(roadmanager, result, temporaryPosition);
                }
                else
                {
                    CreateWaterRamp(roadmanager, result, temporaryPosition);
                }
            }
            else if (roadCount == 2)
            {
                if (resultWater != 0)
                {
                    if (resultWater == 4)
                    {
                        CreateWaterBridge(roadmanager, result, temporaryPosition);
                    }
                    else
                    {
                        CreateWaterRamp(roadmanager, result, temporaryPosition);
                    }
                }
            }
        }
    }

    //[left, up, right,down]
    private void CreateWaterBridge(RoadManager placeBuilding, CellType[] result, Vector3 temporaryPosition)
    {
        if (!placeBuilding.builder.isThisNotCellWater(new Vector3(temporaryPosition.x, 0, temporaryPosition.z - 1)) && !placeBuilding.builder.isThisNotCellWater(new Vector3(temporaryPosition.x, 0, temporaryPosition.z + 1)))
        {
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.Bridge, Quaternion.Euler(0, 90, 0));
        }
        else if (result[2] == CellType.Road)
        {
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.Bridge, Quaternion.Euler(0, 90, 0));
        }
        else if (result[3] == CellType.Road)
        {
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.Bridge, Quaternion.Euler(0, 180, 0));
        }
        else if (result[0] == CellType.Road)
        {
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.Bridge, Quaternion.Euler(0, 270, 0));
        }
    }

    //[left, up, right,down]
    private void CreateWaterRamp(RoadManager roadManager, CellType[] result, Vector3 temporaryPosition)
    {
        //if (result[1] == CellType.PivotRoad && !roadManager.builder.isThisNotCellWater(new Vector3(temporaryPosition.x, 0, temporaryPosition.z - 1)))
        //{
        //    roadManager.ModifyStructureModel(temporaryPosition, StructureType.Bridge, Quaternion.identity);
        //}
        if (result[2] == CellType.PivotRoad && !roadManager.builder.isThisNotCellWater(new Vector3(temporaryPosition.x - 1, 0, temporaryPosition.z)))
        {
            Debug.Log(temporaryPosition.x + "," + temporaryPosition.z);
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.Bridge, Quaternion.Euler(0, -90, 0));
        }
        //else if (result[3] == CellType.PivotRoad && !roadManager.builder.isThisNotCellWater(new Vector3(temporaryPosition.x, 0, temporaryPosition.z + 1)))
        //{
        //    roadManager.ModifyStructureModel(temporaryPosition, StructureType.Bridge, Quaternion.Euler(0, 180, 0));
        //}
        else if (result[0] == CellType.PivotRoad && !roadManager.builder.isThisNotCellWater(new Vector3(temporaryPosition.x + 1, 0, temporaryPosition.z)))
        {
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.Bridge, Quaternion.Euler(0, -90, 0));
        }
    }

    private void Create4Way(RoadManager roadManager, CellType[] result, Vector3 temporaryPosition)
    {
        roadManager.ModifyStructureModel(temporaryPosition, StructureType.FourWay, Quaternion.identity);
    }

    //[left, up, right,down]
    private void Create3Way(RoadManager roadManager, CellType[] result, Vector3 temporaryPosition)
    {
        if (result[1] == CellType.PivotRoad && result[2] == CellType.PivotRoad && result[3] == CellType.PivotRoad)
        {
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.ThreeWay, Quaternion.identity);
            //roadManager.ModifyStructureModel(temporaryPosition, threeWayRight, Quaternion.identity);
        }
        else if (result[0] == CellType.PivotRoad && result[2] == CellType.PivotRoad && result[3] == CellType.PivotRoad)
        {
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.ThreeWay, Quaternion.Euler(0, 90, 0));
            //roadManager.ModifyStructureModel(temporaryPosition, threeWayDown, Quaternion.identity);
        }
        else if (result[0] == CellType.PivotRoad && result[1] == CellType.PivotRoad && result[3] == CellType.PivotRoad)
        {
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.ThreeWay, Quaternion.Euler(0, 180, 0));
            // roadManager.ModifyStructureModel(temporaryPosition, threeWayLeft, Quaternion.identity);
        }
        else if (result[0] == CellType.PivotRoad && result[1] == CellType.PivotRoad && result[2] == CellType.PivotRoad)
        {
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.ThreeWay, Quaternion.Euler(0, 270, 0));
            // roadManager.ModifyStructureModel(temporaryPosition, threeWayUp, Quaternion.identity);
        }
    }

    //[left, up, right,down]
    private void CreateCorner(RoadManager roadManager, CellType[] result, Vector3 temporaryPosition)
    {
        if (result[1] == CellType.PivotRoad && result[2] == CellType.PivotRoad)
        {
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.Corner, Quaternion.Euler(0, 90, 0));
            //roadManager.ModifyStructureModel(temporaryPosition, cornerVariationDownRight, Quaternion.identity);
        }
        else if (result[2] == CellType.PivotRoad && result[3] == CellType.PivotRoad)
        {
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.Corner, Quaternion.Euler(0, 180, 0));
            //roadManager.ModifyStructureModel(temporaryPosition, cornerVariationTopRight, Quaternion.identity);
        }
        else if (result[0] == CellType.PivotRoad && result[3] == CellType.PivotRoad)
        {
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.Corner, Quaternion.Euler(0, 270, 0));
            //roadManager.ModifyStructureModel(temporaryPosition, cornerVariationTopLeft, Quaternion.identity);
        }
        else if (result[0] == CellType.PivotRoad && result[1] == CellType.PivotRoad)
        {
            roadManager.ModifyStructureModel(temporaryPosition, StructureType.Corner, Quaternion.Euler(0, 0, 0));
            //roadManager.ModifyStructureModel(temporaryPosition, cornerVariationDownLeft, Quaternion.identity);
        }
    }

    //[left, up, right,down]
    private bool CreateStraightRoad(RoadManager placeBuilding, CellType[] result, Vector3 temporaryPosition)
    {
        if (result[0] == CellType.PivotRoad && result[2] == CellType.PivotRoad)
        {
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.DeadEnd, Quaternion.Euler(0, 90, 0));

            // placeBuilding.ModifyStructureModel(temporaryPosition, roadStraightLeftRight, Quaternion.Euler(0, 90, 0));
            return true;
        }
        else if (result[1] == CellType.PivotRoad && result[3] == CellType.PivotRoad)
        {
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.DeadEnd, Quaternion.identity);

            //placeBuilding.ModifyStructureModel(temporaryPosition, roadStraightTopDown, Quaternion.identity);

            return true;
        }
        else
        {
            return false;   //its a corner
        }
    }

    //[left, up, right,down]
    private void CreateDeadEnd(RoadManager placeBuilding, CellType[] result, Vector3 temporaryPosition)
    {
        if (result[1] == CellType.PivotRoad)
        {
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.DeadEnd, Quaternion.identity);
            //placeBuilding.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.identity);

            // placeBuilding.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 270, 0));
        }
        else if (result[2] == CellType.PivotRoad)
        {
            //placeBuilding.ModifyStructureModel(temporaryPosition, deadEndRight, Quaternion.Euler(0, 90, 0));
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.DeadEnd, Quaternion.Euler(0, 90, 0));
        }
        else if (result[3] == CellType.PivotRoad)
        {
            //placeBuilding.ModifyStructureModel(temporaryPosition, deadEndTop, Quaternion.identity);
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.DeadEnd, Quaternion.identity);
        }
        else if (result[0] == CellType.PivotRoad)
        {
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.DeadEnd, Quaternion.Euler(0, 90, 0));
            //placeBuilding.ModifyStructureModel(temporaryPosition, deadEndRight, Quaternion.Euler(0, 90, 0));
            //placeBuilding.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 90, 0));
        }
        else
        {
            placeBuilding.ModifyStructureModel(temporaryPosition, StructureType.DeadEnd, Quaternion.identity);

        }
    }
}