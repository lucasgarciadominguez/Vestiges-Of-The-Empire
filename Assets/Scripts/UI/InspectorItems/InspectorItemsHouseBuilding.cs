using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InspectorItemsHouseBuilding : MonoBehaviour
{
    /// <summary>
    /// Extractive Resources
    /// </summary>
    /// 
    [SerializeField]
    Sprite unemployedSprite;
    [SerializeField]
    TextMeshProUGUI nameElaboratedResourcesText;
    [SerializeField]
    TextMeshProUGUI elaboratedResourcesDescriptionText;
    [SerializeField]
    GameObject[] statesStructuresGO;
    [SerializeField]
    Image[] imagesPopulationHouse;

    public void LoadItemsInTheInspectorForHouse(string namePerson, string description,List<AdviseUI> adviseUIs,List<Person> peopleLiving)
    {
        HideAndShowAdvises(adviseUIs);
        HideAndShowPeople(peopleLiving);
        nameElaboratedResourcesText.text = namePerson;
        elaboratedResourcesDescriptionText.text = description;
    }
    void HideAndShowPeople(List<Person> people)
    {
        foreach (var item in imagesPopulationHouse)
        {
            item.enabled = false;
            item.sprite = null;
        }
        for (int i = 0; i < people.Count; i++)
        {

            imagesPopulationHouse[i].enabled = true;
            string name = people[i].Name + " " + people[i].SurnameFamily;
            Debug.Log(name);
            if (people[i].hasEmploy)
            {
                imagesPopulationHouse[i].GetComponent<PersonInfoInspector>().LoadItemsInTheInspectorPerson(name, people[i].Description, people[i].roofNeeded, people[i].hasEmploy, people[i].job, people[i].job.spriteStructure, people[i].family, people[i].personType.ToString(), people[i].demands);

            }
            else
            {
                imagesPopulationHouse[i].GetComponent<PersonInfoInspector>().LoadItemsInTheInspectorPerson(name, people[i].Description, people[i].roofNeeded, people[i].hasEmploy, null, unemployedSprite, people[i].family, people[i].personType.ToString(), people[i].demands);

            }
            imagesPopulationHouse[i].sprite = people[i].SpritePerson;
        }
    }
    void HideAndShowAdvises(List<AdviseUI> adviseUIs)
    {
        foreach (var item in statesStructuresGO)
        {
            item.SetActive(false);
        }
        foreach (var item in adviseUIs)
        {
            if (item.ReturnState() == StatesStructures.NoRoad)
            {
                statesStructuresGO[0].SetActive(true);
            }
            else if (item.ReturnState() == StatesStructures.NoSupply)
            {
                statesStructuresGO[1].SetActive(true);

            }
            else if (item.ReturnState() == StatesStructures.NoWorkers)
            {
                statesStructuresGO[2].SetActive(true);

            }
            else if (item.ReturnState() == StatesStructures.NoSupplyElaboratedResources)
            {
                statesStructuresGO[3].SetActive(true);

            }
        }
    }
}
