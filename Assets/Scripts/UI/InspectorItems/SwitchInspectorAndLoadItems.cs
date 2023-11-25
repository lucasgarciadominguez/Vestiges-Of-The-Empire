using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum TypeInspectorSelected
{
    None,
    ExtractiveResources,
    HouseBuildings,
    ElaboratedResources,
}
public class SwitchInspectorAndLoadItems : MonoBehaviour
{
    [SerializeField]
    InspectorItemsElaboratedResources elaboratedResourcesInspector;
    [SerializeField]
    InspectorItemsExtractiveResources extractiveResourcesInspector;
    [SerializeField]
    InspectorItemsHouseBuilding houseInspector;
    [SerializeField]
    InspectorItemsPerson personInspector;
    public void LoadInspector(TypeInspectorSelected typeInspectorSelected,StructureParent structure)
    {
        HideInspectors();
        switch (typeInspectorSelected)
        {
            case TypeInspectorSelected.None:
                break;
            case TypeInspectorSelected.ExtractiveResources:
                LoadExtractiveInspector(structure);
                break;
            case TypeInspectorSelected.HouseBuildings:
                LoadHouseInspector(structure);
                break;
            case TypeInspectorSelected.ElaboratedResources:
                LoadElaboratedInspector(structure);
                break;
            default:
                break;
        }
    }
    public void HideInspectors()
    {
        elaboratedResourcesInspector.gameObject.SetActive(false);
        extractiveResourcesInspector.gameObject.SetActive(false);
        houseInspector.gameObject.SetActive(false);
        personInspector.gameObject.SetActive(false);
    }
    public void LoadPersonInspector(PersonInfoInspector person)
    {
        HideInspectors();
        personInspector.LoadItemsInTheInspectorPerson(person.Name,person.Description,person.House,person.HouseItem,person.Work,person.WorkItem,
            person.personFamily,person.houseBuilding,person.Status,person.demands1);
        personInspector.gameObject.SetActive(true);

    }
    public void LoadHouseInspector(StructureParent structure)
    {
        HideInspectors();
        HouseBuilding houseBuilding = structure.GetComponent<HouseBuilding>();
        string nameType= houseBuilding.Name;
        string descriptionType = houseBuilding.description;
        houseInspector.LoadItemsInTheInspectorForHouse(nameType,descriptionType, houseBuilding.actualAdvisesUI,houseBuilding.peopleLivingInHouse);

        houseInspector.gameObject.SetActive(true);

    }
    public void LoadHouseInspector(RoofLivingInfoInspector structure)
    {
        HideInspectors();
        houseInspector.LoadItemsInTheInspectorForHouse(structure.Name, structure.Description, structure.listStates, structure.poblation);

        houseInspector.gameObject.SetActive(true);

    }
    public void LoadExtractiveInspector(StructureParent structure)
    {
        HideInspectors();
        ExtractiveResourcesJobBuilding extractiveResources = structure.GetComponent<ExtractiveResourcesJobBuilding>();
        string nameType = extractiveResources.Name;
        string descriptionType = extractiveResources.description;
        Sprite image = extractiveResources.spriteStructure;
        string resourcesProduced = extractiveResources.resourceExtractingOrProducing.ToString();
        string amountResourcesProduced= extractiveResources.trueEfectiviness.ToString();
        extractiveResourcesInspector.LoadItemsInTheInspectorForExtractiveResources(nameType, descriptionType, resourcesProduced, image, amountResourcesProduced,extractiveResources.actualAdvisesUI,extractiveResources.workers);

        extractiveResourcesInspector.gameObject.SetActive(true);

    }
    public void LoadElaboratedInspector(StructureParent structure)
    {
        HideInspectors();
        ElaboratedResourcesJobBuilding elaboratedResources = structure.GetComponent<ElaboratedResourcesJobBuilding>();
        string nameType = elaboratedResources.Name;
        string descriptionType = elaboratedResources.description;

        Sprite imageProduced = elaboratedResources.spriteStructure;
        string resourcesProduced = elaboratedResources.resourceExtractingOrProducing.ToString();
        string amountResourcesProduced = elaboratedResources.producedSupplyForEachCycleJob.ToString();

        Sprite imageItemNeeded = elaboratedResources.imageItemNeeded;
        string resourcesNeeded = elaboratedResources.resourceNeeded.TypeItemNeeded.ToString();
        string amountresourcesNeeded = elaboratedResources.resourceNeeded.amountItem.ToString();

        elaboratedResourcesInspector.LoadItemsInTheInspectorForElaboratedResources(nameType, descriptionType,resourcesNeeded,imageItemNeeded,amountresourcesNeeded, resourcesProduced, imageProduced, amountResourcesProduced, elaboratedResources.actualAdvisesUI,elaboratedResources.workers);

        elaboratedResourcesInspector.gameObject.SetActive(true);

    }
}
