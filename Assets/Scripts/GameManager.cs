using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum ResourcesType
{
    None,
    Clothes,
    Linen,
    Oil,
    Wine,
    Bread,
    Ceramics,
    Meat,
    Wheat,
    Wood,
    Gold,
    Marble,
    Iron,
    Silver,
    Weapons,
    Faith
}

public enum ControllerMode
{
    None,
    Play,
    BuildPlaceHolder,
    Build,
    BuildMenu,
    Menu
}

public class GameManager : MonoBehaviour, IShopInterface
{
    [SerializeField]
    ItemNeeded[] itemsNeededForFinishingGame;
    [SerializeField]
    private Texture2D cursorArrow;

    public CameraController cam;
    public Camera cameraRenderer;
    public ObjectDetector detector;
    public PlaceBuilding builder;
    [SerializeField]
    public AudioManager audioManager;
    [SerializeField]
    ExplanationMessagesUI explanationMessagesUI;
    public GameObject actualGOselected;
    public GameObject highlightedEffectCircle;

    [SerializeField]
    public List<ItemNeeded> items;

    [SerializeField]
    private int amountFreePeople = 0;

    [SerializeField]
    private int amountSlaves = 0;

    [SerializeField]
    private UIManager uiManager;

    public ControllerMode state = ControllerMode.None;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private LayerMask structureLayer;

    public bool placeHolderMode = false;
    public bool inspectorSelected = false;

    private void Start()
    {
        SetCustomizedCursor();
        ChangeState(ControllerMode.Play);
        RefreshUI();
    }
    public void CheckIfItsOver()
    {

    }
    private void Update()
    {
        
        if (state != ControllerMode.Build)
        {
            if (inputManager.isClickedDown)
            {
                explanationMessagesUI.ReturnToDefaultPosition();
                Ray actualRay = cam.PosicionPuntero();
                if (detector.RayCastGround(actualRay, structureLayer) != null)
                {
                    if (highlightedEffectCircle != null)
                    {
                        highlightedEffectCircle.SetActive(false);
                    }
                    actualGOselected = detector.RayCastGround(actualRay, structureLayer);
                    if (actualGOselected!=null)
                    {
                        uiManager.OpenInspectorNeeded(actualGOselected);
                        audioManager.OpenInspector();
                        inspectorSelected = true;

                    }
                    highlightedEffectCircle = actualGOselected.GetComponentInParent<StructureParent>().sphereRadius;  //gets the highlighted box and marks activates for showing that is selected the gameobject
                    if (highlightedEffectCircle != null)
                    {
                        highlightedEffectCircle.SetActive(true);
                        UpdateUIInspector();
                    }
                    else
                    {
                        Debug.LogWarning("Doesn't contain an sphere radius for effect!");
                    }
                }
                else
                {
                    actualGOselected = null;
                    if (inspectorSelected==true)
                    {
                        uiManager.HideInspectors();
                        audioManager.CloseInspector();
                        inspectorSelected = false;

                    }
                    if (highlightedEffectCircle != null)
                    {
                        highlightedEffectCircle.SetActive(false);
                        highlightedEffectCircle = null;
                    }

                    UpdateUIInspector();
                }
            }
            else if (inputManager.isClickedDownRight)
            {
                if (inspectorSelected == true)
                {
                    uiManager.HideInspectors();
                    audioManager.CloseInspector();
                    inspectorSelected = false;

                }
                if (highlightedEffectCircle != null)
                {
                    highlightedEffectCircle.SetActive(false);
                    highlightedEffectCircle = null;
                }
            }
        }
        var rotation = cameraRenderer.transform.rotation;

        foreach (var item in uiManager.ReturnUIAdvises())
        {
            if (item.ReturnState() != StatesStructures.Construction)  //the construction bar is not looking to the player
            {
                item.ReturnGO().transform.LookAt(cameraRenderer.transform.position + rotation * Vector3.forward, rotation * Vector3.up);
            }
        }
    }
    public bool CheckIfCanBeSatisfiedDemand(ItemNeeded demand)
    {
        foreach (var item in items)
        {
            if (item.TypeItemNeeded== demand.TypeItemNeeded)
            {
                float calc=item.amountItem- demand.amountItem;
                if (calc>0)
                {
                    return true;
                }
                return false; 
            }
        }
        return false;
    }
    private void SetCustomizedCursor()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }

    public bool CanReduceStats(ItemNeeded[] resourcesNeeded)
    {
        foreach (var item in items)
        {
            foreach (var itemResource in resourcesNeeded)
            {
                if (item.TypeItemNeeded == itemResource.TypeItemNeeded)
                {
                    if (this.items.Find(j => j.TypeItemNeeded == itemResource.TypeItemNeeded).amountItem >= itemResource.amountItem)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        return false;
    }

    public void ReduceStats(ItemNeeded[] resourcesNeeded)
    {
        foreach (var item in items)
        {
            foreach (var itemResource in resourcesNeeded)
            {
                if (item.TypeItemNeeded == itemResource.TypeItemNeeded)
                {
                    this.items.Find(j => j.TypeItemNeeded == itemResource.TypeItemNeeded).ReduceAmount(Convert.ToInt32(itemResource.amountItem));
                }
            }
        }
        RefreshUI();
    }

    public bool CanBuy(ItemNeeded[] resourcesNeeded)
    {
        int resourcesNumToCheck = resourcesNeeded.Length;
        int count = 0;
        foreach (var item in items)
        {
            foreach (var itemResource in resourcesNeeded)
            {
                if (count == resourcesNumToCheck)
                {
                    return true;
                }
                else if (item.TypeItemNeeded == itemResource.TypeItemNeeded)
                {
                    if (item.amountItem >= itemResource.amountItem)
                    {
                        count++;
                    }
                }
            }
        }
        return false;
    }

    public void RefreshAdvisesUI(StructureParent structure)
    {
        uiManager.MakeResponsiveAdvises(structure);
    }

    public void IncreaseRecourseAmount(ResourcesType type, int value)
    {
        int i = items.FindIndex(j => j.TypeItemNeeded == type);
        ItemNeeded[] itemsArray = items.ToArray();
        itemsArray[i].amountItem += value;
        items = itemsArray.ToList();
        //switch (type)
        //{
        //    case ResourcesType.None:
        //        break;

        //    case ResourcesType.Food:
        //        foodAmount.amountItem += value;
        //        break;

        //    case ResourcesType.Wood:
        //        woodAmount += value;
        //        break;

        //    case ResourcesType.Gold:
        //        goldAmount += value;
        //        break;

        //    case ResourcesType.Iron:
        //        ironAmount += value;
        //        break;

        //    case ResourcesType.Marble:
        //        marbleAmount += value;
        //        break;

        //    default:
        //        break;
        //}
        RefreshUI();
    }

    public void RefreshUIPoblation(TypePeople type, int count)
    {
        if (type == TypePeople.FreePeople)
            uiManager.RefreshUIStatsPoblationFree(count);
        else
            uiManager.RefreshUIStatsSlaves(count);
    }

    public void RefreshUI()
    {
        List<int> resourcesQuantity = new List<int>();
        foreach (var item in items)
        {
            resourcesQuantity.Add(Convert.ToInt32(item.amountItem));
        }
        int[] resourcesQuantityArray = resourcesQuantity.ToArray();
        uiManager.ActualizeStatsRecourses(resourcesQuantityArray);
    }

    public void ShowMessagesUI(StatesStructures state, Vector3 position, StructureParent structure)
    {
        switch (state)
        {
            case StatesStructures.Construction:
                uiManager.CreateUIConstructionBar(position, structure);
                break;

            case StatesStructures.NoRoad:
                uiManager.CreateUINoRoadConnection(position, structure);
                break;

            case StatesStructures.NoSupply:
                uiManager.CreateUINoSupply(position, structure);
                break;

            case StatesStructures.NoWorkers:
                uiManager.CreateUINoWorkers(position, structure);
                break;

            case StatesStructures.NoSupplyElaboratedResources:
                uiManager.CreateUIElaboratedResources(position, structure);
                break;

            default:
                break;
        }
    }

    public void FinishBuildMode()
    {
        builder.StopBuilding();
    }
    public void DisableCamera()
    {
        cam.enabled = false;

    }
    public void EnableOrDisableBuildMenuMode()
    {
        cam.enabled = !cam.enabled;
    }

    public void EnableCamera()
    {
        cam.enabled = true;
    }

    public void DeleteUIAdvise(AdviseUI actualAdviseToDelete)
    {
        int num = uiManager.ReturnUIAdvises().FindIndex(c => c.Equals(actualAdviseToDelete));
        uiManager.ReturnUIAdvises().RemoveAt(num);
    }

    public void UpdateUIInspector()
    {
        if (actualGOselected != null)
        {
            uiManager.ActualizeItemSelectedInInspector(actualGOselected.GetComponentInParent<StructureParent>());
        }
    }

    public void ChangeState(ControllerMode typestate)
    {
        switch (typestate)
        {
            case ControllerMode.None:
                state = ControllerMode.None;
                break;

            case ControllerMode.Play:
                state = ControllerMode.Play;
                break;

            case ControllerMode.Build:
                state = ControllerMode.Build;
                break;

            case ControllerMode.BuildMenu:
                state = ControllerMode.BuildMenu;
                break;

            case ControllerMode.Menu:
                state = ControllerMode.Menu;
                break;

            default:
                Debug.Log("hhh");
                break;
        }
    }
}