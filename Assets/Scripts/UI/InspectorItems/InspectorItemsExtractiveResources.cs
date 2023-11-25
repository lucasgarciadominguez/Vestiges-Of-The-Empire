using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InspectorItemsExtractiveResources : MonoBehaviour
{
    /// <summary>
    /// Extractive Resources
    /// </summary>
    [SerializeField]
    Sprite unemployedSprite;
    [SerializeField]
    TextMeshProUGUI nameElaboratedResourcesText;
    [SerializeField]
    TextMeshProUGUI elaboratedResourcesDescriptionText;
    [SerializeField]
    GameObject[] statesStructuresGO;
    [SerializeField]
    TextMeshProUGUI resourcesProducedText;
    [SerializeField]
    Image resourcesProducedSprite;
    [SerializeField]
    TextMeshProUGUI resourcesProducedAmount;
    [SerializeField]
    Image[] imagesWorkers;

    public void LoadItemsInTheInspectorForExtractiveResources(string namePerson, string description,
        string resourcesProduced, Sprite resourcesProducedSprite, string resourcesProducedAmount,List<AdviseUI> listAdvises,List<Person> listWorkers)
    {
        HideAndShowAdvises(listAdvises);
        HideAndShowPeople(listWorkers);

        nameElaboratedResourcesText.text = namePerson;
        elaboratedResourcesDescriptionText.text = description;
        this.resourcesProducedText.text = resourcesProduced;
        this.resourcesProducedSprite.sprite= resourcesProducedSprite;
        this.resourcesProducedSprite.sprite = resourcesProducedSprite;
        this.resourcesProducedAmount.text = resourcesProducedAmount;
    }
    void HideAndShowPeople(List<Person> people)
    {
        bool checkExit = false;
        int counter = 0;
        foreach (var item in imagesWorkers)
        {
            item.enabled = false;
            item.sprite= null;
        }

        Debug.Log(people.Count);
        for (int i = 0; i < people.Count; i++)
        {
            for (int x = 0; x < imagesWorkers.Length; x++)
            {

                if (imagesWorkers[x].sprite == null && !checkExit)
                {
                    imagesWorkers[x].enabled = true;
                    string name = people[i].Name + " " + people[i].SurnameFamily;

                    imagesWorkers[x].GetComponent<PersonInfoInspector>().LoadItemsInTheInspectorPerson(name, people[i].Description, people[i].roofNeeded, people[i].hasEmploy, people[i].job, people[i].job.spriteStructure, people[i].family, people[i].personType.ToString(), people[i].demands);
                    imagesWorkers[x].sprite = people[i].SpritePerson;
                    counter++;
                }
                if (counter == people.Count)
                {
                    checkExit = true;

                }
            }

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
