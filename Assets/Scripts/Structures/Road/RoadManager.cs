using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlaceBuilding builder { get; private set; }
    private Dictionary<Vector3, StructureModel> temporaryRoadObjects = new Dictionary<Vector3, StructureModel>();
    private Dictionary<Vector3, StructureModel> structureDictionary = new Dictionary<Vector3, StructureModel>();
    private List<Vector3> temporaryPlacementPositions = new List<Vector3>();
    private List<Vector3> roadPositionsToRecheck = new List<Vector3>();
    private Vector3 startPosition;

    [SerializeField]
    private GameObject roadsGO;

    [SerializeField]
    private GameObject roadMultiStructurePrefab;

    private RoadFixer roadFixer;

    [SerializeField]
    private Structures structure;

    [SerializeField]
    private Vector3 actualPosition;

    private void Start()
    {
        roadFixer = GetComponent<RoadFixer>();
        builder = GetComponent<PlaceBuilding>();
    }

    public void PlaceRoad(Vector3 position, ref bool placementMode)
    {
        if (placementMode == false) //on first click, enters this method and sets the first position
        {
            temporaryPlacementPositions.Clear();    //clears the last temporary positions
            roadPositionsToRecheck.Clear();         //clears the roadpositionstorecheck

            placementMode = true;
            startPosition = position;   //sets the start position to the position that we are passing to the method
            temporaryPlacementPositions.Add(position);  //adds the actual position to the temporary list of positions
            PlaceTemporaryStructure(position, roadMultiStructurePrefab, CellType.PivotRoad); //places the first position that sets the start of the road
        }
        else //on drag sets the other positions that composes the road
        {
            //if (position!=actualPosition)
            //{
            RemoveAllTemporaryStructures(); //clears all the temporary objects displayed in the hierarchy
            temporaryPlacementPositions.Clear();    //clears the last temporary positions
            foreach (var positionstoFix in roadPositionsToRecheck)  //fix all the positions to recheck, even the ones who are not being constructed in that moment (the ones who are constructed already)
            {
                roadFixer.FixRoadAtPosititon(this, positionstoFix); //fix the road prefab at that position knowing all his neighbours
            }
            roadPositionsToRecheck.Clear(); //once it is all fixed clears roadPositionsToRecheck

            temporaryPlacementPositions = GetPathBetween(startPosition, position);  //gets the path between the start position and the actual position in the mouse
            foreach (var temporaryPosition in temporaryPlacementPositions)
            {
                if (builder.grid.CheckIfItsOcuppiedTheCell(builder.grid.gridArray[(int)temporaryPosition.x, (int)temporaryPosition.z], builder.cellTypeBuilding))    //if its ocuppied the position who we are pointing with the mouse
                {
                    roadPositionsToRecheck.Add(temporaryPosition);  //adds it to the roadpositionstorecheck, so it can be changed its prefabs and be adapted to the new road structures
                    continue;
                }

                PlaceTemporaryStructure(temporaryPosition, roadMultiStructurePrefab, CellType.PivotRoad);   //adds new temporary structures
            }
            foreach (var temporaryPosition in temporaryPlacementPositions)
            {
                SetPivotRoad(temporaryPosition);
            }
            //}
        }
        actualPosition = position;

        FixRoadPrefabs();   //fixes all the prefabs of all the structures displayed in the hierarchy
    }

    private void SetRoadIndex()
    {
        foreach (var temporaryPosition in temporaryPlacementPositions)
        {
            List<Vector3> listPositionsEmpty = builder.grid.GetAdjacentCellsOfTypeForCorner(temporaryPosition, CellType.Empty);
            foreach (var item in listPositionsEmpty)
            {
                // builder.grid.gridArray[Convert.ToInt32(item.x), Convert.ToInt32(item.z)].ChangeOcuppation(CellType.Road);
            }
        }
    }

    private void SetPivotRoad(Vector3 temporaryPosition)    //sets the positions for the pivotroads
    {
        for (int auxY = 0; auxY <= 1; auxY++)
        {
            for (int auxX = 0; auxX <= 1; auxX++)
            {
                if (auxX == 0 && auxY == 0)
                {
                    builder.grid.gridArray[Convert.ToInt32(temporaryPosition.x), Convert.ToInt32(temporaryPosition.z)].ChangeOcuppation(CellType.PivotRoad);
                }
            }
        }
    }

    private void FixRoadPrefabs()
    {
        foreach (var temporaryPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosititon(this, temporaryPosition);   //fix the road at that position
            var neighbours = builder.GetNeighbourtOfTypeFor(temporaryPosition, CellType.PivotRoad);  //gets the neighbours  for every temporaryposition displayed
            foreach (var roadPosition in neighbours)
            {
                if (roadPositionsToRecheck.Contains(roadPosition) == false)    //if it not contains this position, it adds it inmediatly to the roadPositionsToRecheck
                {
                    roadPositionsToRecheck.Add(roadPosition);
                }
            }
        }
        foreach (var positionsToFix in roadPositionsToRecheck)  //for every position in thhe road positionstorecheck, it fixes its prefab
        {
            roadFixer.FixRoadAtPosititon(this, positionsToFix);
        }
    }

    public void FinishPlacingRoad(ref bool placementMode)
    {
        foreach (var temporaryPosition in temporaryPlacementPositions)  //changes the road connection to the houses
        {
            List<Vector3> listHouses1 = builder.grid.CheckAdjacentCellsOfType((int)temporaryPosition.x, (int)temporaryPosition.z, CellType.HouseConnection);
            foreach (Vector3 house in listHouses1)
            {
                structure.ChangeRoadConnection(house);
            }
            builder.AddNatureForDestroy(temporaryPosition);
            builder.FinishBuilding();
        }
        placementMode = false;  //changes the placement mode for stoping with the drag

        AddTemporaryStructuresToStructureDictionary();
        if (temporaryPlacementPositions.Count > 0)
        {
            //TODO make a sound
        }
        //SetRoadIndex();
        temporaryPlacementPositions.Clear();    //clears all the temporaryPlacement Positions
        startPosition = Vector3.zero;   //sets the positionStartTozero
        builder.DestroyNature();
    }

    private List<Vector3> GetPathBetween(Vector3 startPosition, Vector3 actualPosition)
    {
        //var resultPath = GridSearch.AStarSearch(builder.grid, startPosition, actualPosition);
        var resultPath = GridSearchIA.AStarSearchNormalGrid(builder.grid, startPosition, actualPosition);
        return resultPath;
        //makes us a list of vector3 representing a path between start and end in the grid
    }

    private void RemoveAllTemporaryStructures()
    {
        foreach (var temporaryRoads in temporaryRoadObjects.Values) //removes all the roads placed in the temporary roads values
        {
            var position = Vector3Int.RoundToInt(temporaryRoads.transform.position);
            builder.grid.gridArray[position.x, position.z].cellType = CellType.Empty;   //sets the grid values of celltype to emptys
            Destroy(temporaryRoads.gameObject); //and destroys it
        }
        temporaryRoadObjects.Clear();   //clears the dictionary of references once it destroys all the gameobjects
    }

    private void AddTemporaryStructuresToStructureDictionary()  //adds all the temporarystructures to the dictionary so they becomes "true" roads
    {
        foreach (var temporaryRoads in temporaryRoadObjects)
        {
            structureDictionary.Add(temporaryRoads.Key, temporaryRoads.Value);
        }
        temporaryRoadObjects.Clear();   //its clears of all the road objects temporary
    }

    private void PlaceTemporaryStructure(Vector3 position, GameObject structurePrefab, CellType type)    //ads a new structure temporal
    {
        StructureModel structure = CreateNewStructureModel(position, structurePrefab, type);
        temporaryRoadObjects.Add(position, structure);  //we add it to the temporaryroads list
    }

    public void ModifyStructureModel(Vector3 finalPosition, StructureType newModel, Quaternion rotation)
    {
        if (temporaryRoadObjects.ContainsKey(finalPosition))
            temporaryRoadObjects[finalPosition].SwapModel(newModel, rotation);
        else if (structureDictionary.ContainsKey(finalPosition))
            structureDictionary[finalPosition].SwapModel(newModel, rotation);
    }

    private StructureModel CreateNewStructureModel(Vector3 finalPosition, GameObject structurePrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString()); //creates a new gameobject
        structure.transform.SetParent(roadsGO.transform);   //sets the parent which is the GO for all the strcutures displayed in the world
        structure.transform.localPosition = finalPosition;  //sets it in the position
        var structureModel = structure.AddComponent<StructureModel>();   //adds a component to the gameobject with some logic
        structureModel.CreateModel(structurePrefab);    //access to the method in the component which creates the new model
        return structureModel;
    }
}