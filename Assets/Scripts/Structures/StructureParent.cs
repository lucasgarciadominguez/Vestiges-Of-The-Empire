using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum StatesStructures
{
    Construction,
    NoRoad,
    NoSupply,
    NoSupplyElaboratedResources,
    NoWorkers,
    NoResources
    //TODO do more states
}

public enum RotationType
{
    None,
    RightDown,
    LeftDown,
    RightTop,
    LeftTop,
}

public class StructureParent : MonoBehaviour
{
    public string Name;
    public string description;
    public Sprite spriteStructure;
    public int rowsOcuppation;
    public int columnsOcuppation;
    public Vector3 actualPosition;
    public Vector3 actualRotation;
    public bool isConnectedToRoad = false;
    public bool canWork = false;
    public FunctionTimer functionConstruction;
    public GameObject structureMesh;
    public GameObject structureConstruction;
    public GameObject smokeConstructionDone;
    public GameObject sphereRadius;
    public GameObject enterBuilding;
    private FillConstructionBar constructionBar;
    public float timeBetweenCheckFunctionCanWork = 4;
    public float timeForFinishingConstruction = 4;
    public bool isBuild = false;
    public List<StatesStructures> states = new List<StatesStructures>();
    public GameManager gameManager;
    public List<AdviseUI> actualAdvisesUI = new List<AdviseUI>();  //TODO allow them to make it a list for showing different actualAdvises (change the statesStructures to be more than one..)
    public WorldGenerator grid;
    public List<Cell> cellsOcuppied = new List<Cell>();
    private float timeConstructionBar = 0;
    public List<Vector3> buildingEnterPositions = new List<Vector3>();
    public Vector3 buildingEnterPosition = new Vector3();
    public List<Vector2> offsetEntryPositions= new List<Vector2>();

    [SerializeField]
    public RotationType rotationType;


    private Structures structures;

    [SerializeField]
    public CellType cellType;

    public Vector3 originalPositionUIAdvises;

    public void SetAllStructureRequirements()
    {
        SetAllRequirements();
        StartConstruction();
        functionConstruction = new FunctionTimer(FinishConstruction, timeForFinishingConstruction, false);
    }

    private void StartConstruction()
    {
        CreateUIForNewState(StatesStructures.Construction);
        states.Add(StatesStructures.Construction);
        constructionBar = actualAdvisesUI.Find(j => j.ReturnState() == StatesStructures.Construction).ReturnGO().GetComponent<FillConstructionBar>();
    }

    public void FinishConstruction()
    {
        smokeConstructionDone.SetActive(true);
        structureConstruction.SetActive(false);
        structureMesh.SetActive(true);
        DeleteUIAdvise(StatesStructures.Construction);
        DeleteState(StatesStructures.Construction);
        isBuild = true;
        CheckIfItsConnectedToRoad();
        if (cellType == CellType.StructureInsula || cellType == CellType.StructureMidClass || cellType == CellType.StructureUpperClass )    //only checks if can work right after the building
                                                                                                                                                                                //is built if its a house, because it's the only structure that only needs a road connection for canWork=true (working)
        {
            CheckIfCanWork();
        }
    }

    public void UploadConstructionBar()
    {
        if (!isBuild)
        {
            timeConstructionBar -= Time.deltaTime;
            constructionBar.UpdateConstructionBar(-timeConstructionBar, timeForFinishingConstruction);
        }
    }

    public void UpdateAllStructureRequirements()
    {
        functionConstruction.Update();
        UploadConstructionBar();
    }
    public void BuildingEnterCreate()
    {
        switch (rotationType)
        {
            case RotationType.None:
                break;
            case RotationType.RightDown:
                foreach (var item in offsetEntryPositions)
                {
                    buildingEnterPositions.Add(new Vector3(actualPosition.x + item.x, 0, actualPosition.z + item.y));
                    grid.gridArray[Convert.ToInt32(actualPosition.x + item.x), Convert.ToInt32(actualPosition.z + item.y)].ChangeOcuppation(CellType.HouseConnection);
                }
 

                break;
            case RotationType.LeftDown:
                buildingEnterPositions.Add(new Vector3(actualPosition.x-1, 0, actualPosition.z - 1));
                grid.gridArray[Convert.ToInt32(actualPosition.x - 1), Convert.ToInt32(actualPosition.z - 1)].ChangeOcuppation(CellType.HouseConnection);

                buildingEnterPositions.Add(new Vector3(actualPosition.x - 1, 0, actualPosition.z - 2));
                grid.gridArray[Convert.ToInt32(actualPosition.x - 1), Convert.ToInt32(actualPosition.z - 2)].ChangeOcuppation(CellType.HouseConnection);

                break;

            case RotationType.LeftTop:
                buildingEnterPositions.Add(new Vector3(actualPosition.x - 1, 0, actualPosition.z + 1));
                grid.gridArray[Convert.ToInt32(actualPosition.x - 1), Convert.ToInt32(actualPosition.z + 1)].ChangeOcuppation(CellType.HouseConnection);

                buildingEnterPositions.Add(new Vector3(actualPosition.x - 2, 0, actualPosition.z + 1));
                grid.gridArray[Convert.ToInt32(actualPosition.x - 2), Convert.ToInt32(actualPosition.z + 1)].ChangeOcuppation(CellType.HouseConnection);

                break;
            case RotationType.RightTop:
                buildingEnterPositions.Add(new Vector3(actualPosition.x + 1, 0, actualPosition.z + 1));
                grid.gridArray[Convert.ToInt32(actualPosition.x + 1), Convert.ToInt32(actualPosition.z + 1)].ChangeOcuppation(CellType.HouseConnection);

                buildingEnterPositions.Add(new Vector3(actualPosition.x + 1, 0, actualPosition.z + 2));
                grid.gridArray[Convert.ToInt32(actualPosition.x + 1), Convert.ToInt32(actualPosition.z + 2)].ChangeOcuppation(CellType.HouseConnection);

                break;
            default:
                break;
        }
    }
    public virtual void SetOcuppationInGrid(WorldGenerator grid, CellType type)
    {
        Debug.Log(rotationType);
        switch (rotationType)
        {
            case RotationType.None:
                break;

            case RotationType.RightDown:
                for (int auxY = 1; auxY < rowsOcuppation; auxY++)
                {
                    for (int auxX = 1; auxX < columnsOcuppation; auxX++)
                    {
                        grid.gridArray[Convert.ToInt32(actualPosition.x + auxX), Convert.ToInt32(actualPosition.z - auxY)].ChangeOcuppation(type);
                        cellsOcuppied.Add(grid.gridArray[Convert.ToInt32(actualPosition.x + auxX), Convert.ToInt32(actualPosition.z - auxY)]);
                    }
                }
                break;

            case RotationType.LeftDown:
                for (int auxY = 1; auxY < rowsOcuppation; auxY++)
                {
                    for (int auxX = 1; auxX < columnsOcuppation; auxX++)
                    {
                        grid.gridArray[Convert.ToInt32(actualPosition.x - auxY), Convert.ToInt32(actualPosition.z - auxX)].ChangeOcuppation(type);
                        cellsOcuppied.Add(grid.gridArray[Convert.ToInt32(actualPosition.x - auxY), Convert.ToInt32(actualPosition.z - auxX)]);
                    }
                }
                break;

            case RotationType.LeftTop:
                for (int auxY = 1; auxY < rowsOcuppation; auxY++)
                {
                    for (int auxX = 1; auxX < columnsOcuppation; auxX++)
                    {
                        grid.gridArray[Convert.ToInt32(actualPosition.x - auxX), Convert.ToInt32(actualPosition.z + auxY)].ChangeOcuppation(type);
                        cellsOcuppied.Add(grid.gridArray[Convert.ToInt32(actualPosition.x - auxX), Convert.ToInt32(actualPosition.z + auxY)]);
                    }
                }
                break;

            case RotationType.RightTop:
                for (int auxY = 1; auxY < rowsOcuppation; auxY++)
                {
                    for (int auxX = 1; auxX < columnsOcuppation; auxX++)
                    {
                        grid.gridArray[Convert.ToInt32(actualPosition.x + auxY), Convert.ToInt32(actualPosition.z + auxX)].ChangeOcuppation(type);
                        cellsOcuppied.Add(grid.gridArray[Convert.ToInt32(actualPosition.x + auxY), Convert.ToInt32(actualPosition.z + auxX)]);
                    }
                }
                break;

            default:
                break;
        }
    }

    public void SetAllRequirements()
    {
        structures = FindObjectOfType<Structures>();
        structures.AddStructureToList(this);
        grid = FindObjectOfType<WorldGenerator>();
        actualPosition = transform.position;
        actualRotation = transform.rotation.eulerAngles;
        gameManager = FindObjectOfType<GameManager>();
        SetOcuppationInGrid(this.grid, cellType);
        BuildingEnterCreate();

        isConnectedToRoad = grid.CheckBuildingEnterConnection(buildingEnterPositions, CellType.Road, CellType.PivotRoad,ref buildingEnterPosition);

        if (isConnectedToRoad)
        {
            enterBuilding.SetActive(true);
            foreach (var item in buildingEnterPositions)
            {
                if (buildingEnterPosition!=item)
                {
                    grid.gridArray[(int)item.x, (int)item.z].cellType = cellType;
                }
            }
        }
        //ChangePrefab();                           //and if it's correct, marks it as true
    }

    public void CheckIfItsConnectedToRoad()
    {
        if (isConnectedToRoad == true)
        {
        }
        else
        {
            states.Add(StatesStructures.NoRoad);
            CreateUIForNewState(StatesStructures.NoRoad);  //makes the ui show that there is no road connection
        }
    }

    public void ChangeRoadConnection(bool b)    //changes if it's connected to a road or not

    {
        if (b)
        {
            isConnectedToRoad = true;
            enterBuilding.SetActive(true);

        }
        else if (b == false)
        {
            isConnectedToRoad = false;
        }
        CheckIfCanWork();
    }

    public void CreateUIForNewState(StatesStructures state)
    {
        gameManager.ShowMessagesUI(state, actualPosition, this);
    }

    public AdviseUI GetUIAdvise(StatesStructures state)
    {
        return actualAdvisesUI.Find(j => j.ReturnState() == state);
    }

    public void DeleteUIAdvise(StatesStructures state)
    {
        AdviseUI adviseUI = actualAdvisesUI.Find(j => j.ReturnState() == state);
        int index = actualAdvisesUI.FindIndex(j => j.ReturnState() == state);
        actualAdvisesUI.RemoveAt(index);
        gameManager.DeleteUIAdvise(adviseUI);
        Destroy(adviseUI.ReturnGO());
        gameManager.RefreshAdvisesUI(this);
    }

    public void SetRotation(RotationType type)
    {
        rotationType = new RotationType();
        this.rotationType = type;
    }

    public RotationType GetRotation()
    {
        return this.rotationType;
    }

    public void DeleteState(StatesStructures state)
    {
        int index = states.FindIndex(j => j == state);
        states.RemoveAt(index);
    }

    public void CheckIfCanWork()
    {
        Debug.Log("can work...?");
        if (states.Count == 0)
        {
            canWork = true;
        }
        else
        {
            canWork = false;
        }
    }
}