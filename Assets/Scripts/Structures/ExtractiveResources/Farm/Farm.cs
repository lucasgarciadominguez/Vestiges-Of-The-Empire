using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : ExtractiveResourcesJobBuilding
{
    // Start is called before the first frame update
    private void Start()
    {
        resourceExtractingOrProducing = ResourcesType.Meat;
        cellType = CellType.Farm;

        functionJob = new FunctionTimer(GetAmountFarmResource, timeForDoingFunctionJob, true);
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

    public void GetAmountFarmResource()
    {
        if (GetRatioEfectiviness() > 35)
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
        trueEfectiviness = extractedSupplyForEachCycleJob * efectiveness;
        return trueEfectiviness;
    }
}