using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MineralValueChanged : MonoBehaviour
{
    public TMP_Dropdown dropdownMinerals;
    public ShaderGrid shaderGrid;
    // Start is called before the first frame update
    void Start()
    {
        //assigned on value changed event
        dropdownMinerals.onValueChanged.AddListener(delegate
        { mineralsOnValueChanged(dropdownMinerals); 
        });
    }

    public void mineralsOnValueChanged(TMP_Dropdown sender)
    {
        //shaderGrid.ChangeMaterialSelected(sender.value);
    }
}
