using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StructureType
{
    DeadEnd,
    Corner,
    ThreeWay,
    FourWay,
    Bridge
}

public class StructureModel : MonoBehaviour
{
    private float yHeight = -0.035f;

    public void CreateModel(GameObject model)
    {
        var structure = Instantiate(model, transform); //creates a new gameObject child with the new model passed by with the transform values as 0,0,0
    }

    public void SwapModel(StructureType model, Quaternion rotation)
    {
        GameObject structure = transform.GetChild(0).gameObject;
        int childrens = structure.transform.childCount;
        for (int i = 0; i < childrens; i++)
        {
            structure.transform.GetChild(i).gameObject.SetActive(false);
        }

        structure = transform.GetChild(0).gameObject;

        switch (model)
        {
            case StructureType.DeadEnd:
                structure = structure.transform.GetChild(0).gameObject;
                break;

            case StructureType.Corner:
                structure = structure.transform.GetChild(1).gameObject;

                break;

            case StructureType.ThreeWay:
                structure = structure.transform.GetChild(2).gameObject;

                break;

            case StructureType.FourWay:
                structure = structure.transform.GetChild(3).gameObject;

                break;

            case StructureType.Bridge:
                structure = structure.transform.GetChild(4).gameObject;

                break;

            default:
                break;
        }
        Transform[] childrens2 = structure.GetComponentsInChildren<Transform>();
        foreach (var item in childrens2)
        {
            item.gameObject.SetActive(true);
        }
        //structure.SetActive(true);
        structure.transform.localPosition = new Vector3(0, yHeight, 0);
        structure.transform.localRotation = rotation;
    }
}