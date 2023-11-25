using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vein : MonoBehaviour
{
    [SerializeField]
    int totalSupply;
    [SerializeField]
    Mineral mineralType =Mineral.None;
    [SerializeField]
    Vector3 location;
    private void Start()
    {
        AddVein(mineralType);
        
    }
    public void SetMineralType(Mineral mineral)
    {
        this.mineralType = mineral;
    }
    public void SetLocation(Vector3 location)
    {
        this.location = location;
        transform.position = location;
    }
    public Vector3 GetLocation()
    {
        return location;
    }
    public void AddVein(Mineral type)
    {
        FindObjectOfType<Veins>().AddVein(this, type);
    }
    public void SetSupply(int supply)
    {
        this.totalSupply = supply;
    }
    public void ReduceAmount(int reduction)
    {

        totalSupply -= reduction;
        foreach (ResourcesType i in Enum.GetValues(typeof(ResourcesType)))
        {
            if (i.ToString() == mineralType.ToString())
            {
                FindObjectOfType<GameManager>().IncreaseRecourseAmount(i, reduction);

            }
        }

        

    }
    public bool HasSupply(int reduction)
    {
        if ((totalSupply -= reduction) <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
