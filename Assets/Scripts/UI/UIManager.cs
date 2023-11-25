using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public class AdviseUI
{
    public GameObject adviseUIGO { private get; set; }

    public StatesStructures stateAdviseUI { private get; set; }

    public GameObject ReturnGO()
    {
        return adviseUIGO;
    }

    public StatesStructures ReturnState()
    {
        return stateAdviseUI;
    }
}

public class UIManager : MonoBehaviour
{
    public UIPause pauseMenu;
    public GameManager gameManager;
    public InputManager inputManager;
    public PlaceBuilding placeBuilding;
    public SwitchInspectorAndLoadItems switchInspectorAndLoadItems;
    [SerializeField]
    ChangeScenes changeScenes;

    [SerializeField]
    private TextMeshProUGUI descriptionItemBuildingRadialMenu;

    [SerializeField]
    private TextMeshProUGUI amountClothesTXT;

    [SerializeField]
    private TextMeshProUGUI amountLinenTXT;

    [SerializeField]
    private TextMeshProUGUI amountOilTXT;

    [SerializeField]
    private TextMeshProUGUI amountWineTXT;

    [SerializeField]
    private TextMeshProUGUI amountBreadTXT;

    [SerializeField]
    private TextMeshProUGUI amountCeramicsTXT;

    [SerializeField]
    private TextMeshProUGUI amountMeatTXT;

    [SerializeField]
    private TextMeshProUGUI amountWheatTXT;

    [SerializeField]
    private TextMeshProUGUI amountWoodTXT;

    [SerializeField]
    private TextMeshProUGUI amountMarbleTXT;

    [SerializeField]
    private TextMeshProUGUI amountSilverTXT;
    [SerializeField]
    private TextMeshProUGUI amountIronTXT;
    [SerializeField]
    private TextMeshProUGUI amountGoldTXT;

    [SerializeField]
    private TextMeshProUGUI amountWeaponsTXT;

    [SerializeField]
    private TextMeshProUGUI amountFaithTXT;

    [SerializeField]
    private TextMeshProUGUI amountFreePeopleTXT;


    [SerializeField]
    public GameObject extractingSystemTXT;

    [SerializeField]
    public GameObject extractingSystemPercentTXT;

    [SerializeField]
    public GameObject selectExtractingSystemPercent;

    [SerializeField]
    public GameObject extractingSystemPlantationTXT;

    [SerializeField]
    public GameObject extractingSystemPercentPlantationTXT;

    [SerializeField]
    public GameObject selectExtractingSystemPlantationPercent;

    [SerializeField]
    public GameObject extractingSystemFarmTXT;

    [SerializeField]
    public GameObject extractingSystemPercentFarmTXT;

    [SerializeField]
    public GameObject selectExtractingSystemFarmPercent;

    [SerializeField]
    public GameObject constructionBar;
    [SerializeField]
    public GameObject noSupplyUI;

    [SerializeField]
    public GameObject noRoadConnectionUI;

    [SerializeField]
    public GameObject noWorkersUI;

    [SerializeField]
    public GameObject noElaboratedResourcesUI;

    [SerializeField]
    public GameObject canvasWorld3D;

    [SerializeField]
    private float heightUIWorldSpace;

    [SerializeField]
    private TextMeshProUGUI inspectorName;

    [SerializeField]
    private TextMeshProUGUI inspectorDescription;

    [SerializeField]
    private bool currentStateBuilderMenu = false;

    [SerializeField]
    private bool currentStateBuilderMenuExtractive = false;

    [SerializeField]
    private UnityEvent turnedOnBuilderMenu;

    [SerializeField]
    private UnityEvent turnedOffBuilderMenu;

    [SerializeField]
    private UIBuilderStructureMenu builderStructureMenu;

    public RingMenu mainMenuPrefab;
    public RingMenu mainMenuInstance;
    public Canvas canvasBuildConstruction;
    public Image backgroundBuildMode;
    private bool controlModeBuildMenu = true;
    public ItemNeeded[] lastItemNeeded;

    [SerializeField]
    List<AdviseUI> advisesUI = new List<AdviseUI>();

    [SerializeField]
    float distanceBetweenAdvisesUI = 2;
    private void Update()
    {
        if (inputManager.isClickedDownRight)
        {
            if (mainMenuInstance == null)
            {
                mainMenuInstance = Instantiate(mainMenuPrefab, canvasBuildConstruction.transform);
            }
            else
            {
                if (controlModeBuildMenu)
                {
                    mainMenuInstance.gameObject.SetActive(true);
                    controlModeBuildMenu = false;
                }
            }
        }
        if (inputManager.isClickedDownRight == false)
        {
            if (mainMenuInstance != null)
            {
                if (mainMenuInstance != null)
                {
                    if (mainMenuInstance.itemSelected)
                    {
                        lastItemNeeded = mainMenuInstance.ReturnsItemsNeededElement();
                        gameManager.ChangeState(ControllerMode.Build);
                        gameManager.EnableOrDisableBuildMenuMode();
                        gameManager.placeHolderMode = true;
                        placeBuilding.ChangeConstructionStatus(mainMenuInstance.ReturnsElement());
                        placeBuilding.BuildingPlaceHolder(mainMenuInstance.ReturnsGameObjectElement(), mainMenuInstance.ReturnsGameObjectIncorrectElement(), mainMenuInstance.ReturnsElement(), mainMenuInstance.ReturnsElementDescription());
                        mainMenuInstance.itemSelected = false;
                        EnableAndDisableCanvasBuildMode();
                    }
                    mainMenuInstance.gameObject.SetActive(false);
                }

                if (mainMenuInstance.childs.Count > 0)
                {
                    RingMenu ring = mainMenuInstance.childs.Find(j => j.numberIndexParent == mainMenuInstance.activeElement);

                    if (ring)
                    {
                        if (ring.itemSelected)
                        {
                            lastItemNeeded = ring.ReturnsItemsNeededElement();
                            gameManager.ChangeState(ControllerMode.Build);
                            gameManager.EnableOrDisableBuildMenuMode();
                            gameManager.placeHolderMode = true;
                            placeBuilding.ChangeConstructionStatus(ring.ReturnsElement());
                            placeBuilding.BuildingPlaceHolder(ring.ReturnsGameObjectElement(), ring.ReturnsGameObjectIncorrectElement(), ring.ReturnsElement(), ring.ReturnsElementDescription());
                            ring.itemSelected = false;
                            EnableAndDisableCanvasBuildMode();
                        }
                        if (ring)
                        {
                            ring.gameObject.SetActive(false);
                        }
                    }
                }
                controlModeBuildMenu = true;
            }
        }
        if (gameManager.state == ControllerMode.BuildMenu)
        {
            if (mainMenuInstance != null)
            {
                mainMenuInstance.transform.position = inputManager.mouseposition;
            }
        }
    }
    public void ExitGame()
    {
        changeScenes.LoadMenuScene();
    }
    public void OpenInspectorNeeded(GameObject gameObjectClicked)
    {
        TypeInspectorSelected typeInspectorSelected;
        if (gameObjectClicked.GetComponentInParent<HouseBuilding>()) 
        {
            typeInspectorSelected = TypeInspectorSelected.HouseBuildings;
            switchInspectorAndLoadItems.LoadInspector(typeInspectorSelected, gameObjectClicked.GetComponentInParent<HouseBuilding>());

        }
        else if (gameObjectClicked.GetComponentInParent<ElaboratedResourcesJobBuilding>())
        {
            typeInspectorSelected = TypeInspectorSelected.ElaboratedResources;
            switchInspectorAndLoadItems.LoadInspector(typeInspectorSelected, gameObjectClicked.GetComponentInParent<ElaboratedResourcesJobBuilding>());

        }
        else if (gameObjectClicked.GetComponentInParent<ExtractiveResourcesJobBuilding>())
        {
            typeInspectorSelected = TypeInspectorSelected.ExtractiveResources;
            switchInspectorAndLoadItems.LoadInspector(typeInspectorSelected, gameObjectClicked.GetComponentInParent<ExtractiveResourcesJobBuilding>());

        }
    }
    public void HideInspectors()
    {
        switchInspectorAndLoadItems.HideInspectors();
    }
    public void UpdateDescriptionRadialBuildMenu(string data)
    {
        descriptionItemBuildingRadialMenu.text = data;
    }

    public void EnableAndDisableCanvasBuildMode()
    {
        backgroundBuildMode.enabled = !backgroundBuildMode.enabled;
    }
    public void EnableMenuBuilder()
    {
        turnedOnBuilderMenu.Invoke();

    }
    public void DisableMenuBuilder()
    {
        turnedOffBuilderMenu.Invoke();

    }
    public void EnableAndDisableMenuBuilder()
    {
        if (!builderStructureMenu.isPlaying)
        {
            currentStateBuilderMenu = !currentStateBuilderMenu;
            if (currentStateBuilderMenu)
                turnedOnBuilderMenu.Invoke();
            else
                turnedOffBuilderMenu.Invoke();
        }
    }
    public List<AdviseUI> ReturnUIAdvises()
    {
        return advisesUI;
    }
    public void RefreshUIStatsPoblationFree(int count)
    {
        amountFreePeopleTXT.text=count.ToString();
    }
    public void RefreshUIStatsSlaves(int count)
    {

    }

    public void TurnOnBuilderMenu()
    {
        if (!builderStructureMenu.isPlaying)
        {
        }
    }

    public void TurnOffBuilderMenu()
    {
        if (!builderStructureMenu.isPlaying)
        {
        }
    }

    public void EnableUIExtractingSystem()
    {
        extractingSystemPercentTXT.SetActive(true);
        extractingSystemTXT.SetActive(true);
        selectExtractingSystemPercent.SetActive(true);
    }

    public void DisableUIExtractingSystem()
    {
        extractingSystemPercentTXT.SetActive(false);
        extractingSystemTXT.SetActive(false);
        selectExtractingSystemPercent.GetComponent<TMPro.TMP_Dropdown>().value = 0;

        selectExtractingSystemPercent.SetActive(false);
    }

    public void EnableUIExtractingSystemFarm()
    {
        extractingSystemPercentFarmTXT.SetActive(true);
        extractingSystemFarmTXT.SetActive(true);
        selectExtractingSystemFarmPercent.SetActive(true);
    }

    public void DisableUIExtractingSystemFarm()
    {
        extractingSystemPercentFarmTXT.SetActive(false);
        extractingSystemFarmTXT.SetActive(false);
        selectExtractingSystemFarmPercent.GetComponent<TMPro.TMP_Dropdown>().value = 0;
        selectExtractingSystemFarmPercent.SetActive(false);

    }

    public void EnableUIExtractingSystemPlantation()
    {
        extractingSystemPercentPlantationTXT.SetActive(true);
        extractingSystemPlantationTXT.SetActive(true);
        selectExtractingSystemPlantationPercent.SetActive(true);
    }

    public void DisableUIExtractingSystemPlantation()
    {
        extractingSystemPercentPlantationTXT.SetActive(false);
        extractingSystemPlantationTXT.SetActive(false);
        selectExtractingSystemPlantationPercent.GetComponent<TMPro.TMP_Dropdown>().value = 0;

        selectExtractingSystemPlantationPercent.SetActive(false);
    }

    public void ChangeExtractingSystemPercent(decimal num,ExtractiveResources extractiveResources)
    {
        if (extractiveResources==ExtractiveResources.Pig|| extractiveResources == ExtractiveResources.Cow)
        {
            extractingSystemPercentFarmTXT.GetComponent<TextMeshProUGUI>().text = num.ToString() + " %";

        }
        else if (extractiveResources == ExtractiveResources.Wheat || extractiveResources == ExtractiveResources.Grape|| extractiveResources == ExtractiveResources.Linen || extractiveResources == ExtractiveResources.Oil)
        {
            extractingSystemPercentPlantationTXT.GetComponent<TextMeshProUGUI>().text = num.ToString() + " %";


        }
        else
        {
            extractingSystemPercentTXT.GetComponent<TextMeshProUGUI>().text = num.ToString() + " %";

        }

    }

    public void ActualizeStatsRecourses(int[] statsValues)
    {
        string maxValue = "999";
        for (int i = 0; i < statsValues.Length; i++)
        {
            switch (i)
            {
                case 0:
                    if (statsValues[i] >= 999)
                    {
                        amountClothesTXT.text = maxValue;
                    }
                    else
                    {
                        amountClothesTXT.text = statsValues[i].ToString();

                    }
                    continue;
                case 1:
                    if (statsValues[i] >= 999)
                    {
                        amountLinenTXT.text = maxValue;
                    }
                    else
                    {
                        amountLinenTXT.text = statsValues[i].ToString();
                    }
                    continue;
                case 2:
                    if (statsValues[i] >= 999)
                    {
                        amountOilTXT.text = maxValue;
                    }
                    else
                    {
                        amountOilTXT.text = statsValues[i].ToString();
                    }
                    continue;
                case 3:
                    if (statsValues[i] >= 999)
                    {
                        amountWineTXT.text = maxValue;
                    }
                    else
                    {
                        amountWineTXT.text = statsValues[i].ToString();
                    }
                    continue;
                case 4:
                    if (statsValues[i] >= 999)
                    {
                        amountBreadTXT.text = maxValue;
                    }
                    else
                    {
                        amountBreadTXT.text = statsValues[i].ToString();
                    }
                    continue;
                case 5:
                    if (statsValues[i] >= 999)
                    {
                        amountCeramicsTXT.text = maxValue;
                    }
                    else
                    {
                        amountCeramicsTXT.text = statsValues[i].ToString();
                    }
                    continue;
                case 6:
                    if (statsValues[i] >= 999)
                    {
                        amountMeatTXT.text = maxValue;
                    }
                    else
                    {
                        amountMeatTXT.text = statsValues[i].ToString();
                    }
                    continue;
                case 7:
                    if (statsValues[i] >= 999)
                    {
                        amountWheatTXT.text = maxValue;
                    }
                    else
                    {
                        amountWheatTXT.text = statsValues[i].ToString();
                    }
                    continue;
                case 8:
                    if (statsValues[i] >= 999)
                    {
                        amountWoodTXT.text = maxValue;
                    }
                    else
                    {
                        amountWoodTXT.text = statsValues[i].ToString();
                    }
                    continue;
                case 9:
                    if (statsValues[i] >= 999)
                    {
                        amountGoldTXT.text = maxValue;
                    }
                    else
                    {
                        amountGoldTXT.text = statsValues[i].ToString();
                    }
                    continue;
                case 10:
                    if (statsValues[i] >= 999)
                    {
                        amountMarbleTXT.text = maxValue;
                    }
                    else
                    {
                        amountMarbleTXT.text = statsValues[i].ToString();
                    }

                    continue;
                case 11:
                    if (statsValues[i] >= 999)
                    {
                        amountSilverTXT.text = maxValue;
                    }
                    else
                    {
                        amountSilverTXT.text = statsValues[i].ToString();
                    }
                    continue;

                case 12:
                    if (statsValues[i] >= 999)
                    {
                        amountIronTXT.text = maxValue;
                    }
                    else
                    {
                        amountIronTXT.text = statsValues[i].ToString();
                    }

                    continue;
                    case 13:
                    if (statsValues[i] >= 999)
                    {
                        amountWeaponsTXT.text = maxValue;
                    }
                    else
                    {
                        amountWeaponsTXT.text = statsValues[i].ToString();
                    }

                    continue;
                case 14:
                    if (statsValues[i] >= 999)
                    {
                        amountFaithTXT.text = maxValue;
                    }
                    else
                    {
                        amountFaithTXT.text = statsValues[i].ToString();
                    }
                    continue;

                default:
                    continue;
            }
        }
    }
    public void CreateUIConstructionBar(Vector3 position, StructureParent structure)
    {
        GameObject GOParent = new GameObject("ConstructionBarUI");
        switch (structure.GetRotation())
        {
            case RotationType.None:
                break;

            case RotationType.RightDown:
                float xPosition = position.x + (structure.columnsOcuppation / 2);
                float zPosition = position.z - (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftDown:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z - (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.RightTop:
                xPosition = position.x + (structure.columnsOcuppation / 2);
                zPosition = position.z + (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftTop:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z + (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            default:
                break;
        }
        structure.originalPositionUIAdvises = GOParent.transform.position;
        AdviseUI adviseUIConstruction = new AdviseUI();
        adviseUIConstruction.adviseUIGO = Instantiate(constructionBar, GOParent.transform.position, Quaternion.identity, canvasWorld3D.transform);
        adviseUIConstruction.stateAdviseUI = StatesStructures.Construction;
        structure.actualAdvisesUI.Add(adviseUIConstruction);
        advisesUI.Add(adviseUIConstruction);
        Destroy(GOParent);
        MakeResponsiveAdvises(structure);
    }
    public void CreateUINoRoadConnection(Vector3 position, StructureParent structure)
    {
        gameManager.audioManager.NewAdvise();

        GameObject GOParent = new GameObject("NoRoadUI");
        switch (structure.GetRotation())
        {
            case RotationType.None:
                break;

            case RotationType.RightDown:
                float xPosition = position.x + (structure.columnsOcuppation / 2);
                float zPosition = position.z - (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftDown:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z - (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.RightTop:
                xPosition = position.x + (structure.columnsOcuppation / 2);
                zPosition = position.z + (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftTop:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z + (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            default:
                break;
        }
        structure.originalPositionUIAdvises=GOParent.transform.position;
        AdviseUI adviseUIRoads = new AdviseUI();
        adviseUIRoads.adviseUIGO = Instantiate(noRoadConnectionUI, GOParent.transform.position, Quaternion.identity, canvasWorld3D.transform);
        adviseUIRoads.stateAdviseUI = StatesStructures.NoRoad;
        structure.actualAdvisesUI.Add(adviseUIRoads);
        advisesUI.Add(adviseUIRoads);
        Destroy(GOParent);
        MakeResponsiveAdvises(structure);
    }

    public void ActualizeItemSelectedInInspector(StructureParent item)
    {
        //inspectorName.text = item.Name;
        //inspectorDescription.text = item.description;
        //inspectorDescription.text=item.des
        //TODO, make a ui text description of the item selected and actualize it here
    }

    public void CreateUINoSupply(Vector3 position, StructureParent structure)
    {
        gameManager.audioManager.NewAdvise();

        GameObject GOParent = new GameObject("NoSupplyUI");
        switch (structure.GetRotation())
        {
            case RotationType.None:
                break;

            case RotationType.RightDown:
                float xPosition = position.x + (structure.columnsOcuppation / 2);
                float zPosition = position.z - (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftDown:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z - (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.RightTop:
                xPosition = position.x + (structure.columnsOcuppation / 2);
                zPosition = position.z + (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftTop:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z + (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            default:
                break;
        }
        structure.originalPositionUIAdvises = GOParent.transform.position;
        AdviseUI adviseUIRoads = new AdviseUI();
        adviseUIRoads.adviseUIGO = Instantiate(noSupplyUI, GOParent.transform.position, Quaternion.identity, canvasWorld3D.transform);
        adviseUIRoads.stateAdviseUI = StatesStructures.NoSupply;
        structure.actualAdvisesUI.Add(adviseUIRoads);
        ///structure.actualAdvisesUI.Add(adviseUIRoads);

        advisesUI.Add(adviseUIRoads);
        Destroy(GOParent);
        MakeResponsiveAdvises(structure);

    }
    public void CreateUIElaboratedResources(Vector3 position, StructureParent structure)
    {
        gameManager.audioManager.NewAdvise();

        GameObject GOParent = new GameObject("NoElaboratedResourcesUI");
        switch (structure.GetRotation())
        {
            case RotationType.None:
                break;

            case RotationType.RightDown:
                float xPosition = position.x + (structure.columnsOcuppation / 2);
                float zPosition = position.z - (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftDown:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z - (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.RightTop:
                xPosition = position.x + (structure.columnsOcuppation / 2);
                zPosition = position.z + (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftTop:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z + (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            default:
                break;
        }
        structure.originalPositionUIAdvises = GOParent.transform.position;
        AdviseUI adviseUIRoads = new AdviseUI();
        adviseUIRoads.adviseUIGO = Instantiate(noElaboratedResourcesUI, GOParent.transform.position, Quaternion.identity, canvasWorld3D.transform);
        adviseUIRoads.stateAdviseUI = StatesStructures.NoSupplyElaboratedResources;
        structure.actualAdvisesUI.Add(adviseUIRoads);
        advisesUI.Add(adviseUIRoads);
        Destroy(GOParent);
        MakeResponsiveAdvises(structure);

    }
    public void CreateUINoWorkers(Vector3 position, StructureParent structure)
    {
        gameManager.audioManager.NewAdvise();

        GameObject GOParent = new GameObject("NoWorkersUI");
        switch (structure.GetRotation())
        {
            case RotationType.None:
                break;

            case RotationType.RightDown:
                float xPosition = position.x + (structure.columnsOcuppation / 2);
                float zPosition = position.z - (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftDown:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z - (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.RightTop:
                xPosition = position.x + (structure.columnsOcuppation / 2);
                zPosition = position.z + (structure.rowsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            case RotationType.LeftTop:
                xPosition = position.x - (structure.rowsOcuppation / 2);
                zPosition = position.z + (structure.columnsOcuppation / 2);
                GOParent.transform.position = new Vector3(xPosition, position.y + heightUIWorldSpace, zPosition);
                break;

            default:
                break;
        }
        structure.originalPositionUIAdvises = GOParent.transform.position;
        AdviseUI adviseUIRoads = new AdviseUI();
        adviseUIRoads.adviseUIGO = Instantiate(noWorkersUI, GOParent.transform.position, Quaternion.identity, canvasWorld3D.transform);
        adviseUIRoads.stateAdviseUI = StatesStructures.NoWorkers;
        structure.actualAdvisesUI.Add(adviseUIRoads);
        advisesUI.Add(adviseUIRoads);
        Destroy(GOParent);
        MakeResponsiveAdvises(structure);

    }
    public void MakeResponsiveAdvises(StructureParent structure)    //for making all the advises responsive
    {
        int changenumber = 0;
        if (structure.actualAdvisesUI.Count%2==0)   //if its pair the number de advises
        {
            for (int i = 0; i < structure.actualAdvisesUI.Count; i++)
            {
                if (i%2==0)
                {
                    changenumber++;
                    structure.actualAdvisesUI[i].ReturnGO().transform.position = new Vector3(structure.originalPositionUIAdvises.x + distanceBetweenAdvisesUI*(changenumber), structure.originalPositionUIAdvises.y, structure.originalPositionUIAdvises.z);

                }
                else
                {
                    structure.actualAdvisesUI[i].ReturnGO().transform.position = new Vector3(structure.originalPositionUIAdvises.x - distanceBetweenAdvisesUI * (changenumber), structure.originalPositionUIAdvises.y, structure.originalPositionUIAdvises.z);

                }
            }
        }
        else
        {
            for (int i = 0; i < structure.actualAdvisesUI.Count; i++)
            {

                if (i== 0)
                {
                    changenumber++;
                    structure.actualAdvisesUI[i].ReturnGO().transform.position = new Vector3(structure.originalPositionUIAdvises.x, structure.originalPositionUIAdvises.y, structure.originalPositionUIAdvises.z);

                }
                else if (i%2==0)
                {
                    structure.actualAdvisesUI[i].ReturnGO().transform.position = new Vector3(structure.originalPositionUIAdvises.x + distanceBetweenAdvisesUI * (changenumber), structure.originalPositionUIAdvises .y, structure.originalPositionUIAdvises.z);

                }
                else
                {
                    structure.actualAdvisesUI[i].ReturnGO().transform.position = new Vector3(structure.originalPositionUIAdvises.x - distanceBetweenAdvisesUI * (changenumber), structure.originalPositionUIAdvises.y, structure.originalPositionUIAdvises.z);

                }

            }

        }
    }
    public void ControlPauseMenuActive()
    {
        pauseMenu.PauseByClickingItem();
    }

    public void ControlPauseMenuDesactive()
    {
        pauseMenu.StopPause();
    }
}