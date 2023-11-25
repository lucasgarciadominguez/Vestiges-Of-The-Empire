using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlantationAndFarmValueChanged : MonoBehaviour
{
    public CellType type;
    public TMP_Dropdown dropdownFarmsOrPlantation;
    public PlantationAndFarm plantationAndFarm;
    // Start is called before the first frame update
    void Start()
    {
        //assigned on value changed event
        dropdownFarmsOrPlantation.onValueChanged.AddListener(delegate
        {
            FarmsOrPlantationsOnValueChanged(dropdownFarmsOrPlantation);
        });
    }

    public void FarmsOrPlantationsOnValueChanged(TMP_Dropdown sender)
    {
        plantationAndFarm.ChangeMaterialSelected(sender.value,type);
    }
}
