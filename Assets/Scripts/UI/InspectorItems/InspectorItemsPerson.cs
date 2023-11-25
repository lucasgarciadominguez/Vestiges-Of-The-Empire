using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectorItemsPerson : MonoBehaviour
{
    /// <summary>
    /// PERSON
    /// </summary>
    [SerializeField]
    Sprite unemployedSprite;
    [SerializeField]
    TextMeshProUGUI namePersonText;
    [SerializeField]
    TextMeshProUGUI descriptionPersonText;
    [SerializeField]
    TextMeshProUGUI houseText;
    [SerializeField]
    Image imageHouse;
    [SerializeField]
    TextMeshProUGUI workText;
    [SerializeField]
    Image imageWork;
    [SerializeField]
    TextMeshProUGUI familyText;
    [SerializeField]
    Image imageFamily;
    [SerializeField]
    TextMeshProUGUI statusText;
    [SerializeField]
    Image[] demandsGO;
    [SerializeField]
    TextMeshProUGUI demandsPercentHappiness;
    [SerializeField]
    Demand[] demands;
    public void LoadItemsInTheInspectorPerson(string namePerson,string descripttion , string houseType,Sprite houseSprite, string work,Sprite workSprite, Person family,HouseBuilding house, string socialClass, Demand[] demands)
    {
        this.demands = demands;
        HideAndShowDemands(demands,socialClass);
        namePersonText.text = namePerson;
        descriptionPersonText.text = descripttion;
        houseText.text = houseType;
        imageHouse.sprite= houseSprite;
        workText.text = work;
        imageWork.sprite= workSprite;
        familyText.text = family.Name;
        imageFamily.sprite= family.SpritePerson;
        statusText.text = socialClass;
        if (imageFamily.gameObject.GetComponent<PersonInfoInspector>() != null)
        {
            if (family.hasEmploy)
            {
               imageFamily.GetComponent<PersonInfoInspector>().LoadItemsInTheInspectorPerson(family.Name, family.Description, family.roofNeeded, family.hasEmploy,family.job, family.job.spriteStructure, family.family, family.personType.ToString(),family.demands);

            }
            else
            {
                imageFamily.GetComponent<PersonInfoInspector>().LoadItemsInTheInspectorPerson(family.Name, family.Description, family.roofNeeded, family.hasEmploy, null, unemployedSprite, family.family, family.personType.ToString(),family.demands);

            }


        }
        if (imageHouse.gameObject.GetComponent<RoofLivingInfoInspector>() != null)
        {
            if (house!=null)
            {
                imageHouse.GetComponent<RoofLivingInfoInspector>().LoadItemsInTheInspectorHouseBuilding(house.Name, house.description,house);

            }
            else
            {
                Debug.LogWarning("No house for the family");
            }


        }
    }
    void HideAndShowDemands(Demand[] demands,string socialClass)
    {
        foreach (var item in demandsGO)
        {
            item.enabled = false;
            //item.sprite = null;
        }
        switch (socialClass)
        {
            case "Plebeian":
                demandsGO[0].enabled=true;
                demandsGO[1].enabled=true;

                break;
            case "Freedman":
                demandsGO[0].enabled = true;
                demandsGO[1].enabled = true;
                demandsGO[2].enabled = true;
                demandsGO[5].enabled = true;
                demandsGO[4].enabled = true;

                break;
            case "Patrician":
                demandsGO[0].enabled = true;
                demandsGO[1].enabled = true;
                demandsGO[2].enabled = true;
                demandsGO[3].enabled = true;
                demandsGO[4].enabled = true;
                demandsGO[5].enabled = true;
                break;
            default:
                break;
        }
    }
    public void ShowDemands(int i)
    {
        switch (i)
        {
            case 0:
                demandsPercentHappiness.text= demands[0].PercentHappiness.ToString()+"%";
                break;
            case 1:
                demandsPercentHappiness.text = demands[1].PercentHappiness.ToString() + "%";
                break;
            case 2:
                demandsPercentHappiness.text = demands[2].PercentHappiness.ToString() + "%";
                break;
            case 3:
                demandsPercentHappiness.text = demands[3].PercentHappiness.ToString() + "%";
                break;
            case 4:
                demandsPercentHappiness.text = demands[4].PercentHappiness.ToString() + "%";
                break;
            case 5:
                demandsPercentHappiness.text = demands[5].PercentHappiness.ToString() + "%";
                break;
            default:
                break;
        }
    }
}
