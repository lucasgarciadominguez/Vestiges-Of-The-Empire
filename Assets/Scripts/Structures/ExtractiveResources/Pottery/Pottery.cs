using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pottery : ExtractiveResourcesJobBuilding
{
    // Start is called before the first frame update
    private void Start()
    {
        cellType = CellType.Pottery;
        functionJob = new FunctionTimer(FunctionPottery, timeForDoingFunctionJob, true);
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

    public void FunctionPottery()
    {
        trueEfectiviness = extractedSupplyForEachCycleJob;
        UpdateResourcesForTheGameManager();
        //TODO, make a function that checks the distance to water
    }
}