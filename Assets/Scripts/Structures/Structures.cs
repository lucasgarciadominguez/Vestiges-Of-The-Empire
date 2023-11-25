using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Structures : MonoBehaviour
{
    [SerializeField]
    public List<StructureParent> structures = new List<StructureParent>();

    // Start is called before the first frame update
    private void Start()
    {
    }

    public void ChangeRoadConnection(Vector3 location)
    {
        string locationString = location.x + "," + location.z;
        StructureParent structureSelected = new StructureParent();    //searches a house with the same position as the one who have been send in the method and then changes his connection to a road
        foreach (var item in structures)
        {
            if (item.cellsOcuppied.Find(cell => cell.getPosition() == locationString) != null)
            {
                structureSelected = item;
                if (!structureSelected.isConnectedToRoad)
                {
                    if (structureSelected.GetUIAdvise(StatesStructures.NoRoad) != null)
                    {
                        structureSelected.DeleteUIAdvise(StatesStructures.NoRoad);
                        structureSelected.DeleteState(StatesStructures.NoRoad);
                    }
                    structureSelected.ChangeRoadConnection(true);
                    structureSelected.buildingEnterPosition= location;
                    foreach (var structureEnters in structureSelected.buildingEnterPositions)
                    {
                        if (structureSelected.buildingEnterPosition != structureEnters)
                        {
                            structureSelected.grid.gridArray[(int)structureEnters.x, (int)structureEnters.z].cellType = structureSelected.cellType;

                        }

                    }

                }
            }
        }
    }

    public void AddStructureToList(StructureParent structure)
    {
        if (structure.GetComponent<HouseBuilding>()) 
        {
            Debug.Log("House added");
            Houses.AddHouse(structure.GetComponent<HouseBuilding>());
        }
        structures.Add(structure);  //adds a house to the list
    }
}