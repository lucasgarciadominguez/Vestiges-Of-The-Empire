using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonInfoInspector : MonoBehaviour
{
    public string Name;
    public string Description;
    public Sprite HouseItem;
    public string House;
    public HouseBuilding houseBuilding;
    public Sprite WorkItem;
    public string Work;
    public Person personFamily;
    public string personFamilyText;
    public Sprite personFamilySprite;
    public string Status;
    public Demand[] demands1;
    public void LoadItemsInTheInspectorPerson(string namePerson, string descripttion, HouseBuilding houseType,bool canWork, JobBuilding work,Sprite spriteEmploy,Person family, string socialClass, Demand[] demands)
    {
        Name = namePerson;
        Description = descripttion;
        if (houseType!=null)
        {
            House = houseType.Name;
            HouseItem = houseType.spriteStructure;
            houseBuilding = houseType;
        }

        if (canWork)
        {
            if (work!=null)
            {
                Work = work.Name;
                WorkItem = spriteEmploy;
            }

        }
        else
        {
            Work = "Unemployed";
            WorkItem = spriteEmploy;

        }
        if (family!=null)
        {
            personFamilyText = family.Name;
            personFamilySprite = family.SpritePerson;
            personFamily = family;
        }

        Status = socialClass;
        demands1= demands;
    }
}
