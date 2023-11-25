using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class ItemNeeded
{
    public ResourcesType TypeItemNeeded;
    public float amountItem;

    public void ReduceAmount(int num)
    {

        this.amountItem -= (float)num;
    }
}
[System.Serializable]
public class Demand
{
    public ResourcesType TypeItemNeeded;
    public float AmountItemDemanded;
    public float PercentHappiness;
    public bool canBeSatisfied;
    public float amountSatisfy=10f;
    public float amountUnSatisfy = 10f;
    public Demand(ResourcesType itemNeeded,float amountItemDemanded) 
    {
        this.TypeItemNeeded = itemNeeded;
        this.AmountItemDemanded= amountItemDemanded;
        PercentHappiness = 70f;
        canBeSatisfied= false;
    }
    public void SatisfyDemand()
    {
        Debug.Log("Satisfy OK");
        if ((PercentHappiness+ amountSatisfy) >100)
        {
            PercentHappiness = 100f;
        }
        else
        {
            PercentHappiness += amountSatisfy;

        }
    }
    public void SatisfyDemandIfThePersonDoesntHaveABuildingNear()
    {
       // Debug.Log("Not satysfing at all");
        if ((PercentHappiness + amountSatisfy) > 100)
        {
            PercentHappiness = 100f;
        }
        else
        {
            PercentHappiness += (int)(amountSatisfy*0.33f);

        }
    }
    public void UnSatisfyDemand()
    {
        if ((PercentHappiness - amountUnSatisfy) <=0)
        {
            PercentHappiness = 0;
        }
        else
        {
            PercentHappiness -= amountUnSatisfy;

        }
    }
    //public void CalculatePercentHapiness(float amountItemGiven)
    //{
    //    float calc = (amountItemGiven * 100) / AmountItemDemanded;
    //    calc = (int)calc;
    //    if (calc > 100 )
    //        PercentHappiness = 100;
    //    else if(calc<0)
    //        PercentHappiness = 0;

    //}
}
