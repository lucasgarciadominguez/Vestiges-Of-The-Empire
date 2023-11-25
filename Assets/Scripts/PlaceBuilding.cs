using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// LUCAS GARCIA DOMINGUEZ
/// </summary>

public class PlaceBuilding : MonoBehaviour
{
    private BuildGridShader buildGrid;
    private RoadManager roadManager;
    public WorldGenerator grid { get; private set; }

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private UIManager uiManager;

    private RoadFixer roadFixer;


    [SerializeField]
    private UIManager managerUI;

    [SerializeField]
    private ShaderGrid heatMap;

    private Veins veins;

    public Vector3Int actualPosition;
    private Vector3 actualPositionFloat;
    private string actualCoordinatesPosition;

    [SerializeField] private GameObject plantationWheat;
    [SerializeField] private GameObject plantationGrape;
    [SerializeField] private GameObject plantationLine;
    [SerializeField] private GameObject plantationOil;
    [SerializeField] private GameObject farmCow;
    [SerializeField] private GameObject farmPig;
    [SerializeField] private GameObject houseInsula;
    [SerializeField] private GameObject houseMidClass;
    [SerializeField] private GameObject houseUpperClass;
    [SerializeField] private GameObject mineGold;
    [SerializeField] private GameObject mineIron;
    [SerializeField] private GameObject mineMarble;
    [SerializeField] private GameObject mineSilver;
    [SerializeField] private GameObject lumberCamp;
    [SerializeField] private GameObject pottery;
    [SerializeField] private GameObject bakery;
    [SerializeField] private GameObject blacksmith;
    [SerializeField] private GameObject tailorShop;
    [SerializeField] private GameObject temple;
    [SerializeField] private GameObject sanctuary;
    [SerializeField]
    private GameObject AllStructuresGO;

    [SerializeField]
    private Mineral mineralExtracting;

    [SerializeField]
    private ExtractiveResources resourcesExtracting;

    [SerializeField]
    public CellType cellTypeBuilding;

    private string lastName;
    private string lastDescription;

    private List<Vein> nearestVeins = new List<Vein>();
    private GameObject placeholderOfTheFinalConstruction;
    private GameObject incorrectPlaceholderOfTheFinalConstruction;
    [SerializeField]
    private Vector3 rotationPlaceHolder;
    private Vein actualShortestVein;

    [SerializeField]
    private bool placementMode;

    [SerializeField]
    private int efectiveness;

    [SerializeField]
    private bool ExtractingModeControl = false;

    [SerializeField]
    private LayerMask natureLayer;

    private List<GameObject> natureForDestroy = new List<GameObject>();
    private bool isMineralSelected = false;

    [SerializeField]
    private bool canBuild = false;

    private List<int> efectivenessExtractiveStructure = new List<int>();

    private void Awake()
    {
        grid = FindObjectOfType<WorldGenerator>();    //finds the grid in the hierarchy
        roadManager = GetComponent<RoadManager>();
        buildGrid = GetComponent<BuildGridShader>();
        roadFixer = GetComponent<RoadFixer>();
        cellTypeBuilding = CellType.None;   //initializes as none building being constructed
    }

    private void Start()
    {
        buildGrid.SetSizeBuildGridShader(grid.size);
    }

    public void ChangeGridBuilding(bool check)
    {
        if (check)
        {
            grid.EnableBuildingMode(buildGrid.ReturnGridMaterialShader());
        }
        else
        {
            grid.DisableBuildingMode();
        }
    }

    public void BuildingPlaceHolder(GameObject placeHolder, GameObject incorrectPlaceHolder, string name, string description)
    {
        placeholderOfTheFinalConstruction = Instantiate(placeHolder);   //Passes via button, the type of gameobject you wanted to instantiate as a placeholder
        incorrectPlaceholderOfTheFinalConstruction = Instantiate(incorrectPlaceHolder);
        lastName = name;
        lastDescription = description;
    }

    public void ChangeConstructionStatus(string type)
    {
        ChangeGridBuilding(true);//enable the grid shader for building
        switch (type)
        {
            case "None":
                this.cellTypeBuilding = CellType.None;

                break;

            case "Road":
                this.cellTypeBuilding = CellType.Road;
                break;

            case "StructureInsula":
                this.cellTypeBuilding = CellType.StructureInsula;
                break;

            case "StructureMidClass":
                this.cellTypeBuilding = CellType.StructureMidClass;
                break;

            case "StructureUpperClass":
                this.cellTypeBuilding = CellType.StructureUpperClass;
                break;

            case "Mine":
                this.cellTypeBuilding = CellType.Mine;
                EnableExtractingMode(CellType.Mine);
                break;

            case "LumberCamp":
                this.cellTypeBuilding = CellType.LumberCamp;
                break;

            case "Pottery":
                this.cellTypeBuilding = CellType.Pottery;
                break;

            case "Farm":
                this.cellTypeBuilding = CellType.Farm;
                EnableExtractingMode(CellType.Farm);
                break;

            case "Plantation":
                this.cellTypeBuilding = CellType.Plantation;
                EnableExtractingMode(CellType.Plantation);
                break;

            case "Bakery":
                this.cellTypeBuilding = CellType.Bakery;
                break;

            case "BlackSmith":
                this.cellTypeBuilding = CellType.BlackSmith;
                break;

            case "TailorShop":
                this.cellTypeBuilding = CellType.TailorShop;
                break;

            case "Temple":
                this.cellTypeBuilding = CellType.Temple;
                break;
            case "Sanctuary":
                this.cellTypeBuilding= CellType.Sanctuary;
                break;

            default:
                break;
        }
        //TODO, check the economy to see if it is possible to build and check if it is an sttructure or a building, sopecial building...
    }

    public void EnableExtractingMode(CellType type)
    {
        ExtractingModeControl = true;
        switch (type)
        {
            case CellType.Plantation:
                managerUI.EnableUIExtractingSystemPlantation();
                break;

            case CellType.Mine:
                managerUI.EnableUIExtractingSystem();

                break;

            case CellType.Farm:
                managerUI.EnableUIExtractingSystemFarm();
                break;

            default:
                break;
        }
    }

    public void ChangeExtractingMineral(Material mat, List<Vein> listVeins, Mineral typeMineral)
    {
        nearestVeins = listVeins;
        mineralExtracting = typeMineral;
        grid.ChangeExtractingMode(mat);
        isMineralSelected = true;
    }

    public void ChangeExtractingResources(Material mat, ExtractiveResources typeResources)
    {
        resourcesExtracting = typeResources;
        grid.ChangeExtractingMode(mat);
        isMineralSelected = true;
    }

    public void DisableExtractingMode(CellType type)
    {
        isMineralSelected = false;
        ExtractingModeControl = false;
        grid.ChangeExtractingMode(false);
        switch (type)
        {
            case CellType.Plantation:
                managerUI.DisableUIExtractingSystemPlantation();
                break;

            case CellType.Mine:
                managerUI.DisableUIExtractingSystem();

                break;

            case CellType.Farm:
                managerUI.DisableUIExtractingSystemFarm();
                break;

            default:
                break;
        }
    }

    public int ReturnEfectivinessForExtractiveStructure()
    {
        if (resourcesExtracting == ExtractiveResources.Iron || resourcesExtracting == ExtractiveResources.Gold || resourcesExtracting == ExtractiveResources.Silver || resourcesExtracting == ExtractiveResources.Gold)
        {
            int i= grid.ReturnAverageEfectivenessExtractiveBuildings(actualPosition, placeholderOfTheFinalConstruction.GetComponent<StructureParent>(), rotationPlaceHolder, efectivenessExtractiveStructure, resourcesExtracting);
            if (i>=50)
            {
                i = (i / 2);
                return i;
            }
            else
            {
                i = (i * 2);
                return i;
            }
        }
        else
        {
            return grid.ReturnAverageEfectivenessExtractiveBuildings(actualPosition, placeholderOfTheFinalConstruction.GetComponent<StructureParent>(), rotationPlaceHolder, efectivenessExtractiveStructure, resourcesExtracting) * 2;

        }

    }

    //public Vector3 GetNearestVein()
    //{
    //    //float distance = 100;
    //    //Vector3 nearestPoint = Vector3.zero;
    //    //for (int i = 0; i < nearestVeins.Count; i++)
    //    //{
    //    //    if (distance > Vector3.Distance(actualPositionFloat, nearestVeins[i].GetLocation()))
    //    //    {
    //    //        distance = Vector3.Distance(actualPositionFloat, nearestVeins[i].GetLocation());

    //    //        nearestPoint = nearestVeins[i].GetLocation();
    //    //        actualShortestVein = nearestVeins[i];
    //    //    }
    //    //    else
    //    //    {
    //    //    }
    //    //}
    //    //efectiveness = (int)(heatMap.getPercentForPixel(heatMap.distsq(distance)));
    //    //managerUI.ChangeExtractingSystemPercent(efectiveness);

    //    //return nearestPoint;
    //}

    private void Update()
    {
        actualPosition = (Vector3Int)inputManager.GetPosition(); //gets the actualPosition of the mouse in the grid with entire numbers as for example vector3(1,0,2)

        if (gameManager.state == ControllerMode.Build && !gameManager.placeHolderMode)
        {
            if (canBuild)
            {
                if (Input.GetKeyDown(KeyCode.M) && cellTypeBuilding != CellType.None)
                {
                    PlaceBuildings(actualPosition);
                }
                else if (Input.GetMouseButtonUp(0) && cellTypeBuilding == CellType.Road)
                {
                    roadManager.FinishPlacingRoad(ref placementMode);
                }
                if (inputManager.isClickedDown == true && cellTypeBuilding == CellType.Road)    //if its clicked and some construction is selected
                {
                    PlaceBuildings(actualPosition);
                }
                else if (inputManager.isClickedDown == true && cellTypeBuilding != CellType.None)
                {
                    PlaceBuildings(actualPosition);
                }
            }
            else
            {
                gameManager.placeHolderMode = true; //when clicks in an incorrect position, it continious playing
            }
        }
        else if (gameManager.state == ControllerMode.Build && gameManager.placeHolderMode)
        {
            if (cellTypeBuilding != CellType.None && placeholderOfTheFinalConstruction.gameObject != null)
            {
                if (resourcesExtracting == ExtractiveResources.None&&(cellTypeBuilding==CellType.Mine|| cellTypeBuilding == CellType.Plantation|| cellTypeBuilding == CellType.Farm))
                {
                    canBuild = false;
                    placeholderOfTheFinalConstruction.SetActive(false);
                    incorrectPlaceholderOfTheFinalConstruction.SetActive(true);

                    incorrectPlaceholderOfTheFinalConstruction.transform.position = actualPosition; //follows the cursor when it's checked the button for some structure
                    incorrectPlaceholderOfTheFinalConstruction.transform.rotation = placeholderOfTheFinalConstruction.transform.rotation;

                }
                else if (grid.IsNotOcuppied(actualPosition, placeholderOfTheFinalConstruction.GetComponent<StructureParent>(), rotationPlaceHolder, cellTypeBuilding) && grid.IsNotWater(actualPosition, placeholderOfTheFinalConstruction.GetComponent<StructureParent>(), rotationPlaceHolder))
                {
                    canBuild = true;
                    placeholderOfTheFinalConstruction.SetActive(true);

                    incorrectPlaceholderOfTheFinalConstruction.SetActive(false);
                    placeholderOfTheFinalConstruction.transform.position = actualPosition; //follows the cursor when it's checked the button for some structure
                }
                else
                {
                    canBuild = false;
                    placeholderOfTheFinalConstruction.SetActive(false);
                    incorrectPlaceholderOfTheFinalConstruction.SetActive(true);

                    incorrectPlaceholderOfTheFinalConstruction.transform.position = actualPosition; //follows the cursor when it's checked the button for some structure
                    incorrectPlaceholderOfTheFinalConstruction.transform.rotation = placeholderOfTheFinalConstruction.transform.rotation;
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    placeholderOfTheFinalConstruction.transform.rotation = changesRotationofthePlaceHolderAndFinalInstance();   //sets the new rotation to the placeholder and changes in real time
                }
            }
            if (ExtractingModeControl)
            {
                actualPositionFloat = (Vector3)inputManager.GetPosition();
                actualCoordinatesPosition = inputManager.GetPositionForCoordenates();
                if (canBuild)
                {

                    efectiveness = ReturnEfectivinessForExtractiveStructure();
                    managerUI.ChangeExtractingSystemPercent(efectiveness, resourcesExtracting);
                }
                else
                {
                    efectiveness = 0;
                    managerUI.ChangeExtractingSystemPercent(efectiveness, resourcesExtracting);
                }
            }
        }
    }

    public void StopBuilding()
    {
        Destroy(placeholderOfTheFinalConstruction.gameObject);  //destroys the instance of the placeholder now that the actual real gameobject is instantiated
        Destroy(incorrectPlaceholderOfTheFinalConstruction.gameObject);
        if (cellTypeBuilding == CellType.Mine || cellTypeBuilding == CellType.Plantation || cellTypeBuilding == CellType.Farm)
        {
            DisableExtractingMode(cellTypeBuilding);
        }
        rotationPlaceHolder = Vector3.zero;

        cellTypeBuilding = CellType.None;   //sets the typeofBuilding to zero
        ChangeGridBuilding(false);  //disable the grid shader for building
        gameManager.ChangeState(ControllerMode.Play);
        uiManager.EnableAndDisableCanvasBuildMode();
    }

    public void AddNatureForDestroy(Vector3 position)
    {
        RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), transform.up, Quaternion.identity, 0.5f, natureLayer);
        foreach (var item in hits)
        {
            natureForDestroy.Add(item.transform.gameObject);
        }
    }
    public void AddNatureForDestroy(Vector3 position, StructureParent structureParent)
    {
        RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), transform.up, Quaternion.identity, 0.5f, natureLayer);
        foreach (var item in hits)
        {
            natureForDestroy.Add(item.transform.gameObject);
        }
    }

    public void DestroyNature()
    {
        foreach (var item in natureForDestroy)
        {
            Destroy(item.transform.gameObject);
        }
        natureForDestroy.Clear();
    }

    private void PlaceBuildings(Vector3 position)
    {
        PlaceBuildingNear(rotationPlaceHolder, position); //places the building and uses the placeholder rotation for the new instance of the new object
        //Destroy(placeholderOfTheFinalConstruction.gameObject);  //destroys the instance of the placeholder now that the actual real gameobject is instantiated
        //cellTypeBuilding = CellType.None;   //sets the typeofBuilding to zero
    }

    private void PlaceBuildingNear(Vector3 rotation, Vector3 position)
    {
        Quaternion quaternion = Quaternion.Euler(rotation);   //passes the rotation that is in a vector3 to a quaternion

        if (grid.IsNotOcuppied(position, cellTypeBuilding, canBuild))    //here is where is checked that is not water, that is not ocuppied and where is set
        {
            if (cellTypeBuilding == CellType.Road)
            {
                roadManager.PlaceRoad(position, ref placementMode);
            }
            else if (cellTypeBuilding == CellType.StructureInsula)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats((uiManager.lastItemNeeded));

                        GameObject house = this.houseInsula;
                        house.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());

                        Instantiate(house, position, quaternion, AllStructuresGO.transform);  //makes an instance for a new house with all the logic associated
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                DestroyNature();
                FinishBuilding();
            }
            else if (cellTypeBuilding == CellType.StructureMidClass)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats(uiManager.lastItemNeeded);

                        GameObject house = this.houseMidClass;
                        house.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());

                        Instantiate(house, position, quaternion, AllStructuresGO.transform);  //makes an instance for a new house with all the logic associated
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                DestroyNature();
                FinishBuilding();
            }
            else if (cellTypeBuilding == CellType.StructureUpperClass)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats(uiManager.lastItemNeeded);

                        GameObject house = this.houseUpperClass;
                        house.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());

                        Instantiate(house, position, quaternion, AllStructuresGO.transform);  //makes an instance for a new house with all the logic associated
                        DestroyNature();
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                FinishBuilding();
            }
            else if (cellTypeBuilding == CellType.Mine)
            {
                if (isMineralSelected)
                {
                    AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                    if (canBuild)
                    {
                        if (gameManager.CanBuy(uiManager.lastItemNeeded))
                        {
                            DestroyNature();

                            gameManager.ReduceStats((uiManager.lastItemNeeded));
                            switch (resourcesExtracting)
                            {
                                case ExtractiveResources.None:
                                    break;

                                case ExtractiveResources.Gold:
                                    GameObject mine = this.mineGold;
                                    mine.GetComponent<Mine>().efectiveness = efectiveness;
                                    mine.GetComponent<Mine>().totalAmountVeinUsing = UnityEngine.Random.Range(1000,2001);
                                    mine.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    Instantiate(mine, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;

                                case ExtractiveResources.Iron:
                                    GameObject mineIron = this.mineIron;
                                    mineIron.GetComponent<Mine>().efectiveness = efectiveness;
                                    mineIron.GetComponent<Mine>().totalAmountVeinUsing = UnityEngine.Random.Range(5000, 10000);
                                    mineIron.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    Instantiate(mineIron, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;

                                case ExtractiveResources.Marble:
                                    GameObject mineMarble = this.mineMarble;
                                    mineMarble.GetComponent<Mine>().efectiveness = efectiveness;
                                    mineMarble.GetComponent<Mine>().totalAmountVeinUsing = UnityEngine.Random.Range(7000, 9000);
                                    mineMarble.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    Instantiate(mineMarble, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;
                                case ExtractiveResources.Silver:
                                    GameObject mineSilver = this.mineSilver;
                                    mineSilver.GetComponent<Mine>().efectiveness = efectiveness;
                                    mineSilver.GetComponent<Mine>().totalAmountVeinUsing = UnityEngine.Random.Range(2500, 5000);
                                    mineSilver.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    Instantiate(mineSilver, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;

                                default:
                                    break;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Not enough items");
                        }
                    }

                    FinishBuilding();
                }
            }
            else if (cellTypeBuilding == CellType.LumberCamp)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());

                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats(uiManager.lastItemNeeded);

                        GameObject lumberCamp = this.lumberCamp;
                        lumberCamp.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                        Instantiate(lumberCamp, position, quaternion, AllStructuresGO.transform);
                        DestroyNature();
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                FinishBuilding();
            }
            else if (cellTypeBuilding == CellType.Pottery)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());

                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats(uiManager.lastItemNeeded);

                        GameObject potteryGO = this.pottery;
                        potteryGO.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                        Instantiate(potteryGO, position, quaternion, AllStructuresGO.transform);
                        DestroyNature();
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                FinishBuilding();
            }
            else if (cellTypeBuilding == CellType.Plantation)
            {
                if (isMineralSelected)
                {
                    AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                    if (canBuild)
                    {
                        if (gameManager.CanBuy(uiManager.lastItemNeeded))
                        {
                            DestroyNature();

                            gameManager.ReduceStats((uiManager.lastItemNeeded));
                            switch (resourcesExtracting)
                            {
                                case ExtractiveResources.None:
                                    break;

                                case ExtractiveResources.Wheat:
                                    GameObject plantationWheat = this.plantationWheat;
                                    plantationWheat.GetComponent<ExtractiveResourcesJobBuilding>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    plantationWheat.GetComponent<ExtractiveResourcesJobBuilding>().efectiveness = this.efectiveness;
                                    Instantiate(plantationWheat, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;

                                case ExtractiveResources.Grape:
                                    GameObject plantationGrape = this.plantationGrape;
                                    plantationGrape.GetComponent<ExtractiveResourcesJobBuilding>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    plantationGrape.GetComponent<ExtractiveResourcesJobBuilding>().efectiveness = this.efectiveness;

                                    Instantiate(plantationGrape, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;

                                case ExtractiveResources.Oil:
                                    GameObject plantationOil = this.plantationOil;
                                    plantationOil.GetComponent<ExtractiveResourcesJobBuilding>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    plantationOil.GetComponent<ExtractiveResourcesJobBuilding>().efectiveness = this.efectiveness;

                                    Instantiate(plantationOil, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;

                                case ExtractiveResources.Linen:
                                    GameObject plantationLine = this.plantationLine;
                                    plantationLine.GetComponent<ExtractiveResourcesJobBuilding>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    plantationLine.GetComponent<ExtractiveResourcesJobBuilding>().efectiveness = this.efectiveness;

                                    Instantiate(plantationLine, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;

                                default:
                                    break;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Not enough items");
                        }
                    }

                    FinishBuilding();
                }
            }
            else if (cellTypeBuilding == CellType.Farm)
            {
                if (isMineralSelected)
                {
                    AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                    if (canBuild)
                    {
                        if (gameManager.CanBuy(uiManager.lastItemNeeded))
                        {
                            DestroyNature();

                            gameManager.ReduceStats((uiManager.lastItemNeeded));
                            switch (resourcesExtracting)
                            {
                                case ExtractiveResources.None:
                                    break;

                                case ExtractiveResources.Cow:
                                    GameObject farmCow = this.farmCow;
                                    farmCow.GetComponent<ExtractiveResourcesJobBuilding>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    farmCow.GetComponent<ExtractiveResourcesJobBuilding>().efectiveness = this.efectiveness;
                                    Instantiate(farmCow, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;

                                case ExtractiveResources.Pig:
                                    GameObject farmPig = this.farmPig;
                                    farmPig.GetComponent<ExtractiveResourcesJobBuilding>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                                    farmPig.GetComponent<ExtractiveResourcesJobBuilding>().efectiveness = this.efectiveness;
                                    Instantiate(farmPig, position, quaternion, AllStructuresGO.transform);
                                    DestroyNature();
                                    break;

                                default:

                                    break;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Not enough items");
                        }
                    }

                    FinishBuilding();
                }
            }
            else if (cellTypeBuilding == CellType.Temple)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats(uiManager.lastItemNeeded);

                        GameObject temple = this.temple;
                        temple.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                        Instantiate(temple, position, quaternion, AllStructuresGO.transform);
                        DestroyNature();
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                FinishBuilding();
            }
            else if (cellTypeBuilding == CellType.Sanctuary)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats(uiManager.lastItemNeeded);

                        GameObject sanctuary = this.sanctuary;
                        temple.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                        Instantiate(sanctuary, position, quaternion, AllStructuresGO.transform);
                        DestroyNature();
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                FinishBuilding();
            }
            else if (cellTypeBuilding == CellType.BlackSmith)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats(uiManager.lastItemNeeded);

                        GameObject blacksmith = this.blacksmith;
                        blacksmith.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());

                        Instantiate(blacksmith, position, quaternion, AllStructuresGO.transform);
                        DestroyNature();
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                FinishBuilding();
            }
            else if (cellTypeBuilding == CellType.Bakery)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats(uiManager.lastItemNeeded);

                        GameObject bakery = this.bakery;
                        bakery.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());

                        Instantiate(bakery, position, quaternion, AllStructuresGO.transform);
                        DestroyNature();
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                FinishBuilding();
            }
            else if (cellTypeBuilding == CellType.TailorShop)
            {
                AddNatureForDestroy(position, placeholderOfTheFinalConstruction.GetComponent<StructureParent>());
                if (canBuild)
                {
                    if (gameManager.CanBuy(uiManager.lastItemNeeded))
                    {
                        gameManager.ReduceStats(uiManager.lastItemNeeded);

                        GameObject tailorShop = this.tailorShop;
                        tailorShop.GetComponent<StructureParent>().SetRotation(placeholderOfTheFinalConstruction.GetComponent<StructureParent>().GetRotation());
                        Instantiate(tailorShop, position, quaternion, AllStructuresGO.transform);
                        DestroyNature();
                    }
                    else
                    {
                        Debug.LogWarning("Not enough items");
                    }
                }
                FinishBuilding();
            }
        }
    }

    public void FinishBuilding()
    {
        gameManager.placeHolderMode = true;
        gameManager.audioManager.ConstructionFinishedAndPlaced();
    }

    private Quaternion changesRotationofthePlaceHolderAndFinalInstance()
    {
        rotationPlaceHolder = placeholderOfTheFinalConstruction.transform.rotation.eulerAngles;
        rotationPlaceHolder.y += 90;
        return Quaternion.Euler(rotationPlaceHolder);
    }

    public List<Vector3> GetNeighbourtOfTypeFor(Vector3 position, CellType road)   //returns the types of
    {
        List<Vector3> neighbourVertices = grid.GetAdjacentCellsOfTypeForRoads(position, road);
        return neighbourVertices;
    }

    public CellType[] GetNeighbourtTypesFor(Vector3 temporaryPosition)
    {
        return grid.GetAllAdjacentCellTypes(Convert.ToInt32(temporaryPosition.x), Convert.ToInt32(temporaryPosition.z));
    }

    public int GetNeighbourtTypesForWithWater(Vector3 temporaryPosition)
    {
        return grid.CheckIfWaterIsNext(temporaryPosition);
    }

    public bool isThisNotCellWater(Vector3 temporaryPosition)
    {
        return grid.IsNotWater(temporaryPosition);
    }
}