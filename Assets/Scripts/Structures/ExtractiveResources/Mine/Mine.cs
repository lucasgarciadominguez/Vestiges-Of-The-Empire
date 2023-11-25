using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mineral
{
    None,
    Gold,
    Iron,
    Marble
}

public class Mine : ExtractiveResourcesJobBuilding
{

    [SerializeField]
    private int extractedSupply;


    public int totalAmountVeinUsing;

    // Start is called before the first frame update
    private void Start()
    {
        cellType = CellType.Mine;
        functionJob = new FunctionTimer(GetAmountMineResource, timeForDoingFunctionJob, true);
        SetAllJobRequirements();

    }
    private void Update()
    {
        UpdateAllStructureRequirements();
        if (isBuild)
        {
            UpdateAllJobRequirements();
            if (canWork)
            {
                functionJob.Update();
            }
        }
    }
    private void ReduceVeinSupply()
    {
        if (canWork)
        {
            GetRatioEfectiviness();
            if ((totalAmountVeinUsing - trueEfectiviness) >0)
            {
                totalAmountVeinUsing -= trueEfectiviness;
            }
            else
            {
                states.Add(StatesStructures.NoSupply);
                CreateUIForNewState(StatesStructures.NoSupply);  //makes the ui show that there is no supply
            }
        }
        else
        {
            Debug.Log("No connection or something");
        }
    }

    public void GetAmountMineResource()
    {
        GetRatioEfectiviness();
        if (extractedSupply < totalAmountVeinUsing)
        {
            UpdateResourcesForTheGameManager();
        }
        else
        {
            states.Add(StatesStructures.NoSupply);
            CreateUIForNewState(StatesStructures.NoSupply);  //makes the ui show that there is no supply
            canWork= false;
        }
    }

    private void GetRatioEfectiviness()
    {
        float efectiviness = 0.5f * efectiveness;
        float trueEfectiviness = extractedSupplyForEachCycleJob * efectiviness;
        this.trueEfectiviness= (int)trueEfectiviness;
        extractedSupply += this.trueEfectiviness;
    }
}