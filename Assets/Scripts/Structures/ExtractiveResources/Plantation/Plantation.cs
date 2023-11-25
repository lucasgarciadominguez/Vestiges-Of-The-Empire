using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantation : ExtractiveResourcesJobBuilding
{
    // Start is called before the first frame update
    private void Start()
    {
        cellType = CellType.Plantation;
        functionJob = new FunctionTimer(GetAmountPlantationResource, timeForDoingFunctionJob, true);
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

    public void GetAmountPlantationResource()
    {
        if (GetRatioEfectiviness() > 25)
        {
            UpdateResourcesForTheGameManager();
        }
        else
        {
            states.Add(StatesStructures.NoSupply);
            CreateUIForNewState(StatesStructures.NoSupply);  //makes the ui show that there is no supply
        }
    }

    private int GetRatioEfectiviness()
    {
        float efectiviness = 0.5f * efectiveness;
        float trueEfectiviness = extractedSupplyForEachCycleJob * efectiviness;
        this.trueEfectiviness = (int)trueEfectiviness;
        return this.trueEfectiviness;
    }
}