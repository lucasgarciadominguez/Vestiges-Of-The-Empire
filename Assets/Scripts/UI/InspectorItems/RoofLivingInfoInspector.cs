using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofLivingInfoInspector : MonoBehaviour
{
    public string Name;
    public string Description;
    public List<AdviseUI> listStates;
    public List<Person> poblation;
    public void LoadItemsInTheInspectorHouseBuilding(string nameHouse, string descripttion, HouseBuilding houseType)
    {
        Name = nameHouse;
        Description = descripttion;
        listStates = houseType.actualAdvisesUI;
        poblation = houseType.peopleLivingInHouse;
    }
}
